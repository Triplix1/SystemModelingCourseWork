namespace CourseWork;

public class PriorityQueue: Queue
{
    public override bool PutInQueue(Truck truck)
    {
        if (Trcuks.Count == 0)
        {
            Trcuks.Add(truck);
            return true;
        }

        for (var i = 0; i < Trcuks.Count; i++)
        {
            if (Trcuks[i].Type >= truck.Type) 
                continue;
            
            Trcuks.Insert(i, truck);
            return true;
        }
        
        Trcuks.Add(truck);
        return true;
    }
}