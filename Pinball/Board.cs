namespace Pinball;

public class Board
{
    // For now, gravity is constant, but should be dependent upon Ball.Position.
    public Vector g { get; set; } = new(0, -10);

    public Ball Ball { get; set; } = new Ball(0, 0, 0.25m);

    public Vector[] PointColliders { get; set; } = Array.Empty<Vector>();

    public void Step(decimal Δt)
    {
        var (s, x, r) = Ball;

        var a = g;
        s += Δt * a;
        x += Δt * s;

        Ball = new(s, x, r);

        foreach (var p in PointColliders)
        {
            var collision = DetectCollisionWithPoint(Ball, Δt, p);
            if (collision != null)
            {
                Ball = ResolveCollision(Ball, Δt, collision);
            }
        }
    }

    public static Collision? DetectCollisionWithPoint(Ball ball, decimal Δt, Vector p)
    {
        var n = ball.s.Normalize();
        var xp = p - ball.x;
        if (xp * n <= 0)
        {
            return null;
        }

        var t = n.Rotate90CW();
        var xcx = xp * t;

        if (xcx <= -ball.r || xcx >= ball.r)
        {
            return null;
        }

        var xcy = DecimalEx.Sqrt(ball.r * ball.r - xcx * xcx);
        var xc = new Vector(xcx, xcy);
        var d = (xp - xc).GetNorm();
        var δt = d / ball.s.GetNorm();
        if (δt >= Δt)
        {
            return null;
        }

        var N = -xc.Normalize();

        return new Collision(δt, p, N);
    }

    public static Ball ResolveCollision(Ball ball, decimal Δt, Collision collision)
    {
        var (s, x, r) = ball;
        var (δt, p, N) = collision;
        var C = 1;

        var xc = p + r * N;
        var T = N.Rotate90CW();
        s = (s * T) * T - C * (s * N) * N;
        x = xc + (Δt - δt) * s;

        return ball with { s = s, x = x };
    }
}
