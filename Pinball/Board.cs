namespace Pinball;

public class Board
{
    // For now, gravity is constant, but should be dependent upon Ball.Position.
    public Vector Gravity { get; set; } = new(0, -10);

    public Ball Ball { get; set; } = new Ball();

    public Vector[] PointColliders { get; set; } = Array.Empty<Vector>();

    public void Step(decimal dt)
    {
        Ball.Acceleration = Gravity;
        Ball.Speed += dt * Ball.Acceleration;
        Ball.Position += dt * Ball.Speed;

        foreach(var p in PointColliders)
        {
            var collision = DetectCollisionWithPoint(Ball, dt, p);
            if (collision != null)
            {
                ResolveCollision(dt, collision);
            }
        }
    }

    public static Collision? DetectCollisionWithPoint(Ball ball, decimal dt, Vector p)
    {
        var n = ball.Speed.Normalize();
        var xp = p - ball.Position;
        if (xp * n <= 0)
        {
            return null;
        }

        var t = n.Rotate90CW();
        var xcx = xp * t;

        if (xcx <= -ball.Radius || xcx >= ball.Radius)
        {
            return null;
        }

        var xcy = DecimalEx.Sqrt(ball.Radius * ball.Radius - xcx * xcx);
        var xc = new Vector(xcx, xcy);
        var d = (xp - xc).GetNorm();
        var dtc = d / ball.Speed.GetNorm();
        if (dtc >= dt)
        {
            return null;
        }

        var N = -xc.Normalize();

        return new Collision(dtc, p, N);
    }

    public void ResolveCollision(decimal dt, Collision collision)
    {
        var C = 1;
        var xc = collision.Point + Ball.Radius * collision.Normal;
        var T = collision.Normal.Rotate90CW();
        Ball.Speed = (Ball.Speed * T) * T - C * (Ball.Speed * collision.Normal) * collision.Normal;
        Ball.Position = xc + (dt - collision.TimeToCollision) * Ball.Speed;
    }
}
