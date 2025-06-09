﻿using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using QuizExperiment.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        private const string _folderPattern = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}/(.*?)/*[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}/quiz.json";
        private readonly Regex _folderMatch = new Regex(_folderPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        private readonly BlobContainerClient _blobContainerClient;

        public async Task<QuestionSet> GetQuestionSet(string userId, string id)
        {
            var blobClient = _blobContainerClient.GetBlobClient($"/{userId}/{id}/quiz.json");
            var stream = await blobClient.OpenReadAsync();
            // Use custom options with converters for deserialization
            var options = new JsonSerializerOptions();
            options.Converters.Add(new PolymorphicQuestionConverter());
            options.Converters.Add(new PolymorphicQuestionListConverter());
            options.PropertyNamingPolicy = null;
            var questionSet = JsonSerializer.Deserialize<QuestionSet>(stream, options);
            if (questionSet is null)
            {
                throw new NullReferenceException("GetQuestionSet: questionSet is null");
            }

            // Convert legacy questions to MultipleChoiceQuestion if needed
            if (questionSet.Questions != null)
            {
                for (int i = 0; i < questionSet.Questions.Count; i++)
                {
                    var q = questionSet.Questions[i];
                    // If the type is exactly Question (not a derived type), convert to MultipleChoiceQuestion
                    if (q.GetType() == typeof(Question))
                    {
                        // Try to map legacy Question to MultipleChoiceQuestion
                        var mcq = new MultipleChoiceQuestion
                        {
                            Title = q.Title,
                            ImageUrl = q.ImageUrl,
                            Timeout = q.Timeout,
                            // Use reflection to get legacy properties if present
                            Options = (string[]?)q.GetType().GetProperty("Options")?.GetValue(q),
                            CorrectAnswerIndex = (int?)(q.GetType().GetProperty("CorrectAnswerIndex")?.GetValue(q)) ?? 0
                        };
                        questionSet.Questions[i] = mcq;
                    }
                }
            }

            return questionSet;
        }

        public async Task<QuestionSetSummary[]> GetQuestionSetsForUser(string userId)
        {
            var blobs = _blobContainerClient.GetBlobsAsync(BlobTraits.Metadata, prefix: userId).AsPages();

            var questionSets = new List<QuestionSetSummary>();

            await foreach (Azure.Page<BlobItem> blobPage in blobs)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    var folderPath = "";
                    if (_folderMatch.IsMatch(blobItem.Name))
                    {
                        folderPath = _folderMatch.Match(blobItem.Name).Groups[1].Value;
                    }
                    questionSets.Add(new QuestionSetSummary
                    {
                        Id = blobItem.Metadata["id"],
                        UserId = blobItem.Metadata["userid"],
                        Title = blobItem.Metadata["title"],
                        LastModified = blobItem.Properties.LastModified ?? DateTime.MinValue,
                        QuestionCount = blobItem.Metadata.ContainsKey("questioncount") ? int.Parse(blobItem.Metadata["questioncount"]) : null,
                        FolderPath = folderPath
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

            var questionSetId = questionSet.Id;
            if (!string.IsNullOrWhiteSpace(questionSet.FolderPath))
            {
                questionSetId = $"{questionSet.FolderPath}/{questionSet.Id}";
            }

            var blobClient = _blobContainerClient.GetBlobClient($"/{questionSet.UserId}/{questionSetId}/quiz.json");

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
