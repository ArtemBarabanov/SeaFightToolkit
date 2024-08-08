using SeaFightToolkit.Common.Enums;

namespace SeaFightToolkit.Common.Models
{
    public class FourDeck : Ship
    {
        public FourDeck(ShipOrientation orientation = ShipOrientation.Horizontal)
        {
            this.orientation = orientation;
            DeckNumber = 4;
        }
    }
}
