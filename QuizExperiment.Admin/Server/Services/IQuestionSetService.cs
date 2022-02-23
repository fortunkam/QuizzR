using QuizExperiment.Models;

namespace QuizExperiment.Admin.Server.Services
{
    public interface IQuestionSetService
    {
        Task DeleteQuestionSet(string id, string userId);
        Task<QuestionSet> GetQuestionSet(string userId, string id);
        Task<QuestionSetSummary[]> GetQuestionSetsForUser(string userId);
        Task<QuestionSetSummary> UpsertQuestionSet(QuestionSet questionSet);
    }
}