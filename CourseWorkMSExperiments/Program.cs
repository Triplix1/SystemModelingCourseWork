namespace CourseWorkMS;

class Program
{
    static void Main(string[] args)
    {
        #region Params

        var twentyTonsCount = 1;
        var fiftyTonsCount = 1;
        var excavatorsCount = 3;
        var totalCount = excavatorsCount * (twentyTonsCount + fiftyTonsCount);
        
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
        
        var excavatorsDistribution = Distribution.Exponential;
        var crushingDistribution = Distribution.Exponential;
        var movingToCrushDistrubution = Distribution.None;
        var movingToExcavatorsDistrubution = Distribution.None;
        
        var excavatorsParams = new DelayParams(excavatorDelays, excavatorsDistribution);
        var crushingParams = new DelayParams(crushingDelays, crushingDistribution);
        var moveToExcavatorParams = new DelayParams(moveToExDelays, movingToExcavatorsDistrubution);
        var moveToCrushParams = new DelayParams(moveToCrushDelays, movingToCrushDistrubution);

        #endregion

        #region Move to excavators

        var moveToExcavatorProcesses = new List<Process>();
        for (int i = 0; i < totalCount; i++)
        {
            var process = new Process("Move to Excavator process " + (i + 1), moveToExcavatorParams.Delays, moveToExcavatorParams.Distribution);
            moveToExcavatorProcesses.Add(process);
        }

        var moveToExcavatorsTransition = new List<NextElementTransition>();
        var movingToExcavatorSystem = new SystemMO("Moving to excavator system", moveToExcavatorProcesses,
            moveToExcavatorsTransition);

        #endregion
        
        #region Crushing

        var crushingProcess = new Process("Crushing process", crushingParams.Delays, crushingParams.Distribution);
        var crushingQueue = new PriorityQueue();

        var moveToMovingToExcavatorsTransition = new NextElementTransition()
        {
            System = movingToExcavatorSystem,
            BlockingPredicate = (t, s) => false
        };
        var crushingSystem =
            new SystemMOStatisticable("Crushing system", [crushingProcess], crushingQueue, [moveToMovingToExcavatorsTransition]);

        #endregion

        #region Move to crush

        var moveToCrushProcesses = new List<Process>();
        for (int i = 0; i < totalCount; i++)
        {
            var process = new Process("Move to Crush process " + (i + 1), moveToCrushParams.Delays, moveToCrushParams.Distribution);
            moveToCrushProcesses.Add(process);
        }

        var moveToCrushingTransition = new NextElementTransition()
        {
            System = crushingSystem,
            BlockingPredicate = (t, s) => false
        };

        var movingToCrushSystem =
            new SystemMO("Moving to crushing system", moveToCrushProcesses, [moveToCrushingTransition]);

        #endregion
            
        #region Excavators

        var firstExcavatorProcess = new Process("Excavator 1 process", excavatorsParams.Delays, excavatorsParams.Distribution);
        var firstExcavatorQueue = new Queue();
        var moveToMovingToCrushTransition = new NextElementTransition()
        {
            System = movingToCrushSystem,
            BlockingPredicate = (t, s) => false
        };
        var firstExcavator = new SystemMOStatisticable("Excavator 1 system", [firstExcavatorProcess], firstExcavatorQueue,
            [moveToMovingToCrushTransition]);

        var secondExcavatorProcess = new Process("Excavator 2 process", excavatorsParams.Delays, excavatorsParams.Distribution);
        var secondExcavatorQueue = new Queue();
        var secondExcavator = new SystemMOStatisticable("Excavator 2 system", [secondExcavatorProcess], secondExcavatorQueue,
            [moveToMovingToCrushTransition]);

        var thirdExcavatorProcess = new Process("Excavator 3 process", excavatorsParams.Delays, excavatorsParams.Distribution);
        var thirdExcavatorQueue = new Queue();
        var thirdExcavator = new SystemMOStatisticable("Excavator 3 system", [thirdExcavatorProcess], thirdExcavatorQueue,
            [moveToMovingToCrushTransition]);

        var firstExcavatorTrucks = new List<Truck>();
        for (int i = 0; i < twentyTonsCount; i++)
        {
            firstExcavatorTrucks.Add(new Truck(TruckType.TwentyTons));

        }

        for (int i = 0; i < fiftyTonsCount; i++)
        {
            firstExcavatorTrucks.Add(new Truck(TruckType.FiftyTons));
        }

        var secondExcavatorTrucks = new List<Truck>();
        for (int i = 0; i < twentyTonsCount; i++)
        {
            secondExcavatorTrucks.Add(new Truck(TruckType.TwentyTons));

        }

        for (int i = 0; i < fiftyTonsCount; i++)
        {
            secondExcavatorTrucks.Add(new Truck(TruckType.FiftyTons));
        }

        var thirdExcavatorTrucks = new List<Truck>();
        for (int i = 0; i < twentyTonsCount; i++)
        {
            thirdExcavatorTrucks.Add(new Truck(TruckType.TwentyTons));

        }

        for (int i = 0; i < fiftyTonsCount; i++)
        {
            thirdExcavatorTrucks.Add(new Truck(TruckType.FiftyTons));
        }

        #endregion

        #region Blocking

        var excavatorTrucks = new Dictionary<SystemMO, List<Truck>>();
        excavatorTrucks.Add(firstExcavator, firstExcavatorTrucks);
        excavatorTrucks.Add(secondExcavator, secondExcavatorTrucks);
        excavatorTrucks.Add(thirdExcavator, thirdExcavatorTrucks);

        foreach (var excavator in excavatorTrucks)
        {
            var moveToExcavatorTransition = new NextElementTransition()
            {
                System = excavator.Key,
                BlockingPredicate = (t, s) => !excavator.Value.Contains(t),
            };
            moveToExcavatorsTransition.Add(moveToExcavatorTransition);
        }

        #endregion

        #region In acts

        foreach (var excavatorWithTrucks in excavatorTrucks)
        {
            foreach (var truck in excavatorWithTrucks.Value)
            {
                excavatorWithTrucks.Key.InAct(truck);
            }
        }

        #endregion

        #region Simulation

        var systems = new List<SystemMO>()
        {
            firstExcavator, secondExcavator, thirdExcavator, movingToCrushSystem, crushingSystem,
            movingToExcavatorSystem
        };

        var model = new ModelWithExperiments(systems);
        model.Simulate(1000);

        #endregion
    }
}