using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using Azure;
using Company.Function.Models;

namespace Company.Function
{
    public class GetVotesByUserId
    {
        private readonly ILogger _logger;

        public GetVotesByUserId(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetVotesByUserId>();
        }

        [Function("GetVotesByUserId")]
        public List<Vote> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "votes/{userId}")] HttpRequestData req, string userId)
        {
            _logger.LogInformation("GetVotesByUserId funcion triggered.");

            var connectionString = Environment.GetEnvironmentVariable("TableStorageConnectionString");
            var tableName = Environment.GetEnvironmentVariable("VotesTableName");
            var tableClient = new TableClient(connectionString, tableName);

            var filterByUserId = $"UserId eq '{userId}'";

            Pageable<TableEntity> entities = tableClient.Query<TableEntity>(filter: filterByUserId);

            List<Vote> votes = new List<Vote>();
            foreach (TableEntity entity in entities)
            {
                votes.Add(new Vote
                {
                    JokeId = entity.RowKey,
                    DateSubmitted = (DateTime)(entity.GetDateTime("DateSubmitted")),
                    SubmittedBy = entity.GetString("SubmittedBy"),
                    UserId = entity.GetString("UserId"),
                    Email = entity.GetString("Email")
                });
            }

            return votes;
        }
    }
}
