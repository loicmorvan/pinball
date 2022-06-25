using Engine;

namespace Pinball.Exe;

public class Ball: GameObject
{
    private readonly Pinball.Board board;

    public Ball(Pinball.Board board)
    {
        this.board = board;
    }

    protected override void Step(decimal Δt)
    {
        X = board.Ball.x.X;
        Y = board.Ball.x.Y;
    }
}