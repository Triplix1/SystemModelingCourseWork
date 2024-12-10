using CourseWorkMS;

namespace CourseWorkMSExperiments;

public class ModelCollectExperimentsData
{
    double _timeNext, _timeCurr;
    private Element _currentElement;
    private List<SystemMO> _systems;
    private readonly int _excavatorsCount;
    private List<Truck> _clients;
    private List<SystemMOStatisticable> _statisticable;
    private const int ProbPeriod = 11000;
    
    public ModelCollectExperimentsData(List<SystemMO> elements, int excavatorsCount)
    {
        _systems = elements;
        _excavatorsCount = excavatorsCount;
        _statisticable = new List<SystemMOStatisticable>(_systems.OfType<SystemMOStatisticable>());
        _timeNext = 0d;
        _timeCurr = _timeNext;
    }

    public double Simulate(double time)
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

            if (_timeNext > ProbPeriod)
            {
                if (_timeCurr <= ProbPeriod)
                {
                    foreach (var statisticable in _statisticable)
                    {
                        statisticable.TotalBusyTime = 0d;
                    }
                }
                
                foreach (var statisticable in _statisticable)
                {
                    statisticable.DoStatistic((_timeNext - _timeCurr) / (time - ProbPeriod));
                }

            }

            _timeCurr = _timeNext;

            foreach (var e in _systems)
            {
                e.TimeCurrent = _timeCurr;
            }

            _currentElement.OutAct();
        }

        var observedTime = time - ProbPeriod;

        return _statisticable.Take(_excavatorsCount).Sum(x => x.TotalBusyTime / observedTime) / _excavatorsCount +
               _statisticable.Skip(_excavatorsCount).Sum(d => d.TotalBusyTime / observedTime) /
               (_statisticable.Count - _excavatorsCount);
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
        foreach (var system in _statisticable.Take(_excavatorsCount))
        {
            Console.WriteLine();
            system.PrintResult(time);
            avgTrucksCount += system.GetStatistic();
            avgBusyTime += system.TotalBusyTime / time;
        }

        Console.WriteLine();
        Console.WriteLine($"Excavators avg trucks count: {avgTrucksCount / _excavatorsCount:F2}");
        Console.WriteLine($"Excavators avg busy time: {avgBusyTime / _excavatorsCount:F2}");
        
        Console.WriteLine();
        _statisticable.Last().PrintResult(time);
    }

    private double GetModelingResult(double time)
    {
        var avgTrucksCount = 0d;
        var avgBusyTime = 0d;
        foreach (var system in _statisticable.Take(_excavatorsCount))
        {
            Console.WriteLine();
            system.PrintResult(time);
            avgTrucksCount += system.GetStatistic();
            avgBusyTime += system.TotalBusyTime / time;
        }

        Console.WriteLine();
        return avgBusyTime / _excavatorsCount + _statisticable.Skip(_excavatorsCount).Sum(d => d.TotalBusyTime) / time;
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