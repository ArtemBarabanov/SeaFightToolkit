using SeaFightToolkit.Common.Models;

namespace SeaFightToolkit.ClassicSeaFight.AI
{
    /// <summary>
    /// Оппонент-компьютер
    /// </summary>
    public class Computer
    {
        private readonly Strategy _strategy;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="strategy">Стратегия игры</param>
        public Computer(Strategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Ход оппонента
        /// </summary>
        /// <param name="sea">Игровое поле</param>
        /// <param name="ships">Корабли</param>
        /// <returns>Координаты выбранной для хода клетки</returns>
        public async Task<(int, int)> OpponentMoveAsync(SeaCell[,] sea, List<Ship> ships) 
        {
            await Task.Delay(5000);
            return _strategy.OpponentMove(sea, ships);
        }
    }
}
