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
