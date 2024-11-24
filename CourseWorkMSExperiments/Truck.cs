namespace CourseWorkMS;

public class Truck
{
    public TruckType Type { get; set; }

    public Truck(TruckType type)
    {
        Type = type;
    }
}