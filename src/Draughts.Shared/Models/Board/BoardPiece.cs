namespace Draughts.Shared.Models
{
    public record BoardPiece(Side Side, BoardSquare Pos, bool Promoted = false)
    {
        public BoardPiece Move(BoardSquare pos, bool promote = false) => new BoardPiece(Side, pos, Promoted || promote);
    }
}