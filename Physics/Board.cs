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

namespace Physics;

public class Board
{
    private readonly decimal targetTimestep;
    private decimal elapsedTime;

    private readonly ICollisionResolver collisionResolver;

    public Board(decimal targetTimestep, ICollisionResolver collisionResolver)
    {
        this.targetTimestep = targetTimestep;
        this.collisionResolver = collisionResolver;
    }

    // For now, gravity is constant, but should be dependent upon Ball.Position.
    public Vector g { get; set; } = new(0, -10);

    public Ball Ball { get; set; } = new Ball(0, 0, 0.25m);

    public ICollider[] Colliders { get; set; } = Array.Empty<ICollider>();

    public void Step(decimal Δt)
    {
        elapsedTime += Δt;

        while (elapsedTime >= targetTimestep)
        {
            var iterationTime = targetTimestep;

            var (s, X, r) = Ball;

            var a = g;
            s += iterationTime * a;

            var ball = new Ball(s, X, r);

            bool hasCollided = false;

            foreach (var collider in Colliders)
            {
                var collision = collider.Detect(ball, iterationTime);
                if (collision != null)
                {
                    Ball = collisionResolver.ResolveCollision(ball, iterationTime, collision);
                    iterationTime -= collision.δt;
                    hasCollided = true;
                }
            }

            if (!hasCollided)
            {
                X += iterationTime * s;
                Ball = ball with { X = X };
            }

            elapsedTime -= targetTimestep;
        }
    }
}
