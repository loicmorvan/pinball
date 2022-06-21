using Xunit;

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
}
