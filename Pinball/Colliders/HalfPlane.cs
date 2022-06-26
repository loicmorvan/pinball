using Pinball.Interfaces;
using Pinball.Math;

namespace Pinball;

public record struct HalfPlane(Vector p, Vector n, decimal c) : ICollider
{
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

        var d = (C_B - C).GetNorm();
        var δt = d / s.GetNorm();
        if (δt >= Δt)
        {
            return null;
        }

        return new Collision(δt, C, n, c);
    }
}