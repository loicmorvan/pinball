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
        var sut = new Ball();
        sut.Speed = new Vector(0, 1);
        sut.Radius = 0.25m;

        var result = Board.DetectCollisionWithPoint(sut, 1, new Vector(0, 0.5m));

        Assert.NotNull(result);
        Assert.Equal(0.25m, result!.TimeToCollision);
        Assert.Equal(new Vector(0, 0.5m), result.Point);
        Assert.Equal(new Vector(0, -1), result.Normal);
    }
}