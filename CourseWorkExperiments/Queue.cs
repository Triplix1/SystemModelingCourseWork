namespace CourseWork;

public class Queue
{
    protected List<Truck> Trcuks { get; set; } = new ();
    public int Count => Trcuks.Count;

    public virtual bool PutInQueue(Truck truck)
    {
        Trcuks.Add(truck);

        return true;
    }

    public virtual Truck TakeFromQueue()
    {
        if (Trcuks.Count <= 0) 
            throw new Exception("queue is empty");
        
        var truck = Trcuks.First();

        Trcuks.Remove(truck);

        return truck;
    }
}