using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Draughts.Shared.Models.Board
{
    public record AvailableMove(Move[] Moves)
    {
        [JsonIgnore]
        public BoardSquare From => Moves[0].From;
        
        [JsonIgnore]
        public BoardSquare To => Moves.Last().To;
        
        [JsonIgnore]
        public IEnumerable<BoardPiece> Takes => Moves.Where(x => x.Taken is not null).Select(x => x.Taken);

        public string Identifier => string.Join(" ", Moves.Select(x => x.Identifier));
    };
}