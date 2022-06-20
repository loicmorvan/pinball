namespace Pinball;

public record struct Collision(decimal TimeToCollision, Vector CollisionPoint, Vector ColliderNormal);

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
    }

    public Collision? CollideWithPoint(decimal dt, Vector p)
    {
        var n = Ball.Speed.Normalize();
        var xp = p - Ball.Position;
        if (xp * n <= 0)
        {
            return null;
        }

        var t = new Vector(-n.Y, n.X);
        var xcx = xp * t;

        if (xcx <= -Ball.Radius || xcx >= Ball.Radius)
        {
            return null;
        }

        var xcy = DecimalEx.Sqrt(Ball.Radius * Ball.Radius - xcx * xcx);
        var xc = new Vector(xcx, xcy);
        var d = (xp - xc).GetNorm();
        var dtc = d / Ball.Speed.GetNorm();
        if (dtc >= dt)
        {
            return null;
        }

        var N = -xc.Normalize();

        return new Collision(dtc, Ball.Position + xc, N);
    }
}
