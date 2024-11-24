namespace CourseWorkMS;

public interface IStatisticable
{
    void DoStatistic(double delta);
    double GetStatistic();
    void PrintResult(double time);
}