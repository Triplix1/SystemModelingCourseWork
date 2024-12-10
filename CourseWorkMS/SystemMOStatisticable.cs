namespace CourseWorkMS;

public class SystemMOStatisticable: SystemMO, IStatisticable
{
    private double _lastBusyTime;
    
    public SystemMOStatisticable(string nameOfElement, IEnumerable<Process> processes,
        IList<NextElementTransition> nextElementTransitions) : base(nameOfElement, processes, nextElementTransitions)
    {
    }

    public SystemMOStatisticable(string nameOfElement, IEnumerable<Process> processes, Queue queue,
        IList<NextElementTransition> nextElementTransitions) : base(nameOfElement, processes, queue,
        nextElementTransitions)
    {
    }

    public override void InAct(Truck truck)
    {
        if (!IsBusy)
        {
            _lastBusyTime = TimeCurrent;
        }

        base.InAct(truck);
    }

    public override Truck OutAct()
    {
        TotalBusyTime += TimeCurrent - _lastBusyTime;
        _lastBusyTime = TimeCurrent;

        var result = base.OutAct();

        return result;
    }
    
    public void PrintResult(double time)
    {
        Console.WriteLine(Name + $" avg trucks count: {GetStatistic():F2}");
        Console.WriteLine(Name + $" busy time: {TotalBusyTime / time:F2}");
    }
    
    // public void PrintResult(double time)
    // {
    //     Console.WriteLine(Name + $" average trucks count: {GetStatistic():F2}");
    //     Console.WriteLine(Name + $" busy time: {TotalBusyTime / time:F2}");
    // }


    
    public void DoStatistic(double delta)
    {
        var trucksCount = Processes.Count(p => p.IsBusy);

        if(Queue is not null)
            trucksCount += Queue.Count;
        
        MeanTrucksCount += trucksCount * delta;
    }

    public double GetStatistic()
    {
        return MeanTrucksCount;
    }

}