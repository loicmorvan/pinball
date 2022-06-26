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

namespace Physics.Tests;

public class CollisionResolverTests
{
    [Fact]
    public void Test()
    {
        var ball = new Ball(new(0, -1), 0, 0.25m);
        var collision = new Collision(0.5m, new(0, -0.75m), new(0, 1), 1);
        var sut = new CollisionResolver();

        var result = sut.ResolveCollision(ball, 1, collision);

        Assert.NotNull(result);
        Assert.Equal(new(0, 1), result.s);
        Assert.Equal(0, result.X);
    }
}