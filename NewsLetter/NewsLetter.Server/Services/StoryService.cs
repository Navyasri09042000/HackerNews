using Microsoft.Extensions.Caching.Memory;
using NewsLetter.Server.Cache;
using NewsLetter.Server.Model;

namespace NewsLetter.Server.Services
{
    public class StoryService : IStoryService
    {
        private readonly IHnClient _client;
        private readonly IMemoryCache _cache;
        private readonly CacheOptions _options;

        private const string NewestIdsCacheKey = "hn:newest:ids";

        public StoryService(IHnClient client, IMemoryCache cache, CacheOptions options)
        {
            _client = client;
            _cache = cache;
            _options = options;
        }

        public async Task<(IEnumerable<StoryDto> Items, int Total)> GetNewestAsync(
            string? query, int page, int pageSize, CancellationToken ct)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 20;

            // Get and cache newest IDs
            var ids = await _cache.GetOrCreateAsync(NewestIdsCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.NewestIdsTtlSeconds);
                var list = await _client.GetNewestIdsAsync(ct);
                return list.Take(_options.MaxItemsToHydrate).ToArray();
            }) ?? Array.Empty<int>();

            // Slice the window we need to hydrate (simple server-side paging)
            var skip = (page - 1) * pageSize;
            var window = ids.Skip(skip).Take(pageSize).ToArray();

            // Hydrate details with per-item caching
            var stories = new List<StoryDto>(window.Length);
            foreach (var id in window)
            {
                var item = await _cache.GetOrCreateAsync($"hn:item:{id}", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.StoryTtlSeconds);
                    return await _client.GetItemAsync(id, ct);
                });

                if (item is null || item.deleted == true || item.dead == true || string.IsNullOrWhiteSpace(item.title))
                    continue;

                // Fallback link if url is missing
                var link = !string.IsNullOrWhiteSpace(item.url)
                    ? item.url!
                    : $"https://news.ycombinator.com/item?id={item.id}";

                stories.Add(new StoryDto(item.id, item.title!, link, item.time));
            }

            // Optional server-side search filter (case-insensitive) applied to hydrated page
            if (!string.IsNullOrWhiteSpace(query))
            {
                var q = query.Trim();
                stories = stories
                    .Where(s => s.Title.Contains(q, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Total is total across all newest IDs (not just filtered), so UI can page accurately
            return (stories, ids.Length);
        }
    }
}
