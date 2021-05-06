namespace Draughts.Shared.Models
{
    public class Draughtboard
    {
        public static int Size = 10;
        public Draught[] Draughts { get; } = new Draught[Size * Size];

        public Draughtboard()
        {
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        if (y < 4)
                        {
                            Draughts[x + y * Size] = new Draught(false);
                        }
                        else if (y > 5)
                        {
                            Draughts[x + y * Size] = new Draught(true);
                        }
                        else
                        {
                            Draughts[x + y * Size] = null;
                        }
                    }
                    else
                    {
                        Draughts[x + y * Size] = null;
                    }
                }
            }
        }
    }
}