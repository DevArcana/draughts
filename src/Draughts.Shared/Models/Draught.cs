namespace Draughts.Shared.Models
{
    public class Draught
    {
        public bool IsWhite { get; }

        public Draught(bool white)
        {
            IsWhite = white;
        }
    }
}