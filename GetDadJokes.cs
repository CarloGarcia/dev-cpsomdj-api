using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            // Pageable<TableEntity> entities = tableClient.Query<TableEntity>(filter: "PartitionKey eq 'markers'");
            // // Or using a filter expression
            // Pageable<TableEntity> entities = tableClient.Query<TableEntity>(ent => ent.PartitionKey == "markers");

            // foreach (TableEntity entity in entities)
            // {
            //     Console.WriteLine($"{entity.GetString("Product")}: {entity.GetDouble("Price")}");
            // }

            return response;
        }
    }
}
