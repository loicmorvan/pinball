using Pinball.Interfaces;
using Pinball.Math;

namespace Pinball;

public record struct HalfPlane(Vector p, Vector n) : ICollider
{
    public Collision? Detect(Ball ball, decimal Δt)
    {
        var (s, x, r) = ball;

        if (n * s >= 0)
        {
            return null;
        }

        var cB = x - r * n;
        var t = n.Rotate90CW();
        var c = p + ((cB ^ s) - (p ^ s)) / (t ^ s) * t;

        var d = (cB - c).GetNorm();
        var δt = d / s.GetNorm();
        if (δt >= Δt)
        {
            return null;
        }

        return new Collision(δt, c, n);
    }
}