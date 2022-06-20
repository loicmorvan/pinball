namespace Pinball;
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

    public static Vector operator -(in Vector vec)
    {
        return new Vector(-vec.X, -vec.Y);
    }

    public decimal GetNorm()
    {
        return DecimalEx.Sqrt(X * X + Y * Y);
    }

    public Vector Normalize()
    {
        var norm = GetNorm();
        return new Vector(X / norm, Y / norm);
    }
}