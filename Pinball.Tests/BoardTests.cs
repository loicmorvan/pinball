using Pinball.Impl;
using Pinball.Math;

namespace Pinball.Tests;

public class BoardTests
{
    [Fact]
    public void Test1()
    {
        var sut = new Board(1m, new CollisionResolver());

        sut.Step(1m);

        Assert.Equal(new Vector(0, -10), sut.Ball.s);
        Assert.Equal(new Vector(0, -10), sut.Ball.X);
    }

    [Fact]
    public void Bug()
    {
        var sut = new Board(0.02m, new CollisionResolver());
        sut.Ball = new(new(0, -3.6m), new(0, -0.684m), 0.25m);
        sut.Colliders = new[] { new Point(new(0, -1), 1) };

        sut.Step(0.02m);

        sut.Ball.X.Y.Should().BeGreaterThan(-0.75m);
    }
}
