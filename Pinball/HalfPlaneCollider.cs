namespace Pinball;

public class HalfPlaneCollider : IHalfPlaneCollider
{
    public Collision? DetectCollisionWithHalfPlane(Ball ball, decimal Δt, in HalfPlane halfPlane)
    {
        var (s, x, r) = ball;
        var (p, n) = halfPlane;

        if (n * s >= 0)
        {
            return null;
        }

        var cB = x - r * n;
        var t = n.Rotate90CW();
        var c = p + (((cB ^ s) - (p ^ s)) / (t ^ s)) * t;

        var d = (cB - c).GetNorm();
        var δt = d / s.GetNorm();
        if (δt >= Δt)
        {
            return null;
        }

        return new Collision(δt, c, n);
    }
}