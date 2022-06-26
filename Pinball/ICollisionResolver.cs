namespace Pinball.Interfaces;

public interface ICollisionResolver
{
    Ball ResolveCollision(Ball ball, decimal Δt, Collision collision);
}
