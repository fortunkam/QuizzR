using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using QuizExperiment.Models;
using System.Text.Json;

namespace QuizExperiment.Admin.Server.Services
{
    public class AzureStorageQuestionSetService : IQuestionSetService
    {
        public AzureStorageQuestionSetService(IConfiguration configuration)
        {
            _blobContainerClient = new BlobContainerClient(configuration["Azure:Storage:ConnectionString"],
                configuration["Azure:Storage:QuizAssetsContainerName"]);
            _blobContainerClient.CreateIfNotExists();
        }

        private readonly BlobContainerClient _blobContainerClient;

        public async Task<QuestionSet> GetQuestionSet(string userId, string id)
        {
            var blobClient = _blobContainerClient.GetBlobClient($"/{userId}/{id}/quiz.json");
            var stream = await blobClient.OpenReadAsync();
            var questionSet = JsonSerializer.Deserialize<QuestionSet>(stream);
            if(questionSet is null)
            {
                throw new NullReferenceException("GetQuestionSet: questionSet is null");
            }
            return questionSet;
        }


        public async Task<QuestionSetSummary[]> GetQuestionSetsForUser(string userId)
        {
            var blobs = _blobContainerClient.GetBlobsAsync(BlobTraits.Metadata,prefix: $"{userId}").AsPages();

            var questionSets = new List<QuestionSetSummary>();

            await foreach (Azure.Page<BlobItem> blobPage in blobs)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    questionSets.Add(new QuestionSetSummary
                    {
                        Id = blobItem.Metadata["id"],
                        UserId = blobItem.Metadata["userid"],
                        Title = blobItem.Metadata["title"],
                        LastModified = blobItem.Properties.LastModified ?? DateTime.MinValue,
                        QuestionCount = blobItem.Metadata.ContainsKey("questioncount") ? int.Parse(blobItem.Metadata["questioncount"]) : null

                    });
                }
            }

            return questionSets.ToArray();

        }

        public async Task<QuestionSetSummary> UpsertQuestionSet(QuestionSet questionSet)
        {
            if (questionSet is null)
            {
                throw new NullReferenceException("UpsertQuestionSet: questionSet is null");
            }

            var blobClient = _blobContainerClient.GetBlobClient($"/{questionSet.UserId}/{questionSet.Id}/quiz.json");

            await blobClient.UploadAsync(new BinaryData(questionSet), overwrite: true);
            await blobClient.SetMetadataAsync(new Dictionary<string, string?>{
                    { "id", questionSet.Id },
                    { "userid", questionSet.UserId },
                    { "title",questionSet.Title},
                    { "questioncount",questionSet.Questions?.Count.ToString() }
                }
            );

            return new QuestionSetSummary()
            {
                Id = questionSet.Id,
                UserId = questionSet.UserId,
                Title = questionSet.Title
            };
        }

        public async Task DeleteQuestionSet(string id, string userId)
        {
            var blobs = _blobContainerClient.GetBlobsAsync(prefix: $"{userId}/{id}").AsPages();

            await foreach (Azure.Page<BlobItem> blobPage in blobs)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    var blobClient = _blobContainerClient.GetBlobClient(blobItem.Name);
                    await blobClient.DeleteIfExistsAsync();
                }
            }
        }




    }
}
