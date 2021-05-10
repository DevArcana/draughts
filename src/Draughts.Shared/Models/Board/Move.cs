namespace Draughts.Shared.Models
{
    public record Move(BoardSquare From, BoardSquare To, BoardPiece Taken);
}