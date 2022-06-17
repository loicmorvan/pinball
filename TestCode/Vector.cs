namespace TestCode;

public record struct Vector(decimal X, decimal Y)
{
    public static Vector operator *(in decimal scalar, in Vector vector)
    {
        return new Vector(scalar * vector.X, scalar * vector.Y);
    }

    public static decimal operator *(in Vector left, in Vector right)
    {
        return left.X * right.X + left.Y * right.Y;
    }

    public static Vector operator +(in Vector left, in Vector right)
    {
        return new Vector(left.X + right.X, left.Y + right.Y);
    }

    public static Vector operator -(in Vector left, in Vector right)
    {
        return new Vector(left.X - right.X, left.Y - right.Y);
    }
}
