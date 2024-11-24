namespace CourseWork;

public class Truck
{
    public TruckType Type { get; set; }
    public SystemMO Excavator { get; set; }

    public Truck(TruckType type, SystemMO excavator)
    {
        Type = type;
        Excavator = excavator;
    }
}