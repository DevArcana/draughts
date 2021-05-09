namespace Draughts.Shared.Models
{
    public class Board
    {
        public static int Size = 10;
        public BoardPiece[] Pieces { get; } = new BoardPiece[Size * Size];

        public Board()
        {
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
    }
}