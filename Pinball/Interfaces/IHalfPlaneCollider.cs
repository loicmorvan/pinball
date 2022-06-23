namespace Pinball.Interfaces;
public interface IHalfPlaneCollider
{
    Collision? DetectCollisionWithHalfPlane(Ball ball, decimal Δt, in HalfPlane halfPlane);
}
