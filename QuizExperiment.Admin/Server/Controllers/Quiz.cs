using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace QuizExperiment.Admin.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Quiz : ControllerBase
    {
        private Random random = new Random();

        [HttpGet("newid")]
        public string GenerateClientQuizId([FromQuery]string questionSetId)
        {
            Debug.WriteLine($"Generating new Client Quiz Id for QuestionSet: {questionSetId}");
            var s = string.Join("", Enumerable.Range(1, 4).Select(index => random.Next(0,9).ToString()).ToArray());
            return s;
        }
    }

}
