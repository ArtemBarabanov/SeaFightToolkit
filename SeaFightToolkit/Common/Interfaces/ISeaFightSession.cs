using SeaFightToolkit.Common.Models;

namespace SeaFightToolkit.Common.Interfaces
{
    /// <summary>
    /// Интерфейс игровой сессии
    /// </summary>
    public interface ISeaFightSession
    {
        /// <summary>
        /// Возвращает корабли игрока
        /// </summary>
        /// <returns>Корабли игрока</returns>
        public List<Ship> GetPlayerShips();

        /// <summary>
        /// Возвращает корабли оппонента
        /// </summary>
        /// <returns>Корабли оппонента</returns>
        public List<Ship> GetOpponentShips();

        /// <summary>
        /// Возвращает поле игрока
        /// </summary>
        /// <returns>Поле игрока</returns>
        public SeaCell[,] GetPlayerSea();

        /// <summary>
        /// Возвращает поле оппонента
        /// </summary>
        /// <returns>Поле оппонента</returns>
        public SeaCell[,] GetOpponentSea();

        /// <summary>
        /// Старт игры
        /// </summary>
        public void StartGame();

        /// <summary>
        /// Проверка на уничтожение кораблей и победу в конце хода
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public void CompletingTurn(int x, int y);

        /// <summary>
        /// Ход
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public void Turn(int x, int y);
    }
}
