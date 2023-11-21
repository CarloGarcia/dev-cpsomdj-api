using System.Net;
using System.Text.Json;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class VoteForDadJoke
    {
        private readonly ILogger _logger;

        public VoteForDadJoke(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<VoteForDadJoke>();
        }

        [Function("VoteForDadJoke")]
        public DadJoke Run([HttpTrigger(AuthorizationLevel.Function, "put")] [FromBody] Vote vote)
        {
            _logger.LogInformation("AddDadJoke function triggered.");

            var connectionString = Environment.GetEnvironmentVariable("MyStorageConnection");
            var tableName = "devdadjokes";
            var tableClient = new TableClient(connectionString, tableName);

            TableEntity entity = tableClient.GetEntity<TableEntity>("dadjokes", vote.Id);

            int currentValue  = (int)entity["NoOfVotes"];
            entity["NoOfVotes"] = currentValue + 1;
            tableClient.UpdateEntityAsync(entity, entity.ETag);

            TableEntity updatedEntity = tableClient.GetEntity<TableEntity>("dadjokes", vote.Id);

            var updatedDadJoke = new DadJoke
            {
                Id = updatedEntity.RowKey,
                Joke = updatedEntity.GetString("Joke"),
                DateSubmitted = (DateTime)(updatedEntity.GetDateTime("DateSubmitted")),
                NoOfVotes = (int)updatedEntity["NoOfVotes"],
                SubmittedBy = updatedEntity.GetString("SubmittedBy"),
                UserId = updatedEntity.GetString("UserId"),
                Email = updatedEntity.GetString("Email")
            };

            return updatedDadJoke;
        }
    }
}
