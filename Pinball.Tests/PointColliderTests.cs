using Pinball.Impl;
using Pinball.Math;

namespace Pinball.Tests;

public class PointColliderTests
{
    [Fact]
    public void DetectCollisionWithPointTest()
    {
        var sut = new PointCollider();
        var ball = new Ball(new Vector(0, 1), 0, 0.25m);

        var result = sut.DetectCollisionWithPoint(ball, 1, new Vector(0, 0.5m));

        Assert.NotNull(result);
        Assert.Equal(0.25m, result!.δt);
        Assert.Equal(new Vector(0, 0.5m), result.p);
        Assert.Equal(new Vector(0, -1), result.N);
    }

    [Fact]
    public void Bug()
    {
        var sut = new PointCollider();
        var ball = new Ball(new(0, -3.6m), new(0, -0.684m), 0.25m);

        var result = sut.DetectCollisionWithPoint(ball, 0.02m, new(0, -1));

        result.Should().NotBeNull();
    }
}