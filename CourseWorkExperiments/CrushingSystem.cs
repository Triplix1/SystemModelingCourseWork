namespace CourseWork;

public class CrushingSystem: SystemMO
{
    public CrushingSystem(string nameOfElement, IEnumerable<Process> processes, Queue queue, IList<Element> nextElements) : base(nameOfElement, processes, queue, nextElements)
    {
    }
}