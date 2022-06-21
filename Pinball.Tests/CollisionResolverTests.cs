namespace Pinball.Tests;

public class CollisionResolverTests
{
    [Fact]
    public void Test()
    {
        var ball = new Ball(new(0, -1), 0, 0.25m);
        var collision = new Collision(0.5m, new(0, -0.75m), new(0, 1));
        var sut = new CollisionResolver();

        var result = sut.ResolveCollision(ball, 1, collision);

        Assert.NotNull(result);
        Assert.Equal(new(0, 1), result.s);
        Assert.Equal(0, result.x);
    }
}