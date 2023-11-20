using System.Net;
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
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            // // Get the entity to update.
            // TableEntity qEntity = await tableClient.GetEntityAsync<TableEntity>(partitionKey, rowKey);
            // qEntity["Price"] = 7.00;

            // // Since no UpdateMode was passed, the request will default to Merge.
            // await tableClient.UpdateEntityAsync(qEntity, qEntity.ETag);

            // TableEntity updatedEntity = await tableClient.GetEntityAsync<TableEntity>(partitionKey, rowKey);
            // Console.WriteLine($"'Price' before updating: ${entity.GetDouble("Price")}");
            // Console.WriteLine($"'Price' after updating: ${updatedEntity.GetDouble("Price")}");

            return response;
        }
    }
}
