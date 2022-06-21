namespace Pinball;

public class Board
{
    // For now, gravity is constant, but should be dependent upon Ball.Position.
    public Vector Gravity { get; set; } = new(0, -10);

    public Ball Ball { get; set; } = new Ball();

    public Vector[] PointColliders { get; set; } = Array.Empty<Vector>();

    public void Step(decimal Δt)
    {
        Ball.a = Gravity;
        Ball.s += Δt * Ball.a;
        Ball.x += Δt * Ball.s;

        foreach(var p in PointColliders)
        {
            var collision = DetectCollisionWithPoint(Ball, Δt, p);
            if (collision != null)
            {
                ResolveCollision(Δt, collision);
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

    public void ResolveCollision(decimal Δt, Collision collision)
    {
        var (δt, p, N) = collision;
        var C = 1;

        var xc = p + Ball.r * N;
        var T = N.Rotate90CW();
        Ball.s = (Ball.s * T) * T - C * (Ball.s * N) * N;
        Ball.x = xc + (Δt - δt) * Ball.s;
    }
}
