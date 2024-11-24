namespace CourseWork;

public class SystemMO : Element, IStatisticable
{
    public override bool IsBusy => Processes.All(p => p.IsBusy);
    public override double TimeNext => Processes.Min(p => p.TimeNext);
    public Queue Queue { get; set; }
    public IEnumerable<Process> Processes { get; set; }
    public double MeanTrucksCount { get; set; }
    public double TotalBusyTime { get; set; }
    private double _lastBusyTime;

    public override double TimeCurrent
    {
        get => _timeCurr;
        set
        {
            _timeCurr = value;
            foreach (var process in Processes)
            {
                process.TimeCurrent = value;
            }
        }
    }
    private double _timeCurr;

    public SystemMO(string nameOfElement, IEnumerable<Process> processes, Queue queue, IList<Element> nextElements) : base(nameOfElement, nextElements)
    {
        Queue = queue;
        Processes = processes;
    }

    public override void InAct(Truck truck)
    {
        if (!IsBusy)
        {
            _lastBusyTime = TimeCurrent;
            var freeProcess = Processes.First(p => !p.IsBusy);

            freeProcess.InAct(truck);

            TimeNext = Processes.Min(p => p.TimeNext);
        }
        else
        {
            Queue.PutInQueue(truck);
        }
    }

    public override Truck OutAct()
    {
        Quantity++;

        TimeNext = double.MaxValue;

        var currentProcess = Processes.Single(p => Math.Abs(p.TimeNext - TimeCurrent) < 0.00001);
        var result = currentProcess.OutAct();
        TotalBusyTime += TimeCurrent - _lastBusyTime;
        _lastBusyTime = TimeCurrent;

        if (Queue.Count > 0)
        {
            var truck = Queue.TakeFromQueue();
            InAct(truck);
        }

        InNextElement(result);
        return result;
    }

    public virtual void InNextElement(Truck client)
    {
        var random = new Random();
        
        if(!NextElements.Any())
            return;
        
        var next = NextElements[random.Next(NextElements.Count)];
        
        next.InAct(client);

    }

    public void PrintResult(double time)
    {
        Console.WriteLine(Name + $" average trucks count: {GetStatistic():F2}");
        Console.WriteLine(Name + $" busy time: {TotalBusyTime / time:F2}");
    }

    public override void PrintInfo()
    {
        Console.WriteLine(Name + " state= " + IsBusy +
                          " quantity = " + Quantity +
                          " tnext= " + TimeNext +
                          "Queue= " + Queue.Count);
    }

    public void DoStatistic(double delta)
    {
        var processCars = Processes.Count(p => p.IsBusy);

        var trucksCount = Queue.Count + processCars;
        
        MeanTrucksCount += trucksCount * delta;
    }

    public double GetStatistic()
    {
        return MeanTrucksCount;
    }
}