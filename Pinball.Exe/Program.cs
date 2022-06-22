using Pinball;

var board = new Board(new PointCollider(), new CollisionResolver());
board.Ball = new(0, 0, 0.25m);
board.PointColliders = new[] { new Vector(0, -1) };

using var file = File.CreateText("output.csv");
file.WriteLine("iteration,s.x,s.y,x.x,x.y");

for (int i = 0; i < 1000; ++i)
{
    // 50Hz -> 20s
    board.Step(0.02m);
    file.WriteLine($"{i},{board.Ball.s.X},{board.Ball.s.Y},{board.Ball.x.X},{board.Ball.x.Y}");
}
