namespace CourseWork;

public class MovingToCrushingSystem: MovingSystem
{
    public MovingToCrushingSystem(string nameOfElement, IList<Element> nextElements, Dictionary<TruckType, double> delays) : base(nameOfElement, nextElements, delays)
    {
        
    }
    
    public override Truck OutAct()
    {
        var truck = base.OutAct();
        NextElements.Single().InAct(truck);

        return truck;
    }
}