using Draughts.Shared.Models;

namespace Draughts.Server.Models
{
    public record MakeMoveDto(BoardSquare From, BoardSquare To);
}