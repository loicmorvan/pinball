using Pinball.Interfaces;
using Pinball.Math;

namespace Pinball.Impl;

public class PointCollider : IPointCollider
{
    public Collision? DetectCollisionWithPoint(Ball ball, decimal Δt, Vector p)
    {
        var (s, x, r) = ball;

        if (s.GetNorm() == 0)
        {
            return null;
        }

        var n = s.Normalize();
        var xp = p - x;
        if (xp * n <= 0)
        {
            return null;
        }

        var t = n.Rotate90CW();
        var xct = xp * t;

        if (xct <= -r || xct >= r)
        {
            return null;
        }

        var xcn = DecimalEx.Sqrt(r * r - xct * xct);
        var xc = xct * t + xcn * n;
        var d = (xp - xc).GetNorm();
        var δt = d / s.GetNorm();
        if (δt >= Δt)
        {
            return null;
        }

        var N = -xc.Normalize();

        return new Collision(δt, p, N);
    }
}