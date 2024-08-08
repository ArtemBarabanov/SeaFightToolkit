namespace SeaFightToolkit.Common.Models
{
    public class SeaCell
    {
        public int X { get; }

        public int Y { get; }

        public int BorderCount { get; set; }

        public bool IsVisited { get; set; }

        public bool IsOccupied { get; set; }

        public SeaCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
