using System.Data;
using System.Data.SqlClient;
using NLog;
using VelocityService.Models;

namespace VelocityService.Services;

public class VelocityWorker : IHostedService, IDisposable
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly string ConnStr;
    private Timer _timer = null!;
    private int _executionCount = 0;
    private readonly int _interval = 0;
    
    public VelocityWorker(string connstr, int interval)
    {
        ConnStr = connstr;
        _interval = interval;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Trace("Timed Hosted Service running.");
        _timer = new Timer(BodyOfWork, null, TimeSpan.Zero, 
            TimeSpan.FromSeconds(_interval));

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
        DataTable dt = DataTableUtil.GetDataTable();
        
        Random rand = new();
        List<int> recordsToInsert = Enumerable.Range(1, rand.Next(1, 1000)).ToList();
        _logger.Trace($"Records: {recordsToInsert.Count}");

        Velocity velocity = new();
        velocity.SetAcceleration(); /* acceleration m/s^2 */
        
        recordsToInsert.ForEach(p =>
        {
            DateTime st = DateTime.Now; 
            velocity.SetTime(); /* time in seconds  */
            velocity.ComputeFinalVelocity();
            velocity.LogVelocity(_logger);
            dt.Rows.Add(dt.BuildDataRow(velocity, st));

            velocity.InitialVelocity = velocity.FinalVelocity;
            velocity.Rpms = rand.Next(1, 100);
            velocity.SetAcceleration();
        });
        
        using (SqlBulkCopy sqlBulk = new (ConnStr))
        {
            sqlBulk.DestinationTableName = "VelocityReadings";
            sqlBulk.ColumnMappings.Add(nameof(velocity.Guid), "Guid");
            sqlBulk.ColumnMappings.Add(nameof(velocity.InitialVelocity), "InitialVelocity");
            sqlBulk.ColumnMappings.Add(nameof(velocity.Acceleration), "Acceleration");
            sqlBulk.ColumnMappings.Add(nameof(velocity.Time), "Time");
            sqlBulk.ColumnMappings.Add(nameof(velocity.Rpms), "Rpms");
            sqlBulk.ColumnMappings.Add("RunDurationSeconds", "RunDurationSeconds");
            sqlBulk.ColumnMappings.Add("EntryDateTime", "EntryDateTime");
            
            sqlBulk.WriteToServer(dt);
        }
        _logger.Trace($"Timed Hosted Service is working. Count: {count}");
    }
    
    public void Dispose()
    {
        _timer.Dispose();
    }
}