using System.Text.Json.Serialization;

namespace Draughts.Shared.Models.Board
{
    public record Move(BoardSquare From, BoardSquare To, BoardPiece Taken)
    {
        [JsonIgnore]
        public string Identifier => From.Identifier + (Taken is null ? "-" : "x") + To.Identifier;
    };
}