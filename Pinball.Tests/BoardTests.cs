using Xunit;

namespace Pinball.Tests;

public class BoardTests
{
    [Fact]
    public void Test1()
    {
        var sut = new Board();

        sut.Step(1m);

        Assert.Equal(new Vector(0, -10), sut.Ball.s);
        Assert.Equal(new Vector(0, -10), sut.Ball.x);
    }

    [Fact]
    public void Test()
    {
        var sut = new Ball(new Vector(0, 1), 0, 0.25m);

        var result = Board.DetectCollisionWithPoint(sut, 1, new Vector(0, 0.5m));

        Assert.NotNull(result);
        Assert.Equal(0.25m, result!.δt);
        Assert.Equal(new Vector(0, 0.5m), result.p);
        Assert.Equal(new Vector(0, -1), result.N);
    }
}