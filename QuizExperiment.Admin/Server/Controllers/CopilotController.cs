using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using QuizExperiment.Models;
using System.Diagnostics;
using System.Text.Json;

namespace QuizExperiment.Admin.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CopilotController : ControllerBase
    {
        public CopilotController(IConfiguration configuration)
        {
            _openAIEndpoint = configuration["OpenAI:Endpoint"];
            _openAIKey = configuration["OpenAI:Key"];
            _openAIDeploymentName = configuration["OpenAI:DeploymentName"];
        }

        private readonly string _openAIEndpoint;
        private readonly string _openAIKey;
        private readonly string _openAIDeploymentName;

        [HttpGet("suggestquestion")]
        public async Task<Question> SuggestQuestion([FromQuery] string subject)
        {
            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(_openAIDeploymentName,
                _openAIEndpoint,
                _openAIKey);

            var kernel = builder.Build();
            var prompts = kernel.CreatePluginFromPromptDirectory("Prompts");

            var chatResult = await kernel.InvokeAsync(
                prompts["quiz_subject"],
                    new() {
                    { "subject", subject }
                }
            );

            var result = chatResult.ToString();

            var q = JsonSerializer.Deserialize<Question>(result) ?? throw new NullReferenceException("SuggestQuestion: Question q is null");
            q.Timeout = 20;

            return q;
        }
    }
}
