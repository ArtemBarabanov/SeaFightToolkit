using SeaFightToolkit.Common.Models;

namespace SeaFightToolkit.ClassicSeaFight.AI
{
    /// <summary>
    /// Стратегия игры
    /// </summary>
    public abstract class Strategy
    {
        /// <summary>
        /// Ход оппонента
        /// </summary>
        /// <param name="sea">Игровое поле</param>
        /// <param name="ships">Корабли</param>
        /// <returns></returns>
        public abstract (int, int) OpponentMove(SeaCell[,] sea, List<Ship> ships);
    }
}
