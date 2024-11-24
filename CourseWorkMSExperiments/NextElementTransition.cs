namespace CourseWorkMS;

public class NextElementTransition
{
    public SystemMO System { get; set; }
    public Func<Truck, SystemMO, bool> BlockingPredicate { get; set; }
}