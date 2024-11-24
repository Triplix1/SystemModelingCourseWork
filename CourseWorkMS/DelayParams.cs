namespace CourseWorkMS;

public class DelayParams
{
    public Dictionary<TruckType, double> Delays { get; set; }
    public Distribution Distribution { get; set; }
    
    public DelayParams(Dictionary<TruckType, double> delays, Distribution distribution)
    {
        Delays = delays;
        Distribution = distribution;
    }
}