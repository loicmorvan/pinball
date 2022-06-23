namespace Pinball.Interfaces;
public interface IDiskCollider
{
    Collision? DetectCollision(Ball ball, decimal Δt, in Disk disk);
}
