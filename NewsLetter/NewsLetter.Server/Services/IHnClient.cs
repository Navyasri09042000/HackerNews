using NewsLetter.Server.Model;

namespace NewsLetter.Server.Services
{
    public interface IHnClient
    {
        Task<IReadOnlyList<int>> GetNewestIdsAsync(CancellationToken ct);
        Task<Stories?> GetItemAsync(int id, CancellationToken ct);
    }
}
