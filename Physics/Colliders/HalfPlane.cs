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

using Physics.Math;

namespace Physics.Colliders;

public record struct HalfPlane(Vector p, Vector n, decimal c) : ICollider
{
    public (decimal, Vector) GetDistanceTo(Ball ball)
    {
        return (CollisionDetector.Detect(new Math.Disk(ball.X, ball.r), new Math.HalfPlane(p, n)), n);
    }

    public Collision? Detect(Ball ball, decimal Δt)
    {
        var (s, x, r) = ball;

        if (n * s >= 0)
        {
            return null;
        }

        var C_B = x - r * n;
        var t = n.Rotate90CW();
        var C = p + ((C_B ^ s) - (p ^ s)) / (t ^ s) * t;

        var CC_B = C_B - C;
        var d = CC_B.GetNorm();
        var δt = d / s.GetNorm();
        if (δt >= Δt)
        {
            return null;
        }

        return new Collision(δt, C, n, c);
    }
}