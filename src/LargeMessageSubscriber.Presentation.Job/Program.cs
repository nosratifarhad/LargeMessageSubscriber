using LargeMessageSubscriber.Integration.Options;
using LargeMessageSubscriber.Presentation.Job.BackgroundWorkers;
using LargeMessageSubscriber.Presentation.Job.Extensions;
using LargeMessageSubscriber.Shared.Services;
using LargeMessageSubscriber.Shared.Services.Contracts;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddOptions();
        services.Configure<BrokerOptions>(hostContext.Configuration.GetSection("BrokerOptions"));
        services.AddJobServices();

        services.AddSingleton<IDataGeneratorService, DataGeneratorService>();

        services.AddHostedService<BackgroundWorkerService>();
    })
    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
    });

var host = builder.Build();

await host.RunAsync();
