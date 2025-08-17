using NewsLetter.Server.Model;

namespace NewsLetter.Server.Services
{
    public class HnClient : IHnClient
    {
        private readonly HttpClient _http;
        public HnClient(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
        }

        public async Task<IReadOnlyList<int>> GetNewestIdsAsync(CancellationToken ct)
        {
            var ids = await _http.GetFromJsonAsync<int[]>("newstories.json", ct);
            return ids ?? Array.Empty<int>();
        }

        public Task<Stories?> GetItemAsync(int id, CancellationToken ct)
            => _http.GetFromJsonAsync<Stories>($"item/{id}.json", ct);
    }
}
