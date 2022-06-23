using Pinball.Math;

namespace Pinball.Interfaces;

public interface IPointCollider
{
    Collision? DetectCollisionWithPoint(Ball ball, decimal Δt, Vector p);
}
