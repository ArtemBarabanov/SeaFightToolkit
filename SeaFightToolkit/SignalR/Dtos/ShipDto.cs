using SeaFightToolkit.Common.Models;

namespace SeaFightToolkit.SignalR.Dtos
{
    /// <summary>
    /// Корабль
    /// </summary>
    public class ShipDto
    {
        /// <summary>
        /// Количество палуб
        /// </summary>
        public int DeckNumber { get; set; }

        /// <summary>
        /// Палубы
        /// </summary>

        public List<Deck> Decks = [];

        /// <summary>
        /// Уничтожен ли корабль
        /// </summary>
        public bool IsDestroyed { get; set; }

        /// <summary>
        /// Маппинг модели Ship в ShipDto
        /// </summary>
        /// <param name="ship">Корабль (Ship)</param>
        /// <returns>ShipDto</returns>
        public static ShipDto MapShipToShipDto(Ship ship) 
        {
            return new ShipDto() 
            {
                DeckNumber = ship.DeckNumber,
                Decks = ship.Decks,
                IsDestroyed = ship.IsDestroyed
            };
        }
    }
}
