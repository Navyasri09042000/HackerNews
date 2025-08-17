using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsLetter.Server.Services;

namespace NewsLetter.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _service;
        public StoriesController(IStoryService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetNewest(
            [FromQuery] string? query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var (items, total) = await _service.GetNewestAsync(query, page, pageSize, ct);
            return Ok(new { items, total, page, pageSize });
        }
    }
}
