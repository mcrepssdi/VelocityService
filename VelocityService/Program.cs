using VelocityService;
using VelocityService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        string connstr = hostContext.Configuration.GetValue<string>("ConnStr");
        int interval = hostContext.Configuration.GetValue<int>("TimeIntervalSeconds");
        services.AddHostedService(sp => new VelocityWorker(connstr, interval));
    })
    .Build();

await host.RunAsync();