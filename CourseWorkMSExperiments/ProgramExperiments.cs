using CourseWorkMSExperiments;

namespace CourseWorkMS;

class ProgramExperiments
{
    static void Main(string[] args)
    {

        var paramsArr = new Params[]
        {
            new Params() { X1 = 5, X2 = 5, X3 = 6 },
            new Params() { X1 = 3, X2 = 5, X3 = 6 },
            new Params() { X1 = 5, X2 = 4, X3 = 6 },
            new Params() { X1 = 3, X2 = 4, X3 = 6 },
            new Params() { X1 = 5, X2 = 5, X3 = 3 },
            new Params() { X1 = 3, X2 = 5, X3 = 3 },
            new Params() { X1 = 5, X2 = 4, X3 = 3 },
            new Params() { X1 = 3, X2 = 4, X3 = 3 },
        };

        for (int p = 0; p < paramsArr.Length; p++)
        {
            #region Params

            var testsCount = 20;

            var twentyTonsCount = 1;
            var fiftyTonsCount = 1;
            var excavatorsCount = 3;
            var totalCount = excavatorsCount * (twentyTonsCount + fiftyTonsCount);

            var excavatorDelays = new Dictionary<TruckType, double>();
            excavatorDelays.Add(TruckType.TwentyTons, paramsArr[p].X3);
            excavatorDelays.Add(TruckType.FiftyTons, 10d);

            var crushingDelays = new Dictionary<TruckType, double>();
            crushingDelays.Add(TruckType.TwentyTons, paramsArr[p].X1);
            crushingDelays.Add(TruckType.FiftyTons, paramsArr[p].X2);

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

            var results = new List<double>();

            for (int j = 0; j < testsCount; j++)
            {
                #region Move to excavators

                var moveToExcavatorProcesses = new List<Process>();
                for (int i = 0; i < totalCount; i++)
                {
                    var process = new Process("Move to Excavator process " + (i + 1), moveToExcavatorParams.Delays,
                        moveToExcavatorParams.Distribution);
                    moveToExcavatorProcesses.Add(process);
                }

                var moveToExcavatorsTransition = new List<NextElementTransition>();
                var movingToExcavatorSystem = new SystemMO("Moving to excavator system", moveToExcavatorProcesses,
                    moveToExcavatorsTransition);

                #endregion

                #region Crushing

                var crushingProcess =
                    new Process("Crushing process", crushingParams.Delays, crushingParams.Distribution);
                var crushingQueue = new PriorityQueue();

                var moveToMovingToExcavatorsTransition = new NextElementTransition()
                {
                    System = movingToExcavatorSystem,
                    BlockingPredicate = (t, s) => false
                };
                var crushingSystem =
                    new SystemMOStatisticable("Crushing system", [crushingProcess], crushingQueue,
                        [moveToMovingToExcavatorsTransition]);

                #endregion

                #region Move to crush

                var moveToCrushProcesses = new List<Process>();
                for (int i = 0; i < totalCount; i++)
                {
                    var process = new Process("Move to Crush process " + (i + 1), moveToCrushParams.Delays,
                        moveToCrushParams.Distribution);
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

                var moveToMovingToCrushTransition = new NextElementTransition()
                {
                    System = movingToCrushSystem,
                    BlockingPredicate = (t, s) => false
                };

                var excavatorTrucks = new Dictionary<SystemMO, List<Truck>>();
                var excavators = new List<SystemMO>();

                for (int i = 0; i < excavatorsCount; i++)
                {
                    var excavatorProcess = new Process($"Excavator {i + 1} process", excavatorsParams.Delays,
                        excavatorsParams.Distribution);
                    var excavatorsQueue = new Queue();
                    var excavator = new SystemMOStatisticable($"Excavator {i + 1} system", [excavatorProcess],
                        excavatorsQueue,
                        [moveToMovingToCrushTransition]);

                    excavators.Add(excavator);

                    var currentExcavatorTrucks = new List<Truck>();
                    for (int k = 0; k < twentyTonsCount; k++)
                    {
                        currentExcavatorTrucks.Add(new Truck(TruckType.TwentyTons));
                    }

                    for (int k = 0; k < twentyTonsCount; k++)
                    {
                        currentExcavatorTrucks.Add(new Truck(TruckType.FiftyTons));
                    }

                    excavatorTrucks.Add(excavator, currentExcavatorTrucks);
                }

                #endregion

                #region Blocking

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

                var systems = new List<SystemMO>();
                systems.AddRange(excavators);
                systems.Add(movingToCrushSystem);
                systems.Add(crushingSystem);
                systems.Add(movingToExcavatorSystem);

                var model = new ModelCollectExperimentsData(systems, paramsArr[p].X3);
                var result = model.Simulate(12000);
                results.Add(result);

                #endregion
            }

            Console.WriteLine("Resulted avg: " + results.Average());

            Serializer.SerializeDictionaryToFile(results, $"Experiments{p+1}.json");
        }
    }
}