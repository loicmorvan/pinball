namespace Pinball;

public interface IPointCollider
{
    Collision? DetectCollisionWithPoint(Ball ball, decimal Δt, Vector p);
}
