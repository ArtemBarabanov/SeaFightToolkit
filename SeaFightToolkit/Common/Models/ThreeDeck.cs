using SeaFightToolkit.Common.Enums;

namespace SeaFightToolkit.Common.Models
{
    public class ThreeDeck : Ship
    {
        public ThreeDeck(ShipOrientation orientation = ShipOrientation.Horizontal)
        {
            this.orientation = orientation;
            DeckNumber = 3;
        }
    }
}
