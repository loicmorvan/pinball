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

public record struct Disk(Vector P, decimal rD, decimal c): ICollider
{
    public Collision? Detect(Ball ball, decimal Δt)
    {
        var (s, X, r) = ball;

        var XP = P - X;
        if (s * XP <= 0)
        {
            return null;
        }

        var sNorm = s.Normalize();
        var D = X + XP * sNorm * sNorm;

        var DP = P - D;
        if (DP.GetNorm() >= (r + rD))
        {
            return null;
        }

        var X2 = D - DecimalEx.Sqrt((r + rD) * (r + rD) - DP * DP) * sNorm;
        var n = (X2 - P).Normalize();
        var C = P + rD * n;

        var δt = (X2 - X).GetNorm() / s.GetNorm();
        if (δt >= Δt)
        {
            return null;
        }

        return new Collision(δt, C, n, c);
    }
}