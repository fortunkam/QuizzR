using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace QuizExperiment.Admin.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GiphyController : ControllerBase
    {
        public GiphyController(IConfiguration configuration)
        {
            _giphy = new Giphy(configuration["Giphy:ApiKey"] ?? "");
        }

        private readonly Giphy _giphy;
        [HttpGet("search")]
        public async Task<string?[]> Search([FromQuery] string query)
        {
            var result = await _giphy.GifSearch(new SearchParameter
            {
                Query = query,
                Rating = Rating.Pg,
                Limit= 10
            });

            if(result == null || result.Data == null || result.Data.Length == 0)
            {
                return Array.Empty<string?>();
            }

            return result.Data
                .Where(r => r.Images?.Original?.Url != null)
                .Select(r => r.Images?.Original?.Url).ToArray();
        }
    }
}
