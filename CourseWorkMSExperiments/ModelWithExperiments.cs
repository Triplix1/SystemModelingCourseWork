using CourseWorkMSExperiments;

namespace CourseWorkMS;

public class ModelWithExperiments
{
    double _timeNext, _timeCurr;
    private Element _currentElement;
    private List<SystemMO> _systems;
    private List<Truck> _clients;
    private List<SystemMOStatisticable> _statisticable; 
    Dictionary<double, double> queueValues = new();
    private double _time = 0d;
    private double _timeStep = 100d;

    
    public ModelWithExperiments(List<SystemMO> elements)
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
            
            if (_timeCurr > _time + _timeStep)
            {
                queueValues.Add(_timeCurr, _systems.Sum(s => s.MeanTrucksCount * time / _timeCurr));
                _time = _timeCurr;
            }

            _currentElement.OutAct();
            
            PrintInfo();
        }

        PrintResult(time);

        Serializer.SerializeDictionaryToFile(queueValues, "First1.json");
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
        foreach (var system in _statisticable)
        {
            Console.WriteLine();
            system.PrintResult(time);
        }
    }
}