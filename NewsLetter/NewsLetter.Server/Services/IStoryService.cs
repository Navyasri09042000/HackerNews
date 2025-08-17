using NewsLetter.Server.Model;

namespace NewsLetter.Server.Services
{
    public interface IStoryService
    {
        Task<(IEnumerable<StoryDto> Items, int Total)> GetNewestAsync(
            string? query,
            int page,
            int pageSize,
            CancellationToken ct);
    }
}
