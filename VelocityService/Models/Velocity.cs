using NLog;

namespace VelocityService.Models;

public class Velocity
{
    public string Guid { get; set; }
    public double InitialVelocity { get; set; }
    public double Acceleration { get; set; } = 0;
    public double Time { get; set; }
    public double FinalVelocity { get; set; }
    public int Rpms { get; set; }

    public Velocity()
    {
        Guid = System.Guid.NewGuid().ToString();
        Acceleration = 0;
        Rpms = new Random().Next(0, 100);
    }

    public void SetAcceleration()
    {
        Acceleration = GetValue(Rpms, 1, 100, .001, .5);
    }
    
    public void SetTime()
    {
        Time = GetValue(Acceleration, .001, Acceleration + .5, .01, .3);
    }
    
    public void ComputeFinalVelocity()
    {
        FinalVelocity = InitialVelocity + Acceleration * Time;
    }

    public void LogVelocity(Logger logger)
    {
        logger.Trace($"Rpms: {Rpms}, Acceleration: {Acceleration}, Time: {Time}, Velocity: {FinalVelocity}");
    }
    
    private static double GetValue(double rpms, double inMin, double inMax, double outMin, double outMax)
    {
        return (rpms - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}