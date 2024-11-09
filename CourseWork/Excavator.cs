namespace CourseWork;

public class Excavator: SystemMO
{
    public Excavator(string nameOfElement, IEnumerable<Process> processes, Queue queue, IList<Element> nextElements) : base(nameOfElement, processes, queue, nextElements)
    {
    }
}