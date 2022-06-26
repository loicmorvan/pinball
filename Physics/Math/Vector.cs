/*
 Copyright (c) 2022 Loïc Morvan

 Permission is hereby granted, free of charge, to any person obtaining a copy of
 this software and associated documentation files (the "Software"), to deal in
 the Software without restriction, including without limitation the rights to
 use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 the Software, and to permit persons to whom the Software is furnished to do so,
 subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace Physics.Math;

public record struct Vector(decimal X, decimal Y)
{
    public static decimal operator ^(in Vector left, in Vector right)
    {
        return left.X * right.Y - left.Y * right.X;
    }

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

    public Vector Rotate90CW()
    {
        return new Vector(-Y, X);
    }

    public static implicit operator Vector(in decimal value)
    {
        return new Vector(value, value);
    }
}
