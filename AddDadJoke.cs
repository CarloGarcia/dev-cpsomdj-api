using System.Net;
using Microsoft.Extensions.Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Company.Function
{
    public class AddDadJoke
    {
        private readonly ILogger _logger;

        public AddDadJoke(ILogger<AddDadJoke> logger)
        {
            _logger = logger;
        }

        [Function("AddDadJoke")]
        public TableEntity Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] [FromBody] DadJoke dadJoke,
            FunctionContext context)
        {
            _logger.LogInformation("AddDadJoke function triggered.");

            var connectionString = Environment.GetEnvironmentVariable("MyStorageConnection");
            var tableName = "devdadjokes";
            var tableClient = new TableClient(connectionString, tableName);

            var entity = new TableEntity("dadjokes", dadJoke.Id)
            {
                { "Joke", dadJoke.Joke },
                { "DateSubmitted", DateTime.UtcNow },
                { "NoOfVotes", 0 },
                { "SubmittedBy", dadJoke.SubmittedBy },
                { "UserId", dadJoke.UserId },
                { "Email", dadJoke.Email }
            };
            
            tableClient.AddEntity(entity);

            return entity;
        }
    }
}
