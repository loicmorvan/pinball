namespace Pinball;
public static class DecimalEx
{
    public static decimal Sqrt(decimal value)
    {
        return (decimal)Math.Sqrt((double)value);
    }
}