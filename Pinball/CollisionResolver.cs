using Pinball.Interfaces;

namespace Pinball.Impl;

public class CollisionResolver : ICollisionResolver
{
    public Ball ResolveCollision(Ball ball, decimal Δt, Collision collision)
    {
        var (s, X, r) = ball;
        var (δt, P, n, c) = collision;

        var XC = P + r * n;
        var t = n.Rotate90CW();
        s = s * t * t - c * (s * n) * n;
        X = XC + (Δt - δt) * s;

        return ball with { s = s, X = X };
    }
}