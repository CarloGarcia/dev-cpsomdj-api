using Microsoft.Extensions.Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
    //.ConfigureServices((hostContext, services) => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        // services.AddAzureClients(clientBuilder =>
        // {
        //     clientBuilder.AddTableServiceClient(hostContext.Configuration.GetSection("MyStorageConnection"))
        //         .WithName("dadjokes");
        // });
    })

    .Build();

host.Run();
