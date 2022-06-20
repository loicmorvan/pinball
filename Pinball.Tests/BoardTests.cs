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
}