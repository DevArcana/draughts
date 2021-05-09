using System;
using System.Collections.Generic;
using System.Linq;

namespace Draughts.Shared.Models
{
    public class Board
    {
        public static int Size = 10;
        public BoardPiece[] Pieces { get; set; } = new BoardPiece[Size * Size];
        public List<Move> History { get; set; } = new List<Move>();
        
        public Side CurrentTurn { get; set; }

        private int PosIndex(int x, int y) => x + y * Size;

        private BoardPiece GetPiece(BoardSquare square) => GetPiece(square.X, square.Y);
        
        private BoardPiece GetPiece(int x, int y)
        {
            var pos = PosIndex(x, y);
            
            if (pos < 0 || pos >= Pieces.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            return Pieces[pos];
        }

        private void SetPiece(BoardSquare square, BoardPiece piece) => SetPiece(square.X, square.Y, piece);

        private void SetPiece(int x, int y, BoardPiece piece)
        {
            Pieces[PosIndex(x, y)] = piece;
        }
        
        public AvailableMove[] GetAvailableMovesFor(BoardPiece piece)
        {
            if (piece.Side != CurrentTurn)
            {
                return Array.Empty<AvailableMove>();
            }
            
            var (x, y) = piece.Pos;
            
            var legalMoves = new List<AvailableMove>(4);

            if (piece.Side == Side.White || piece.Promoted)
            {
                var upperLeft = GetPiece(x - 1, y - 1);
                var upperRight = GetPiece(x + 1, y - 1);

                if (upperLeft is null)
                {
                    legalMoves.Add(new AvailableMove(new BoardSquare(x - 1, y - 1), null));
                }
                else if (upperLeft.Side != piece.Side)
                {
                    if (GetPiece(x - 2, y - 2) is null)
                    {
                        legalMoves.Add(new AvailableMove(new BoardSquare(x - 2, y - 2), upperLeft));
                    }
                }
            
                if (upperRight is null)
                {
                    legalMoves.Add(new AvailableMove(new BoardSquare(x + 1, y - 1), null));
                }
                else if (upperRight.Side != piece.Side)
                {
                    if (GetPiece(x + 2, y - 2) is null)
                    {
                        legalMoves.Add(new AvailableMove(new BoardSquare(x + 2, y - 2), upperRight));
                    }
                }
            }
            
            if (piece.Side == Side.Black || piece.Promoted)
            {
                var lowerLeft = GetPiece(x - 1, y + 1);
                var lowerRight = GetPiece(x + 1, y + 1);

                if (lowerLeft is null)
                {
                    legalMoves.Add(new AvailableMove(new BoardSquare(x - 1, y + 1), null));
                }
                else if (lowerLeft.Side != piece.Side)
                {
                    if (GetPiece(x - 2, y + 2) is null)
                    {
                        legalMoves.Add(new AvailableMove(new BoardSquare(x - 2, y + 2), lowerLeft));
                    }
                }
            
                if (lowerRight is null)
                {
                    legalMoves.Add(new AvailableMove(new BoardSquare(x + 1, y + 1), null));
                }
                else if (lowerRight.Side != piece.Side)
                {
                    if (GetPiece(x + 2, y + 2) is null)
                    {
                        legalMoves.Add(new AvailableMove(new BoardSquare(x + 2, y + 2), lowerRight));
                    }
                }
            }

            return legalMoves.ToArray();
        }

        public Move MakeMove(BoardSquare from, BoardSquare to)
        {
            var piece = GetPiece(from);

            if (piece is null)
            {
                return null;
            }

            var availableMove = GetAvailableMovesFor(piece).FirstOrDefault(m => m.Pos == to);

            if (availableMove is null)
            {
                return null;
            }

            var move = new Move(piece, availableMove);
            
            History.Add(move);

            SetPiece(from, null);

            var promote = (piece.Side == Side.Black && to.Y == Size - 1) || (piece.Side == Side.White && to.Y == 0);
            SetPiece(to, piece.Move(to, promote));

            if (availableMove.Takes is not null)
            {
                SetPiece(availableMove.Takes.Pos, null);
            }

            CurrentTurn = CurrentTurn == Side.Black ? Side.White : Side.Black;
            
            return move;
        }

        public void Initialize()
        {
            CurrentTurn = Side.Black;
            
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
        }

        public Board()
        {
            
        }
    }
}