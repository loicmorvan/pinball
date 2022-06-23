namespace Pinball.Tests;
public class HalfPinballColliderTests
{
    [Fact]
    public void Test()
    {
        var sut = new HalfPlaneCollider();
        var ball = new Ball(new(0, -1), new(0, 1), 0.25m);
        var halfPlane = new HalfPlane(0, new(0, 1));

        var result = sut.DetectCollisionWithHalfPlane(ball, 1, halfPlane);

        result.Should().NotBeNull(because: "a collision should have been detected");
        result!.δt.Should().Be(0.75m, because: "the ball should fall by 0.75 until its surface reaches the half plane");
        result.p.Should().Be(new Vector(0, 0), because: "the ball trivially touched the plane at 0");
        result.N.Should().Be(new Vector(0, 1), because: "the half-plane is pointing up");
    }
}