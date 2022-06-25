using Pinball.Interfaces;
using Pinball.Math;

namespace Pinball;

public class Board
{
    private readonly ICollisionResolver collisionResolver;

    public Board(ICollisionResolver collisionResolver)
    {
        this.collisionResolver = collisionResolver;
    }

    // For now, gravity is constant, but should be dependent upon Ball.Position.
    public Vector g { get; set; } = new(0, -10);

    public Ball Ball { get; set; } = new Ball(0, 0, 0.25m);

    public ICollider[] Colliders {get;set;}=Array.Empty<ICollider>();

    public void Step(decimal Δt)
    {
        var (s, x, r) = Ball;

        var a = g;
        s += Δt * a;

        var ball = new Ball(s, x, r);

        bool hasCollided = false;
        
        foreach (var collider in Colliders)
        {
            var collision = collider.Detect(ball, Δt);
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
