namespace CourseWorkMS;

public class Process : Element
{
    private readonly Distribution _distribution;
    private readonly Dictionary<TruckType, double> _delays;
    public Truck Client { get; set; }
    
    public Process(string nameOfElement, Dictionary<TruckType, double> delays, Distribution distribution) : base(
        nameOfElement)
    {
        _distribution = distribution;
        _delays = delays;
    }

    public override void InAct(Truck truck)
    {
        Client = truck;

        IsBusy = true;
        TimeNext = TimeCurrent + FunRand.GetDelay(truck.Type, _delays, _distribution);
    }

    public override Truck OutAct()
    {
        Quantity++;

        TimeNext = double.MaxValue;
        IsBusy = false;

        return Client;
    }
}