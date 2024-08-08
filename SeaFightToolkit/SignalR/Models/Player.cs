using SeaFightToolkit.ClassicSeaFight.Constants;
using SeaFightToolkit.Common.Models;
using SeaFightToolkit.SignalR.Dtos;

namespace SeaFightToolkit.SignalR.Models
{
    /// <summary>
    /// Модель игрока
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Признак занятости
        /// </summary>
        public bool IsBusy { get; set; }

        /// <summary>
        /// Поле игрока
        /// </summary>
        public SeaCell[,] PlayerSea { get; set; } = default!;

        /// <summary>
        /// Корабли игрока
        /// </summary>
        public List<ShipDto> PlayerShips { get; set; } = default!;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Имя</param>
        public Player(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Игрок принимает участие в игре
        /// </summary>
        public void EnterGame()
        {
            PlayerSea = new SeaCell[FieldConstants.FieldWidth, FieldConstants.FieldHeight];
            PlayerShips = [];
            CreateEmptySea();
        }

        /// <summary>
        /// Игрок покидает игру
        /// </summary>
        public void QuitGame()
        {
            IsBusy = false;
        }

        /// <summary>
        /// Соднаие пустого игрового поля
        /// </summary>
        private void CreateEmptySea()
        {
            for (int i = 0; i < FieldConstants.FieldWidth; i++)
            {
                for (int j = 0; j < FieldConstants.FieldHeight; j++)
                {
                    PlayerSea![i, j] = new SeaCell(i, j);
                }
            }
        }
    }
}
