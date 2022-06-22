using FluentAssertions;

namespace Pinball.Tests;

public class BoardTests
{
    [Fact]
    public void Test1()
    {
        var sut = new Board(new PointCollider(), new CollisionResolver());

        sut.Step(1m);

        Assert.Equal(new Vector(0, -10), sut.Ball.s);
        Assert.Equal(new Vector(0, -10), sut.Ball.x);
    }

    [Fact]
    public void Bug()
    {
        var sut = new Board(new PointCollider(), new CollisionResolver());
        sut.Ball = new(new(0, -3.6m), new(0, -0.684m), 0.25m);
        sut.PointColliders = new[] { new Vector(0, -1) };

        sut.Step(0.02m);

        sut.Ball.x.Y.Should().BeGreaterThan(-0.75m);
    }
}
