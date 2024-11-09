namespace CourseWork;

class Program
{
    static void Main(string[] args)
    {
        var excavatorDelays = new Dictionary<TruckType, double>();
        excavatorDelays.Add(TruckType.TwentyTons, 5d);
        excavatorDelays.Add(TruckType.FiftyTons, 10d);
        
        var crushingDelays = new Dictionary<TruckType, double>();
        crushingDelays.Add(TruckType.TwentyTons, 5d);
        crushingDelays.Add(TruckType.FiftyTons, 4d);
        
        var moveToExDelays = new Dictionary<TruckType, double>();
        moveToExDelays.Add(TruckType.TwentyTons, 1.5);
        moveToExDelays.Add(TruckType.FiftyTons, 2d);
        
        var moveToCrushDelays = new Dictionary<TruckType, double>();
        moveToCrushDelays.Add(TruckType.TwentyTons, 2.5);
        moveToCrushDelays.Add(TruckType.FiftyTons, 3d);

        var movingToExcavatorSystem = new MovingToExcavatorSystem("Moving to excavator system", moveToExDelays);
        
        var crushingProcess = new Process("Crushing process", crushingDelays);
        var crushingQueue = new PriorityQueue();
        
        var crushingSystem =
            new CrushingSystem("Crushing system", [crushingProcess], crushingQueue, [movingToExcavatorSystem]);

        var movingSystemFromExcavator = new MovingToCrushingSystem("Moving to crushing system", [crushingSystem], moveToCrushDelays);
        
        var firstExcavatorProcess = new Process("Excavator 1 process", excavatorDelays);
        var firstExcavatorQueue = new Queue();
        var firstExcavator = new SystemMO("Excavator 1 system", [firstExcavatorProcess], firstExcavatorQueue, [movingSystemFromExcavator]);
        
        var secondExcavatorProcess = new Process("Excavator 2 process", excavatorDelays);
        var secondExcavatorQueue = new Queue();
        var secondExcavator = new SystemMO("Excavator 2 system", [secondExcavatorProcess], secondExcavatorQueue, [movingSystemFromExcavator]);
        
        var thirdExcavatorProcess = new Process("Excavator 3 process", excavatorDelays);
        var thirdExcavatorQueue = new Queue();
        var thirdExcavator = new SystemMO("Excavator 3 system", [thirdExcavatorProcess], thirdExcavatorQueue, [movingSystemFromExcavator]);

        var firstExcavatorTwentyTruck = new Truck(TruckType.TwentyTons, firstExcavator);
        var firstExcavatorFiftyTruck = new Truck(TruckType.FiftyTons, firstExcavator);
        
        var secondExcavatorTwentyTruck = new Truck(TruckType.TwentyTons, secondExcavator);
        var secondExcavatorFiftyTruck = new Truck(TruckType.FiftyTons, secondExcavator);
        
        var thirdExcavatorTwentyTruck = new Truck(TruckType.TwentyTons, thirdExcavator);
        var thirdExcavatorFiftyTruck = new Truck(TruckType.FiftyTons, thirdExcavator);
        
        firstExcavator.InAct(firstExcavatorTwentyTruck);
        firstExcavator.InAct(firstExcavatorFiftyTruck);
        
        secondExcavator.InAct(secondExcavatorTwentyTruck);
        secondExcavator.InAct(secondExcavatorFiftyTruck);
        
        thirdExcavator.InAct(thirdExcavatorTwentyTruck);
        thirdExcavator.InAct(thirdExcavatorFiftyTruck);
        
        var systems = new List<Element>() {firstExcavator, secondExcavator, thirdExcavator, movingSystemFromExcavator, crushingSystem, movingToExcavatorSystem};

        var model = new Model(systems);
        model.Simulate(1000);
    }
}