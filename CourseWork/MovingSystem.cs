namespace CourseWork;

public abstract class MovingSystem: Element
{
    protected Dictionary<TruckType, double> Delays { get; }
    protected readonly Dictionary<Truck, double> NextTimes = new ();
    public override double TimeNext => NextTimes.Any() ? NextTimes.Values.Min() : Double.MaxValue;

    public MovingSystem(string nameOfElement, Dictionary<TruckType, double> delays) : base(nameOfElement)
    {
        Delays = delays;
    }
    
    public MovingSystem(string nameOfElement, IList<Element> nextElements, Dictionary<TruckType, double> delays) : base(nameOfElement, nextElements)
    {
        Delays = delays;
    }
    
    public override void InAct(Truck truck)
    {
        var nextTime = TimeCurrent + Delays[truck.Type];
        
        NextTimes.Add(truck, nextTime);
    }

    public override Truck OutAct()
    {
        Quantity++;
        var truckToRemove = NextTimes.Single(t => Math.Abs(t.Value - TimeCurrent) < 0.0001).Key;

        NextTimes.Remove(truckToRemove);

        return truckToRemove;
    }
}