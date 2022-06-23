using Pinball;
using Pinball.Impl;

var board = new Board(new HalfPlaneCollider(), new DiskCollider(), new PointCollider(), new CollisionResolver());
board.Ball = new(new(0.1m, 0), new(0, 10), 0.25m);
board.HalfPlaneColliders = new[] { new HalfPlane(0, new(0, 1)), new HalfPlane(new(10m, 0), new(-1, 0)), new HalfPlane(new(-10m, 0), new(1, 0)) };
board.DiskColliders = new[] { new Disk(0, 5) };
// board.PointColliders = Enumerable.Range(0, 2500).Select(i => new Vector(0.1m * i, 0)).ToArray();

using var file = File.CreateText("output.csv");
file.WriteLine("iteration,x,y,energie cinetique,energie potentielle,energie mecanique");

for (int i = 0; i < 10000; ++i)
{
    // 50Hz -> 20s
    board.Step(0.02m);
    var (s, x, _) = board.Ball;
    var ec = (s.X * s.X + s.Y * s.Y) / 2; // 1/2 m v^2
    var ep = 10 * x.Y; // m g deltah
    var em = ec + ep;
    file.WriteLine($"{i},{x.X},{x.Y},{ec},{ep},{em}");
}
