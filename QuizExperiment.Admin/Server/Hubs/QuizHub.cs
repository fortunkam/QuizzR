using Microsoft.AspNetCore.SignalR;
using QuizExperiment.Models;

namespace QuizExperiment.Admin.Server.Hubs
{
    public class QuizHub : Hub
    {

        public async Task SendAdminInfoMessage(string clientQuizId, string message)
        {
            await Clients.Groups(clientQuizId).SendAsync("AdminInfoMessage", clientQuizId, message);
        }

        public async Task RegisterQuizSession(string clientQuizId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, clientQuizId);
            await SendAdminInfoMessage(clientQuizId, "Group created");
        }

        public async Task JoinQuizSession(string clientQuizId, string userName)
        {
            //TODO: Check is quiz is running (and accepting users)
            await Groups.AddToGroupAsync(Context.ConnectionId, clientQuizId);
            await Clients.Groups(clientQuizId).SendAsync("UserJoined", clientQuizId, userName, Context.ConnectionId);
        }

        public async Task SendQuestion(string clientQuizId, string question, QuestionType questionType, string[] possibleAnswers)
        {
            await Clients.Groups(clientQuizId).SendAsync("QuestionArrived", question, questionType, possibleAnswers);
        }

        public async Task SubmitAnswer(string clientQuizId, string answer, double timeTaken)
        {
            await Clients.Groups(clientQuizId).SendAsync("ClientAnswerReceived", clientQuizId, Context.ConnectionId, answer, timeTaken);
        }

        public async Task SendAnswerResult(string clientQuizId, string clientId, string correctAnswer, int currentScore, int position)
        {
            await Clients.Client(clientId).SendAsync("AnswerArrived", correctAnswer, currentScore, position);
        }

        //public async Task SendUserMessage(string userId, string message)
        //{
        //    await Clients.Client(userId).SendAsync("UserMessage", message);
        //}


    }
}

