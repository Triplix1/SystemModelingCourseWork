namespace CourseWork;

public class Process : Element
{
    public int TotalQuantity { get; private set; }
    private Dictionary<TruckType, double> Delays { get; set; }
    public Truck Client { get; set; }
    
    public Process(string nameOfElement, Dictionary<TruckType, double> delays) : base(
        nameOfElement)
    {
        Delays = delays;
    }

    public override void InAct(Truck truck)
    {
        TotalQuantity++;

        Client = truck;

        var delayMean = Delays[truck.Type];
        
        IsBusy = true;
        TimeNext = TimeCurrent + FunRand.Exp(delayMean);
    }

    public override Truck OutAct()
    {
        Quantity++;

        TimeNext = double.MaxValue;
        IsBusy = false;

        return Client;
    }
}