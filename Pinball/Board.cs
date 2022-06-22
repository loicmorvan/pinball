namespace Pinball;

public class Board
{
    private readonly IPointCollider pointCollider;
    private readonly ICollisionResolver collisionResolver;

    public Board(IPointCollider pointCollider, ICollisionResolver collisionResolver)
    {
        this.pointCollider = pointCollider;
        this.collisionResolver = collisionResolver;
    }

    // For now, gravity is constant, but should be dependent upon Ball.Position.
    public Vector g { get; set; } = new(0, -10);

    public Ball Ball { get; set; } = new Ball(0, 0, 0.25m);

    public Vector[] PointColliders { get; set; } = Array.Empty<Vector>();

    public void Step(decimal Δt)
    {
        var (s, x, r) = Ball;

        var a = g;
        s += Δt * a;

        var ball = new Ball(s, x, r);

        bool hasCollided = false;
        foreach (var p in PointColliders)
        {
            var collision = pointCollider.DetectCollisionWithPoint(ball, Δt, p);
            if (collision != null)
            {
                Ball = collisionResolver.ResolveCollision(ball, Δt, collision);
                hasCollided = true;
                break;
            }
        }

        if (!hasCollided)
        {
            x += Δt * s;
            Ball = ball with { x = x };
        }
    }
}
