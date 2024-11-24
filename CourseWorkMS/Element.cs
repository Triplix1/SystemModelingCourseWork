namespace CourseWorkMS;

public abstract class Element
{
    public string Name { get; set; }
    public virtual double TimeNext { get; set; } = double.MaxValue;
    public virtual double TimeCurrent { get; set; }
    public int Quantity { get; set; }
    public virtual bool IsBusy { get; set; }
    public Element(string nameOfElement)
    {
        Name = nameOfElement;
    }

    public abstract void InAct(Truck truck);

    public abstract Truck OutAct();

    public virtual void PrintInfo()
    {
        var timeNext = Math.Abs(TimeNext - double.MaxValue) < 0.01 ? "infinity" : $"{TimeNext:F2}";
        Console.WriteLine(Name + " State = " + IsBusy +
                          "; Quantity = " + Quantity +
                          $"; Time next = " + timeNext);
    }
}