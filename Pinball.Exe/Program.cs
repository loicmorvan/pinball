using Engine;
using Pinball;
using Pinball.Impl;
using Pinball.Interfaces;

var board = new Board(new CollisionResolver());
board.Ball = new(new(0.1m, 0), new(0, 10), 0.25m);
board.Colliders = new ICollider[]
{
    new HalfPlane(0, new(0, 1), 0.9m),
    new HalfPlane(new(10m, 0), new(-1, 0), 0.9m),
    new HalfPlane(new(-10m, 0), new(1, 0), 0.9m),
    new Disk(0, 5, 1)
};

var window = new Window();
window.Room.Camera = new(-10, 0, 20, 10);
window.Room.GameObjects.Add(new Pinball.Exe.Board(board));
window.Room.GameObjects.Add(new GameObject() { Sprite = new("", 10, 10, 5m, 5m) });
window.Room.GameObjects.Add(new Pinball.Exe.Ball(board) { Sprite = new("", 0.5m, 0.5m, 0.25m, 0.25m) });
window.Run();
