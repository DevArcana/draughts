using System;

namespace Draughts.Shared.Models
{
    public record BoardSquare(int X, int Y)
    {
        public BoardSquare Shift(Direction dir)
        {
            return dir switch
            {
                Direction.UpperRight => new BoardSquare(X - 1, Y - 1),
                Direction.UpperLeft => new BoardSquare(X + 1, Y - 1),
                Direction.LowerLeft => new BoardSquare(X - 1, Y + 1),
                Direction.LowerRight => new BoardSquare(X + 1, Y + 1),
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }
    };
}