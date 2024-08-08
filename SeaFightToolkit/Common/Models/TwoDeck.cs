using SeaFightToolkit.Common.Enums;

namespace SeaFightToolkit.Common.Models
{
    public class TwoDeck : Ship
    {
        public TwoDeck(ShipOrientation orientation = ShipOrientation.Horizontal)
        {
            this.orientation = orientation;
            DeckNumber = 2;
        }
    }
}
