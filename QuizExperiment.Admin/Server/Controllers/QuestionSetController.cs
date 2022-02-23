using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizExperiment.Admin.Server.Services;
using QuizExperiment.Models;

namespace QuizExperiment.Admin.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestionSetController : ControllerBase
    {
        private readonly IQuestionSetService _questionSetService;

        public QuestionSetController(IQuestionSetService questionSetService)
        {
            _questionSetService = questionSetService;
        }

        [HttpGet("list")]
        public async Task<QuestionSetSummary[]> GetQuestionSets([FromQuery] string userId)
        {
            return await _questionSetService.GetQuestionSetsForUser(userId);
        }

        [HttpGet("get")]
        public async Task<QuestionSet> GetQuestionSet([FromQuery] string userId, [FromQuery] string id)
        {
            return await _questionSetService.GetQuestionSet(userId, id);
        }

        [HttpPost("upsert")]
        public async Task<QuestionSetSummary> UpsertQuestionSet([FromBody] QuestionSet questionSet)
        {
            return await _questionSetService.UpsertQuestionSet(questionSet);
        }

        [HttpDelete("delete")]
        public async Task DeleteQuestionSet([FromQuery] string userId, [FromQuery] string id)
        {
            await _questionSetService.DeleteQuestionSet(id,userId);
        }


    }
}
