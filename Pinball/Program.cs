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

using System.Reflection;
using Engine;
using Engine.Graphics;
using Physics;
using Physics.Colliders;
using Pinball;

var board = new Physics.Board(0.01m);
board.Ball = new(new(0.1m, 0), new(0, 10), 0.25m);
var resolver = new CollisionResolver();
board.Colliders = new (ICollider, ICollisionResolver)[]
{
    (new HalfPlane(0, new(0, 1), 0.9m), resolver),
    (new HalfPlane(new(10m, 0), new(-1, 0), 0.9m), resolver),
    (new HalfPlane(new(-10m, 0), new(1, 0), 0.9m), resolver)
};

var window = new Window(new AssemblyResourceRepository(Assembly.GetExecutingAssembly()));
window.Room.Camera = new(-10, 0, 20, 10);
window.Room.GameObjects.Add(new Pinball.Board(board));
window.Room.GameObjects.Add(new Bumper(board, new(0, 0)));
window.Room.GameObjects.Add(new Bumper(board, new(1, 1)));
window.Room.GameObjects.Add(new Bumper(board, new(2, 2)));
window.Room.GameObjects.Add(new Bumper(board, new(3, 3)));
window.Room.GameObjects.Add(new Bumper(board, new(4, 4)));
window.Room.GameObjects.Add(new Bumper(board, new(5, 5)));
window.Room.GameObjects.Add(new Pinball.Ball(board) { Sprite = new("Pinball.Resources.Sample.png", 0.5m, 0.5m, 0.25m, 0.25m) });
window.Run();
