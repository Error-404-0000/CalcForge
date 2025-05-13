namespace CalcForge;

//created so the risk of getting a Param miss count/wrong type is very long to System.Math
public static class Math
{
  
    public static decimal Truncate(decimal value)
    => (decimal)System.Math.Truncate((double)value);

    public static double Pow(double x, double y)
        => System.Math.Pow(x,y);

    public static double Add(double a, double b)
        => a + b;

    public static double Add(double a, double b, double c)
        => a + b + c;
    public static double Print(double value)
    {
        Console.WriteLine(value);
        return 0;
    }
    public static double Subtract(double a, double b)
        => a - b;

    public static double Multiply(double a, double b)
        => a * b;

    public static double Divide(double a, double b)
        => b != 0 ? a / b : throw new DivideByZeroException();

    public static double Abs(double value)
        => System.Math.Abs(value);

    public static double Floor(double value)
        => System.Math.Floor(value);

    public static double Ceiling(double value)
        => System.Math.Ceiling(value);

    public static double Round(double value)
        => System.Math.Round(value);

    public static double Max(double a, double b)
        => System.Math.Max(a, b);

    public static double Min(double a, double b)
        => System.Math.Min(a, b);

    public static double Sqrt(double value)
        => System.Math.Sqrt(value);

    public static double Log(double value)
        => System.Math.Log(value);

    public static double Log10(double value)
        => System.Math.Log10(value);

    public static decimal Exp(decimal value)
        => (decimal)System.Math.Exp((double)value);

    public static double Sin(double value)
        =>System.Math.Sin(value);

    public static double Cos(double value)
        => System.Math.Cos(value);

    public static double Tan(double value)
        => System.Math.Tan(value);
}
