using Pinball.Interfaces;

namespace Pinball.Impl;

public class CollisionResolver : ICollisionResolver
{
    public Ball ResolveCollision(Ball ball, decimal Δt, Collision collision)
    {
        var (s, x, r) = ball;
        var (δt, p, N) = collision;
        var C = 1;

        var xc = p + r * N;
        var T = N.Rotate90CW();
        s = s * T * T - C * (s * N) * N;
        x = xc + (Δt - δt) * s;

        return ball with { s = s, x = x };
    }
}