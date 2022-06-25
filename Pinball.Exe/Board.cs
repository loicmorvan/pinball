using Engine;

namespace Pinball.Exe;

public class Board: GameObject
{
    private Pinball.Board board;

    public Board(Pinball.Board board)
    {
        this.board = board;
    }

    protected override void Step(decimal Δt)
    {
        board.Step(Δt);
    }
}