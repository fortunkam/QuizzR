using Microsoft.AspNetCore.SignalR;
using QuizExperiment.Models.Client;

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

        public async Task SendQuestion(string clientQuizId, ClientQuestion question)
        {
            await Clients.Groups(clientQuizId).SendAsync("QuestionArrived", question);
        }

        public async Task SubmitAnswer(string clientQuizId, ClientAnswer answer, double timeTaken)
        {
            await Clients.Groups(clientQuizId).SendAsync("ClientAnswerReceived", clientQuizId, Context.ConnectionId, answer, timeTaken);
        }

        public async Task SendAnswerResult(string clientQuizId, string clientId, ClientAnswer answer, int currentScore, int position, bool isLastQuestion)
        {
            await Clients.Client(clientId).SendAsync("AnswerArrived", answer, currentScore, position, isLastQuestion);
        }

        public async Task SendPlayerPosition(string clientQuizId, string clientId,int position, int score)
        {
            await Clients.Client(clientId).SendAsync("PlayerPositionArrived", position, score);
        }



    }
}

