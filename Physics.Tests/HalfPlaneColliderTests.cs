/*
 Copyright (c) 2022 Loïc Morvan

 Permission is hereby granted, free of charge, to any person obtaining a copy of
 this software and associated documentation files (the "Software"), to deal in
 the Software without restriction, including without limitation the rights to
 use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 the Software, and to permit persons to whom the Software is furnished to do so,
 subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using Physics.Colliders;
using Physics.Math;

namespace Physics.Tests;

public class HalfPinballColliderTests
{
    [Fact]
    public void Test()
    {
        var ball = new Ball(new(0, -1), new(0, 1), 0.25m);
        var sut = new HalfPlane(0, new(0, 1), 1);

        var result = sut.Detect(ball, 1);

        result.Should().NotBeNull(because: "a collision should have been detected");
        result!.δt.Should().Be(0.75m, because: "the ball should fall by 0.75 until its surface reaches the half plane");
        result.p.Should().Be(new Vector(0, 0), because: "the ball trivially touched the plane at 0");
        result.N.Should().Be(new Vector(0, 1), because: "the half-plane is pointing up");
    }
}