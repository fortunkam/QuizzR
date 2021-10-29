using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using QuizExperiment.Models;

namespace QuizExperiment.Admin.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Quiz : ControllerBase
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        public Quiz(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        private Random random = new Random();

        [HttpGet("generateClientQuizId")]
        public string GenerateClientQuizId([FromQuery]string questionSetId)
        {
            Debug.WriteLine($"Generating new Client Quiz Id for QuestionSet: {questionSetId}");
            var s = string.Join("", Enumerable.Range(1, 8).Select(index => random.Next(0,9).ToString()).ToArray());
            return s;
        }

        [HttpGet("getQuestionSets")]
        public QuestionSetSummary[] GetQuestionSets()
        {
            var gamesFiles = Directory.GetFiles(Path.Combine(this.Environment.ContentRootPath, "Games/"));

            var questionSets = new List<QuestionSetSummary>();
            foreach(var gameFile in gamesFiles)
            {
                var json = JsonObject.Parse(System.IO.File.ReadAllText(gameFile));

                questionSets.Add(new QuestionSetSummary()
                {
                    Id = json["id"].GetValue<string>(),
                    Title = json["title"].GetValue<string>(),
                });
            }

            return questionSets.ToArray();
        }

        [HttpGet("getQuestionSet")]
        public QuestionSet GetQuestionSet([FromQuery] string Id)
        {
            var gamesFiles = Directory.GetFiles(Path.Combine(this.Environment.ContentRootPath, "Games/"));

            var questionSets = new List<QuestionSetSummary>();
            foreach (var gameFile in gamesFiles)
            {
                var json = JsonObject.Parse(System.IO.File.ReadAllText(gameFile));

                if(json["id"].GetValue<string>() == Id)
                {
                    return JsonSerializer.Deserialize<QuestionSet>(json);
                }
            }

            throw new NullReferenceException("Unable to find a matching question set");
        }
    }

}
