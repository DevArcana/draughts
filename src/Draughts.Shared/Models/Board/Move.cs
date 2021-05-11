using System.Text.Json.Serialization;

namespace Draughts.Shared.Models.Board
{
    public record Move(BoardSquare From, BoardSquare To, BoardPiece Taken);
}