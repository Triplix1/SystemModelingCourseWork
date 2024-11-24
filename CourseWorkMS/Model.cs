namespace CourseWorkMS;

public class Model
{
    double _timeNext, _timeCurr;
    private Element _currentElement;
    private List<SystemMO> _systems;
    private List<Truck> _clients;
    private List<SystemMOStatisticable> _statisticable; 
    
    public Model(List<SystemMO> elements)
    {
        _systems = elements;
        _statisticable = new List<SystemMOStatisticable>(_systems.OfType<SystemMOStatisticable>());
        _timeNext = 0d;
        _timeCurr = _timeNext;
    }

    public void Simulate(double time)
    {
        while (_timeCurr < time)
        {
            _timeNext = double.MaxValue;

            foreach (var element in _systems)
            {
                if (element.TimeNext < _timeNext)
                {
                    _timeNext = element.TimeNext;
                    _currentElement = element;
                }
            }

            Console.WriteLine("\nIt's time for event in " + _currentElement.Name + ", time = " + _timeNext);

            foreach (var statisticable in _statisticable)
            {
                statisticable.DoStatistic((_timeNext - _timeCurr) / time);
            }
            
            _timeCurr = _timeNext;

            foreach (var e in _systems)
            {
                e.TimeCurrent = _timeCurr;
            }

            _currentElement.OutAct();
            
            PrintInfo();
        }
        
        PrintResult(time);
    }

    private void PrintInfo()
    {
        foreach (var e in _systems)
        {
            e.PrintInfo();
        }
    }

    private void PrintResult(double time)
    {
        var avgTrucksCount = 0d;
        var avgBusyTime = 0d;
        foreach (var system in _statisticable.Take(3))
        {
            Console.WriteLine();
            system.PrintResult(time);
            avgTrucksCount += system.GetStatistic();
            avgBusyTime += system.TotalBusyTime / time;
        }

        Console.WriteLine();
        Console.WriteLine($"Excavators avg trucks count: {avgTrucksCount / 3:F2}");
        Console.WriteLine($"Excavators avg busy time: {avgBusyTime / 3:F2}");
        
        Console.WriteLine();
        _statisticable.Last().PrintResult(time);
    }
    
    // private void PrintResult(double time)
    // {
    //     foreach (var system in _statisticable)
    //     {
    //         Console.WriteLine();
    //         system.PrintResult(time);
    //     }
    // }
}