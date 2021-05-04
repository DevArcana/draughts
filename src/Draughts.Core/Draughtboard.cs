using System.Collections.Generic;

namespace Draughts.Core
{
    public class Draughtboard
    {
        public List<Draught> Draughts { get; }

        private void PopulateInitialDraughts()
        {
            for (var i = 0; i < 3; i++)
            {
                for (var j = (i + 1) % 2; j < 8; j += 2)
                {
                    Draughts.Add(new Draught(j, i, true));
                    Draughts.Add(new Draught(7-j, 7-i, false));
                }
            }
        }

        public Draughtboard()
        {
            Draughts = new List<Draught>(24);
            
            PopulateInitialDraughts();
        }
    }
}