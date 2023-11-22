using System.Net;
using System.Text.Json;
using Azure.Data.Tables;
using Company.Function.Models;
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
        public DadJoke Run([HttpTrigger(AuthorizationLevel.Function, "post")] [FromBody] Vote vote)
        {
            _logger.LogInformation("AddDadJoke function triggered.");

            var connectionString = Environment.GetEnvironmentVariable("TableStorageConnectionString");
            var jokesTableName = Environment.GetEnvironmentVariable("DadJokesTableName");
            var jokesTableClient = new TableClient(connectionString, jokesTableName);
          
            TableEntity entity = jokesTableClient.GetEntity<TableEntity>("dadjokes", vote.JokeId);

            int currentValue  = (int)entity["NoOfVotes"];
            entity["NoOfVotes"] = currentValue + 1;
            jokesTableClient.UpdateEntityAsync(entity, entity.ETag);

            TableEntity updatedEntity = jokesTableClient.GetEntity<TableEntity>("dadjokes", vote.JokeId);

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

            var voteEntity = new TableEntity("dadjokes", vote.Id)
            {
                { "JokeId", vote.JokeId },
                { "DateSubmitted", DateTime.UtcNow },
                { "SubmittedBy", vote.SubmittedBy },
                { "UserId", vote.UserId },
                { "Email", vote.Email }
            };
            
            var votesTableName = Environment.GetEnvironmentVariable("VotesTableName");
            var votesTableClient = new TableClient(connectionString, votesTableName);  
            votesTableClient.AddEntity(voteEntity);

            return updatedDadJoke;
        }
    }
}
