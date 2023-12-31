using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using System.Text.Json;
using Azure;
using Company.Function.Models;

namespace Company.Function
{
    public class GetDadJokes
    {
        private readonly ILogger _logger;

        public GetDadJokes(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetDadJokes>();
        }

        [Function("GetDadJokes")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("GetDadJokes funcion triggered.");

            var connectionString = Environment.GetEnvironmentVariable("TableStorageConnectionString");
            var tableName = Environment.GetEnvironmentVariable("DadJokesTableName");
            var tableClient = new TableClient(connectionString, tableName);

            Pageable<TableEntity> entities = tableClient.Query<TableEntity>();

            List<DadJoke> dadJokes = new List<DadJoke>();
            foreach (TableEntity entity in entities)
            {
                dadJokes.Add(new DadJoke
                {
                    Id = entity.RowKey,
                    Joke = entity.GetString("Joke"),
                    DateSubmitted = (DateTime)(entity.GetDateTime("DateSubmitted")),
                    NoOfVotes = (int)entity["NoOfVotes"],
                    SubmittedBy = entity.GetString("SubmittedBy"),
                    UserId = entity.GetString("UserId"),
                    Email = entity.GetString("Email")
                });
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(JsonSerializer.Serialize(dadJokes));

            return response;
        }
    }
}