namespace Pinball;

public class PointCollider: IPointCollider
{
    public Collision? DetectCollisionWithPoint(Ball ball, decimal Δt, Vector p)
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
}