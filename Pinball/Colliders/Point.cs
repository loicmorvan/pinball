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

using Pinball.Interfaces;
using Pinball.Math;

namespace Pinball;

public record Point(Vector P, decimal c): ICollider
{
    public Collision? Detect(Ball ball, decimal Δt)
    {
        var (s, X, r) = ball;

        if (s.GetNorm() == 0)
        {
            return null;
        }

        var n = s.Normalize();
        var XP = P - X;
        if (XP * n <= 0)
        {
            return null;
        }

        var t = n.Rotate90CW();
        var XC_t = XP * t;

        if (XC_t <= -r || XC_t >= r)
        {
            return null;
        }

        var XC_n = DecimalEx.Sqrt(r * r - XC_t * XC_t);
        var XC = XC_t * t + XC_n * n;
        var d = (XP - XC).GetNorm();
        var δt = d / s.GetNorm();
        if (δt >= Δt)
        {
            return null;
        }

        var N = -XC.Normalize();

        return new Collision(δt, P, N, c);
    }
}