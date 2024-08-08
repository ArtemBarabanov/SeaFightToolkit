namespace SeaFightToolkit.ClassicSeaFight.AI.Models
{
    /// <summary>
    /// Предыдущий ход
    /// </summary>
    public class PreviousMove
    {
        /// <summary>
        /// Координата X
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Координата Y
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public PreviousMove(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
