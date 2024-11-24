namespace CourseWorkMS;

public class FunRand
{ 
    // Generates a random value according to an exponential
    // distribution
    // @param timeMean mean value
    // @return a random value according to an exponential
    // distribution
    public static double Exp(double timeMean)
    {
        double a = 0;
        var random = new Random();
        while (a == 0)
        {
            a = random.NextDouble();
        }

        a = -timeMean * Math.Log(1.0 - a);
        return a;
    }

    public static double Normal(double mean, double stdev)
    {
        Random rand = new Random();
        double miu;

        double sum = 0;
        for (int j = 0; j < 12; j++)
        {
            sum += rand.NextDouble();
        }

        miu = sum - 6; // Сума 12 рівномірно розподілених випадкових чисел мінус 6

        miu = stdev * miu + mean; // Масштабуємо значення до заданих mean і stdev

        return miu;
    }

    public static double Erlang(double mean, int k)
    {
        var sum = 0d;
        for (int i = 0; i < k; i++)
        {
            sum += Exp(mean / k);
        }

        return sum;
    }

    // Generates a random value according to a uniform
    // distribution
    // @param timeMin
    // @param timeMax
    // @return a random value according to a uniform distribution
    public static double Unif(double timeMin, double timeMax)
    {
        double a = 0;
        var random = new Random();
        while (a == 0)
        {
            a = random.NextDouble();
        }

        a = timeMin + a * (timeMax - timeMin);
        return a;
    }

    public static double GetDelay(TruckType truckType, Dictionary<TruckType, double> characteristic, Distribution distribution)
    {
        switch (distribution)
        {
            case Distribution.None:
                return characteristic[truckType];
            case Distribution.Exponential:
                return Exp(characteristic[truckType]);
            default:
                throw new ArgumentOutOfRangeException(nameof(distribution), distribution, null);
        }
    }
}