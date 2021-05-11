using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Shared.Models
{
    public class Board
    {
        public static int Size = 10;
        public BoardPiece[] Pieces { get; set; } = new BoardPiece[Size * Size];
        public AvailableMove[] AvailableMoves { get; set; } = Array.Empty<AvailableMove>();
        public Side CurrentTurn { get; set; }

        private int PosIndex(int x, int y) => x + y * Size;

        private BoardPiece GetPiece(BoardSquare square) => GetPiece(square.X, square.Y);
        
        private BoardPiece GetPiece(int x, int y)
        {
            var pos = PosIndex(x, y);
            
            if (pos < 0 || pos >= Pieces.Length)
            {
                return null;
            }

            return Pieces[pos];
        }

        private void SetPiece(BoardSquare square, BoardPiece piece) => SetPiece(square.X, square.Y, piece);

        private void SetPiece(int x, int y, BoardPiece piece)
        {
            Pieces[PosIndex(x, y)] = piece;
        }

        private IEnumerable<Move[]> FindMoves(Side side, BoardSquare pos, Direction dir, HashSet<BoardSquare> visited = null, bool capture = false)
        {
            visited ??= new HashSet<BoardSquare>();
            var square = pos.Shift(dir);

            if (visited.Contains(square))
            {
                return Array.Empty<Move[]>();
            }
            
            var piece = GetPiece(square);

            if (piece is null)
            {
                return capture ? Array.Empty<Move[]>() : new[] {new []{ new Move(pos, square, null)}};
            }

            if (piece.Side != side)
            {
                var jump = square.Shift(dir);
                    
                if (GetPiece(jump) is null)
                {
                    var move = new Move(pos, jump, piece);

                    visited.Add(square);
                    var paths = new List<Move[]>();
                    paths.AddRange(FindMoves(side, jump, Direction.UpperLeft, visited, true));
                    paths.AddRange(FindMoves(side, jump, Direction.UpperRight, visited, true));
                    paths.AddRange(FindMoves(side, jump, Direction.LowerLeft, visited, true));
                    paths.AddRange(FindMoves(side, jump, Direction.LowerRight, visited, true));
                    visited.Remove(square);

                    if (!paths.Any())
                    {
                        return new[] {new[] {move}};
                    }
                    
                    var maxLength = paths.Max(x => x.Length);
                    return paths.Where(x => x.Length == maxLength).Select(path =>
                    {
                        var moves = new List<Move> {move};
                        moves.AddRange(path);
                        return moves.ToArray();
                    });
                }
            }

            return Array.Empty<Move[]>();
        }

        private AvailableMove[] FindMoves(BoardPiece piece)
        {
            var moves = new List<Move[]>(4);

            var upperLeft = FindMoves(piece.Side, piece.Pos, Direction.UpperLeft);
            moves.AddRange(upperLeft);
            
            var upperRight = FindMoves(piece.Side, piece.Pos, Direction.UpperRight);
            moves.AddRange(upperRight);
            
            var lowerLeft = FindMoves(piece.Side, piece.Pos, Direction.LowerLeft);
            moves.AddRange(lowerLeft);
            
            var lowerRight = FindMoves(piece.Side, piece.Pos, Direction.LowerRight);
            moves.AddRange(lowerRight);

            if (moves.Any(x => x[0].Taken is not null))
            {
                var maxLength = moves.Max(x => x.Length);
                return moves.Where(x => x.Length == maxLength).Select(x => new AvailableMove(x)).ToArray();
            }

            return moves.Select(x => new AvailableMove(x)).ToArray();
        }
        
        public AvailableMove[] GetAvailableMoves()
        {
            var availableMoves = new List<AvailableMove>();

            foreach (var piece in Pieces.Where(x => x is not null && x.Side == CurrentTurn))
            {
                availableMoves.AddRange(FindMoves(piece));
            }

            var jumps = availableMoves.Where(move => move.Moves.Any(m => m.Taken is not null)).ToList();

            if (jumps.Any())
            {
                var maxLength = jumps.Max(x => x.Moves.Length);
                return jumps.Where(x => x.Moves.Length == maxLength).ToArray();
            }
            
            return availableMoves.ToArray();
        }
        
        public AvailableMove MakeMove(AvailableMove move)
        {
            var piece = GetPiece(move.From);

            if (piece is null)
            {
                return null;
            }

            var availableMove = AvailableMoves.FirstOrDefault(m => m.To == move.To);

            if (availableMove is null)
            {
                return null;
            }

            var to = move.To;
            SetPiece(move.From, null);
            var promote = (piece.Side == Side.Black && to.Y == Size - 1) || (piece.Side == Side.White && to.Y == 0);
            SetPiece(to, piece.Move(to, promote));

            foreach (var taken in move.Takes)
            {
                SetPiece(taken.Pos, null);
            }

            CurrentTurn = CurrentTurn == Side.Black ? Side.White : Side.Black;
            
            AvailableMoves = GetAvailableMoves();
            
            return move;
        }

        public void Initialize()
        {
            CurrentTurn = Side.White;
            
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        Pieces[x + y * Size] = y switch
                        {
                            < 4 => new BoardPiece(Side.Black, new BoardSquare(x, y)),
                            > 5 => new BoardPiece(Side.White, new BoardSquare(x, y)),
                            _ => null
                        };
                    }
                    else
                    {
                        Pieces[x + y * Size] = null;
                    }
                }
            }

            AvailableMoves = GetAvailableMoves();
        }

        public Board()
        {
            
        }
    }
}