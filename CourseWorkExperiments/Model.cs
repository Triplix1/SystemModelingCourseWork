using System.Security.Cryptography.X509Certificates;

namespace CourseWork;

public class Model
{
    double _timeNext, _timeCurr;
    private Element _currentElement;
    private List<Element> _elements;
    private List<Truck> _clients;
    private List<SystemMO> _systems; 
    Dictionary<double, double> queueValues = new();
    private double _time = 0d;
    private double _timeStep = 100d;
    public Model(List<Element> elements)
    {
        _elements = elements;
        _systems = new List<SystemMO>(_elements.OfType<SystemMO>());
        _timeNext = 0d;
        _timeCurr = _timeNext;
    }

    public void Simulate(double time)
    {
        
        while (_timeCurr < time)
        {
            _timeNext = double.MaxValue;

            foreach (var element in _elements)
            {
                if (element.TimeNext < _timeNext)
                {
                    _timeNext = element.TimeNext;
                    _currentElement = element;
                }
            }

            Console.WriteLine("\nIt's time for event in " + _currentElement.Name + ", time = " + _timeNext);

            foreach (var statisticable in _systems)
            {
                statisticable.DoStatistic((_timeNext - _timeCurr) / time);
            }
            
            _timeCurr = _timeNext;

            foreach (var e in _elements)
            {
                e.TimeCurrent = _timeCurr;
            }

            _currentElement.OutAct();
            
            if (_timeCurr > _time + _timeStep)
            {
                queueValues.Add(_timeCurr, _systems.Sum(s => s.MeanTrucksCount * time / _timeCurr));
                _time = _timeCurr;
            }
            
            PrintInfo();
        }
        
        PrintResult(time);

        Serializer.SerializeDictionaryToFile(queueValues, "First21.json");
    }

    private void PrintInfo()
    {
        foreach (var e in _elements)
        {
            e.PrintInfo();
        }
    }

    private void PrintResult(double time)
    {
        foreach (var system in _systems)
        {
            Console.WriteLine();
            system.PrintResult(time);
        }
    }

    private double GetStatistic(SystemMO system, double delta)
    {
        var processCars = system.Processes.Count(p => p.IsBusy);

        var trucksCount = system.Queue.Count + processCars;
        
        return trucksCount * delta;
    }
}