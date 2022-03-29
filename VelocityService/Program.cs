using VelocityService;
using VelocityService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        string connstr = hostContext.Configuration.GetValue<string>("ConnStr");
        services.AddHostedService(sp => new VelocityWorker(connstr));
    })
    .Build();

await host.RunAsync();