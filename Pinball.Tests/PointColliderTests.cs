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

using Pinball.Math;

namespace Pinball.Tests;

public class PointColliderTests
{
    [Fact]
    public void DetectCollisionWithPointTest()
    {
        var sut = new Point(new(0, 0.5m), 1);
        var ball = new Ball(new(0, 1), 0, 0.25m);

        var result = sut.Detect(ball, 1);

        Assert.NotNull(result);
        Assert.Equal(0.25m, result!.δt);
        Assert.Equal(new Vector(0, 0.5m), result.p);
        Assert.Equal(new Vector(0, -1), result.N);
    }

    [Fact]
    public void Bug()
    {
        var sut = new Point(new(0, -1), 1);
        var ball = new Ball(new(0, -3.6m), new(0, -0.684m), 0.25m);

        var result = sut.Detect(ball, 0.02m);

        result.Should().NotBeNull();
    }
}