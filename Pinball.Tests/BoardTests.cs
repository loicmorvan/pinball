using Xunit;

namespace Pinball.Tests;

public class BoardTests
{
    [Fact]
    public void Test1()
    {
        var sut = new Board();

        sut.Step(1m);

        Assert.Equal(new Vector(0, -10), sut.Ball.Speed);
        Assert.Equal(new Vector(0, -10), sut.Ball.Position);
    }

    [Fact]
    public void Test()
    {
        var sut = new Board();
        sut.Ball.Speed = new Vector(0, 1);
        sut.Ball.Radius = 0.25m;

        var result = sut.CollideWithPoint(1, new Vector(0, 0.5m));

        Assert.NotNull(result);
        Assert.Equal(0.25m, result.Value.TimeToCollision);
        Assert.Equal(new Vector(0, 0.5m), result.Value.Point);
        Assert.Equal(new Vector(0, -1), result.Value.Normal);
    }
}