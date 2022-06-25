using Pinball.Interfaces;
using Pinball.Math;

namespace Pinball;

public class Board
{
    private readonly IHalfPlaneCollider halfPlaneCollider;
    private readonly IDiskCollider diskCollider;
    private readonly IPointCollider pointCollider;
    private readonly ICollisionResolver collisionResolver;

    public Board(IHalfPlaneCollider halfPlaneCollider, IDiskCollider diskCollider, IPointCollider pointCollider, ICollisionResolver collisionResolver)
    {
        this.halfPlaneCollider = halfPlaneCollider;
        this.diskCollider = diskCollider;
        this.pointCollider = pointCollider;
        this.collisionResolver = collisionResolver;
    }

    // For now, gravity is constant, but should be dependent upon Ball.Position.
    public Vector g { get; set; } = new(0, -10);

    public Ball Ball { get; set; } = new Ball(0, 0, 0.25m);

    public Vector[] PointColliders { get; set; } = Array.Empty<Vector>();

    public Disk[] DiskColliders { get; set; } = Array.Empty<Disk>();

    public HalfPlane[] HalfPlaneColliders { get; set; } = Array.Empty<HalfPlane>();

    public void Step(decimal Δt)
    {
        var (s, x, r) = Ball;

        var a = g;
        s += Δt * a;

        var ball = new Ball(s, x, r);

        bool hasCollided = false;
        foreach (var halfPlane in HalfPlaneColliders)
        {
            var collision = halfPlaneCollider.DetectCollisionWithHalfPlane(ball, Δt, halfPlane);
            if (collision != null)
            {
                Δt = Δt - collision.δt;
                Ball = collisionResolver.ResolveCollision(ball, Δt, collision);
                hasCollided = true;
            }
        }

        foreach (var disk in DiskColliders)
        {
            var collision = diskCollider.DetectCollision(ball, Δt, disk);
            if (collision != null)
            {
                Δt = Δt - collision.δt;
                Ball = collisionResolver.ResolveCollision(ball, Δt, collision);
                hasCollided = true;
            }
        }

        foreach (var p in PointColliders)
        {
            var collision = pointCollider.DetectCollisionWithPoint(ball, Δt, p);
            if (collision != null)
            {
                Δt = Δt - collision.δt;
                Ball = collisionResolver.ResolveCollision(ball, Δt, collision);
                hasCollided = true;
            }
        }

        if (!hasCollided)
        {
            x += Δt * s;
            Ball = ball with { x = x };
        }
    }
}
