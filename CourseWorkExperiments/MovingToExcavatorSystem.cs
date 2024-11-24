namespace CourseWork;

public class MovingToExcavatorSystem: MovingSystem
{
    public MovingToExcavatorSystem(string nameOfElement, Dictionary<TruckType, double> delays) : base(nameOfElement, delays)
    {
    }

    public override Truck OutAct()
    {
        var truck = base.OutAct();
        truck.Excavator.InAct(truck);

        return truck;
    }
}