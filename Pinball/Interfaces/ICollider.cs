namespace Pinball.Interfaces;

public interface ICollider
{
    Collision? Detect(Ball ball, decimal Δt);
}