using SeaFightToolkit.Common.Enums;

namespace SeaFightToolkit.Common.Models
{
    public abstract class Ship
    {
        public int DeckNumber { get; protected set; }

        public List<Deck> Decks = new List<Deck>();

        public bool IsChosen { get; set; }

        public bool IsDestroyed { get; set; }

        protected ShipOrientation orientation;

        public void CreateShip(List<(int, int)> coordinates, SeaCell[,] sea)
        {
            if (orientation == ShipOrientation.Horizontal)
            {
                for (int i = 0; i < coordinates.Count; i++)
                {
                    int x = coordinates[i].Item1;
                    int y = coordinates[i].Item2;

                    sea[x, y].IsOccupied = true;

                    if (x - 1 >= 0)
                    {
                        sea[x - 1, y].BorderCount += 1;
                    }
                    if (x + 1 < 10 && y + 1 < 10)
                    {
                        sea[x + 1, y + 1].BorderCount += 1;
                    }
                    if (x - 1 >= 0 && y + 1 < 10)
                    {
                        sea[x - 1, y + 1].BorderCount += 1;
                    }
                    if (x + 1 < 10 && y - 1 >= 0)
                    {
                        sea[x + 1, y - 1].BorderCount += 1;
                    }
                    if (x - 1 >= 0 && y - 1 >= 0)
                    {
                        sea[x - 1, y - 1].BorderCount += 1;
                    }
                    if (y + 1 < 10)
                    {
                        sea[x, y + 1].BorderCount += 1;
                    }
                    if (y - 1 >= 0)
                    {
                        sea[x, y - 1].BorderCount += 1;
                    }
                    if (i == coordinates.Count - 1 && x + 1 < 10)
                    {
                        sea[x + 1, y].BorderCount += 1;
                    }

                    var deck = new Deck() { X = x, Y = y };
                    Decks.Add(deck);
                }
            }
            else if (orientation == ShipOrientation.Vertical)
            {
                for (int i = 0; i < DeckNumber; i++)
                {
                    int x = coordinates[i].Item1;
                    int y = coordinates[i].Item2;

                    sea[x, y].IsOccupied = true;

                    if (x - 1 >= 0)
                    {
                        sea[x - 1, y].BorderCount += 1;
                    }
                    if (x + 1 < 10 && y + 1 < 10)
                    {
                        sea[x + 1, y + 1].BorderCount += 1;
                    }
                    if (x - 1 >= 0 && y + 1 < 10)
                    {
                        sea[x - 1, y + 1].BorderCount += 1;
                    }
                    if (x + 1 < 10 && y - 1 >= 0)
                    {
                        sea[x + 1, y - 1].BorderCount += 1;
                    }
                    if (x - 1 >= 0 && y - 1 >= 0)
                    {
                        sea[x - 1, y - 1].BorderCount += 1;
                    }
                    if (x + 1 < 10)
                    {
                        sea[x + 1, y].BorderCount += 1;
                    }
                    if (y - 1 >= 0)
                    {
                        sea[x, y - 1].BorderCount += 1;
                    }
                    if (i == coordinates.Count - 1 && y + 1 < 10)
                    {
                        sea[x, y + 1].BorderCount += 1;
                    }

                    var deck = new Deck() { X = x, Y = y };
                    Decks.Add(deck);
                }
            }
        }
        public void DeleteShip(SeaCell[,] sea)
        {
            if (orientation == ShipOrientation.Horizontal)
            {
                foreach (Deck deck in Decks)
                {
                    sea[deck.X, deck.Y].IsOccupied = false;

                    if (deck.X + 1 < 10)
                    {
                        sea[deck.X + 1, deck.Y].IsOccupied = false;
                    }
                    if (deck.X - 1 >= 0)
                    {
                        sea[deck.X - 1, deck.Y].BorderCount -= 1;
                    }
                    if (deck.X + 1 < 10 && deck.Y + 1 < 10)
                    {
                        sea[deck.X + 1, deck.Y + 1].BorderCount -= 1;
                    }
                    if (deck.X - 1 >= 0 && deck.Y + 1 < 10)
                    {
                        sea[deck.X - 1, deck.Y + 1].BorderCount -= 1;
                    }
                    if (deck.X + 1 < 10 && deck.Y - 1 >= 0)
                    {
                        sea[deck.X + 1, deck.Y - 1].BorderCount -= 1;
                    }
                    if (deck.X - 1 >= 0 && deck.Y - 1 >= 0)
                    {
                        sea[deck.X - 1, deck.Y - 1].BorderCount -= 1;
                    }
                    if (deck.Y + 1 < 10)
                    {
                        sea[deck.X, deck.Y + 1].BorderCount -= 1;
                    }
                    if (deck.Y - 1 >= 0)
                    {
                        sea[deck.X, deck.Y - 1].BorderCount -= 1;
                    }
                    if (deck == Decks[Decks.Count - 1] && deck.X + 1 < 10)
                    {
                        sea[deck.X + 1, deck.Y].BorderCount -= 1;
                    }
                }
            }
            else if (orientation == ShipOrientation.Vertical)
            {
                foreach (Deck deck in Decks)
                {
                    sea[deck.X, deck.Y].IsOccupied = false;
                    if (deck.Y + 1 < 10)
                    {
                        sea[deck.X, deck.Y + 1].IsOccupied = false;
                    }
                    if (deck.X - 1 >= 0)
                    {
                        sea[deck.X - 1, deck.Y].BorderCount -= 1;
                    }
                    if (deck.X + 1 < 10 && deck.Y + 1 < 10)
                    {
                        sea[deck.X + 1, deck.Y + 1].BorderCount -= 1;
                    }
                    if (deck.X - 1 >= 0 && deck.Y + 1 < 10)
                    {
                        sea[deck.X - 1, deck.Y + 1].BorderCount -= 1;
                    }
                    if (deck.X + 1 < 10 && deck.Y - 1 >= 0)
                    {
                        sea[deck.X + 1, deck.Y - 1].BorderCount -= 1;
                    }
                    if (deck.X - 1 >= 0 && deck.Y - 1 >= 0)
                    {
                        sea[deck.X - 1, deck.Y - 1].BorderCount -= 1;
                    }
                    if (deck.X + 1 < 10)
                    {
                        sea[deck.X + 1, deck.Y].BorderCount -= 1;
                    }
                    if (deck.Y - 1 >= 0)
                    {
                        sea[deck.X, deck.Y - 1].BorderCount -= 1;
                    }
                    if (deck == Decks[Decks.Count - 1] && deck.Y + 1 < 10)
                    {
                        sea[deck.X, deck.Y + 1].BorderCount -= 1;
                    }
                }
            }
        }
    }
}
