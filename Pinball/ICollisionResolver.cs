namespace Pinball;

public interface ICollisionResolver
{
    Ball ResolveCollision(Ball ball, decimal Δt, Collision collision);
}
