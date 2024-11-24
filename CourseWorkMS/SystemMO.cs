namespace CourseWorkMS;

public class SystemMO : Element
{
    public override bool IsBusy => Processes.All(p => p.IsBusy);
    public override double TimeNext => Processes.Min(p => p.TimeNext);
    public Queue Queue { get; set; }
    public IEnumerable<Process> Processes { get; set; }
    public double MeanTrucksCount { get; set; }
    public double TotalBusyTime { get; set; }
    public int Failures { get; set; }
    public IList<NextElementTransition> NextElementTransitions;

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
    
    public SystemMO(string nameOfElement, IEnumerable<Process> processes, IList<NextElementTransition> nextElementTransitions) : base(nameOfElement)
    {
        NextElementTransitions = nextElementTransitions;
        Processes = processes;
    }

    public SystemMO(string nameOfElement, IEnumerable<Process> processes, Queue queue, IList<NextElementTransition> nextElementTransitions) : this(nameOfElement, processes, nextElementTransitions)
    {
        Queue = queue;
    }

    public override void InAct(Truck truck)
    {
        if (!IsBusy)
        {
            var freeProcess = Processes.First(p => !p.IsBusy);

            freeProcess.InAct(truck);

            TimeNext = Processes.Min(p => p.TimeNext);
        }
        else
        {
            if (Queue is not null)
            {
                var successfullyPutted = Queue.PutInQueue(truck);
                
                if (!successfullyPutted)
                    Failures++;
            }
            else
            {
                Failures++;
            }
        }
    }

    public override Truck OutAct()
    {
        Quantity++;

        TimeNext = double.MaxValue;

        var currentProcess = Processes.Single(p => p.TimeNext == TimeCurrent);
        var result = currentProcess.OutAct();

        if (Queue is not null && Queue.Count > 0)
        {
            var truck = Queue.TakeFromQueue();
            InAct(truck);
        }

        InNextElement(result);
        return result;
    }

    public virtual void InNextElement(Truck truck)
    {
        var random = new Random();
        
        if(!NextElementTransitions.Any())
            return;
        
        var nonBlockedElement = NextElementTransitions.Where(t => !t.BlockingPredicate(truck, t.System)).ToArray();
        if (nonBlockedElement.Any(c => c.System.Name == "Excavator 2 system"))
        {
            Console.WriteLine();
        }
        var next = nonBlockedElement[random.Next(nonBlockedElement.Count())];
        
        next.System.InAct(truck);

    }

    public override void PrintInfo()
    {
        var stringToShow = Name + " State = " + IsBusy +
                           " Quantity = " + Quantity +
                           " TNext = " + TimeNext;
        if(Queue is not null)
            stringToShow += " Queue = " + Queue.Count;
        
        Console.WriteLine(stringToShow);
    }
}