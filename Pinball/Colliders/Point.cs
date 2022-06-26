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