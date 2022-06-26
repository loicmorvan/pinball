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