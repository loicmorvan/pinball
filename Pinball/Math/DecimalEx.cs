namespace Pinball.Math;
public static class DecimalEx
{
    public static decimal Sqrt(decimal value)
    {
        return (decimal)System.Math.Sqrt((double)value);
    }
}