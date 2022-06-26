using Pinball.Interfaces;
using Pinball.Math;

namespace Pinball;

public class Board
{
    private readonly decimal targetTimestep;
    private decimal elapsedTime;

    private readonly ICollisionResolver collisionResolver;

    public Board(decimal targetTimestep, ICollisionResolver collisionResolver)
    {
        this.targetTimestep = targetTimestep;
        this.collisionResolver = collisionResolver;
    }

    // For now, gravity is constant, but should be dependent upon Ball.Position.
    public Vector g { get; set; } = new(0, -10);

    public Ball Ball { get; set; } = new Ball(0, 0, 0.25m);

    public ICollider[] Colliders {get;set;}=Array.Empty<ICollider>();

    public void Step(decimal Δt)
    {
        elapsedTime += Δt;

        while (elapsedTime >= targetTimestep)
        {
            var iterationTime = targetTimestep;

            var (s, X, r) = Ball;

            var a = g;
            s += iterationTime * a;

            var ball = new Ball(s, X, r);

            bool hasCollided = false;
            
            foreach (var collider in Colliders)
            {
                var collision = collider.Detect(ball, iterationTime);
                if (collision != null)
                {
                    Ball = collisionResolver.ResolveCollision(ball, iterationTime, collision);
                    iterationTime -= collision.δt;
                    hasCollided = true;
                }
            }

            if (!hasCollided)
            {
                X += iterationTime * s;
                Ball = ball with { X = X };
            }

            elapsedTime -= targetTimestep;
        }
    }
}
