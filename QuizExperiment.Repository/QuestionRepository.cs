using QuizExperiment.Models;

namespace QuizExperiment.Repository
{
    public class QuestionRepository
    {
        public async Task<IEnumerable<QuestionSet>> GetQuestionSetsForUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Question>> GetQuestionsForSet(Guid questionSetId)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> UpsertQuestionSet(QuestionSet questionSet)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteQuestionSet(Guid questionSetId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add or Insert a new question
        /// </summary>
        /// <param name="question"></param>
        /// <returns>The new question Guid</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Guid> UpsertQuestion(Guid questionSetId, Question question)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteQuestion(Guid questionId)
        {
            throw new NotImplementedException();
        }


    }
}