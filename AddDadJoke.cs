using System.Net;
using Microsoft.Extensions.Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;

namespace Company.Function
{
    public class AddDadJoke
    {
        private readonly ILogger _logger;
        // private readonly TableClient _tableClient;

        public AddDadJoke(ILogger<AddDadJoke> logger)//, IAzureClientFactory<TableClient> tableClientFactory)
        {
            _logger = logger;
            // _tableClient = tableClientFactory.CreateClient("dadjokes").GetTableClient("devdadjokes");
            // _tableClient.CreateIfNotExists();
            // _tableClient = tableClientFactory.CreateClient("dadjokes");
            // _tableClient = tableClientFactory.CreateClient(new TableClientOptions
            // {
            //     ConnectionString = "DefaultEndpointsProtocol=https;AccountName=movdadjokes;AccountKey=7DcPaitu3NkYFt5JkOe1SJOIF7HKVhOk/+PVpmgubTnbPr4T+lrGa6psQWTWKGoQ1WKf5gL0TOIH+ASt81u72w==;EndpointSuffix=core.windows.net",
            //     TableName = "devdadjokes"
            //  });
        }

        [Function("AddDadJoke")]
        public TableEntity Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] dynamic input,
            FunctionContext context)
        {
            _logger.LogInformation("AddDadJoke function triggered.");

            var connectionString = Environment.GetEnvironmentVariable("MyStorageConnection");
            var tableName = "devdadjokes";
            var tableClient = new TableClient(connectionString, tableName);

            //var tableClient = _tableClientFactory.CreateClient(new TableClientOptions
            //{
            //    ConnectionString = connectionString,
            //    TableName = tableName
            //});

            TableEntity entity = new TableEntity("myPartition", Guid.NewGuid().ToString())
            {
                { "Product", "Marker Set" },
                { "Price", 5.00 },
                { "Quantity", 21 }
            };
            tableClient.AddEntity(entity);

            // var entity = new MyTableEntity
            // {
            //     PartitionKey = "myPartition",
            //     RowKey = Guid.NewGuid().ToString(),
            //     MyProperty = "myValue2"
            // };
            // tableClient.AddEntity(entity);
            return entity;
        }
    }
}
