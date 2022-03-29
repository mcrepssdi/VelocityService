using NLog;

namespace VelocityService.Services;

public class VelocityWorker : IHostedService, IDisposable
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private string ConnStr;
    private Timer _timer = null!;
    private int _executionCount = 0;
    
    public VelocityWorker(string connstr)
    {
        ConnStr = connstr;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Trace("Timed Hosted Service running.");
        _timer = new Timer(BodyOfWork, null, TimeSpan.Zero, 
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Trace("Timed Hosted Service is stopping.");
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void BodyOfWork(object? state)
    {
        int count = Interlocked.Increment(ref _executionCount);
        _logger.Trace($"Timed Hosted Service is working. Count: {count}");
    } 

    public void Dispose()
    {
        _timer.Dispose();
    }
}