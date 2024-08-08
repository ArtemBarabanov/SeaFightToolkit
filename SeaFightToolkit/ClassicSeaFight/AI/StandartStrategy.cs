using SeaFightToolkit.ClassicSeaFight.AI.Enums;
using SeaFightToolkit.ClassicSeaFight.AI.Models;
using SeaFightToolkit.ClassicSeaFight.Constants;
using SeaFightToolkit.Common.Models;

namespace SeaFightToolkit.ClassicSeaFight.AI
{
    /// <summary>
    /// Стратегия игры
    /// 1. Если нет данных о раненном корабле, делается случайный ход в ту клетку, по которой еще не делался выстрел;
    /// 2. Если есть данные о раненном корабле, делаются ходы для определения направления корабля и его уничтожения:
    /// - 2.1 Делается выстрел вверх от координат ранения, если корабль повторно ранен, то направление определяется, как вертикальное, делаются последующие выстрелы для его уничтожения;
    /// - 2.2 Делается выстрел вправо от координат ранения, если корабль повторно ранен, то направление определяется, как горизонтальное, делаются последующие выстрелы для его уничтожения;
    /// - 2.3 Делается выстрел вниз от координат ранения, если корабль повторно ранен, то направление определяется, как вертикальное, делаются последующие выстрелы для его уничтожения;
    /// - 2.4 Делается выстрел влево от координат ранения, если корабль повторно ранен, то направление определяется, как горизонтальное, делаются последующие выстрелы для его уничтожения;
    /// 3. Если корабль уничтожен, делается случайный ход;
    /// </summary>
    public class StandartStrategy : Strategy
    {
        private int _currentMoveX;
        private int _currentMoveY;
        private bool _isHaunting;
        private List<Deck> _enemyShip = [];
        private PreviousMove? _previousMove;

        /// <summary>
        /// Ход компьютерного оппонента
        /// </summary>
        /// <param name="sea">Игровое поле</param>
        /// <param name="myShips">Корабли</param>
        /// <returns>Координаты хода</returns>
        public override (int, int) OpponentMove(SeaCell[,] sea, List<Ship> myShips)
        {
            if (!_isHaunting)
            {
                return MakeRandomShot(sea, myShips);
            }

            var direction = NextMoveDirection.Up;
            if (_previousMove != null)
            {
                _currentMoveX = _previousMove.X;
                _currentMoveY = _previousMove.Y;
            }

            if (direction == NextMoveDirection.Up)
            {
                if (_currentMoveY > 0 && !sea[_currentMoveX, _currentMoveY - 1].IsVisited)
                {
                    _currentMoveY--;
                }
                else
                {
                    direction = NextMoveDirection.Right;
                }
                if (_currentMoveY == 9)
                {
                    direction = NextMoveDirection.Right;
                }
            }
            if (direction == NextMoveDirection.Right)
            {
                if (_currentMoveX < 9 && !sea[_currentMoveX + 1, _currentMoveY].IsVisited)
                {
                    _currentMoveX++;
                }
                else
                {
                    direction = NextMoveDirection.Down;
                }
                if (_currentMoveX == 9)
                {
                    direction = NextMoveDirection.Down;
                }
            }
            if (direction == NextMoveDirection.Down)
            {
                if (_currentMoveY < 9 && !sea[_currentMoveX, _currentMoveY + 1].IsVisited)
                {
                    _currentMoveY++;
                }
                else
                {
                    direction = NextMoveDirection.Left;
                }
                if (_currentMoveY == 0)
                {
                    direction = NextMoveDirection.Left;
                }
            }
            if (direction == NextMoveDirection.Left)
            {
                if (_currentMoveX > 0 && !sea[_currentMoveX - 1, _currentMoveY].IsVisited)
                {
                    _currentMoveX--;
                }
            }

            sea[_currentMoveX, _currentMoveY].IsVisited = true;
            if (sea[_currentMoveX, _currentMoveY].IsOccupied)
            {
                ShipProcessing(_currentMoveX, _currentMoveY, sea, myShips);
                _previousMove = new PreviousMove(_currentMoveX, _currentMoveY);
            }

            if (_enemyShip.Count >= 2)
            {
                if (_enemyShip[0].X == _enemyShip[1].X)
                {
                    foreach (Deck deck in _enemyShip)
                    {
                        if (deck.Y + 1 < FieldConstants.FieldHeight && !(sea[deck.X, deck.Y + 1].IsVisited))
                        {
                            _previousMove = new PreviousMove(deck.X, deck.Y);
                        }
                        if (deck.Y - 1 >= 0 && !(sea[deck.X, deck.Y - 1].IsVisited))
                        {
                            _previousMove = new PreviousMove(deck.X, deck.Y);
                        }
                    }
                }
                if (_enemyShip[0].Y == _enemyShip[1].Y)
                {
                    foreach (Deck deck in _enemyShip)
                    {
                        if (deck.X + 1 < FieldConstants.FieldWidth && !(sea[deck.X + 1, deck.Y].IsVisited))
                        {
                            _previousMove = new PreviousMove(deck.X, deck.Y);
                        }
                        if (deck.X - 1 >= 0 && !(sea[deck.X - 1, deck.Y].IsVisited))
                        {
                            _previousMove = new PreviousMove(deck.X, deck.Y);
                        }
                    }
                }
            }

            return (_currentMoveX, _currentMoveY);
        }

        /// <summary>
        /// Производит случайный выстрел
        /// </summary>
        /// <param name="sea">Игровое поле</param>
        /// <param name="myShips">Корабли</param>
        /// <returns>Координаты хода</returns>
        private (int, int) MakeRandomShot(SeaCell[,] sea, List<Ship> myShips) 
        {
            var notVisitedCells = sea.Cast<SeaCell>()
               .Where(cell => cell.IsVisited == false)
               .ToList();
            var randomPosition = new Random().Next(notVisitedCells.Count);
            var randomCell = notVisitedCells[randomPosition];
            _currentMoveX = randomCell.X;
            _currentMoveY = randomCell.Y;

            if (sea[_currentMoveX, _currentMoveY].IsOccupied)
            {
                _previousMove = new PreviousMove(_currentMoveX, _currentMoveY);
                sea[_currentMoveX, _currentMoveY].IsVisited = true;
                ShipProcessing(_currentMoveX, _currentMoveY, sea, myShips);
            }
            else
            {
                _previousMove = null;
                sea[_currentMoveX, _currentMoveY].IsVisited = true;
            }

            return (_currentMoveX, _currentMoveY);
        }

        /// <summary>
        /// Обработка данных корабля
        /// </summary>
        /// <param name="compX">Предыдущая координата X</param>
        /// <param name="compY">Предыдущая координата Y</param>
        /// <param name="sea">Игровое поле</param>
        /// <param name="myShips">Корабли</param>
        private void ShipProcessing(int compX, int compY, SeaCell[,] sea, List<Ship> myShips)
        {
            var damagedShip = myShips.First(ship => ship.Decks.Any(deck => deck.X == compX && deck.Y == compY));
            var damagedDeck = damagedShip.Decks.First(deck => deck.X == compX && deck.Y == compY);

            damagedDeck.IsDamaged = true;
            sea[compX, compY].IsVisited = true;
            if (compX + 1 < FieldConstants.FieldWidth && compY + 1 < FieldConstants.FieldHeight)
            {
                sea[compX + 1, compY + 1].IsVisited = true;
            }
            if (compX + 1 < FieldConstants.FieldWidth && compY - 1 >= 0)
            {
                sea[compX + 1, compY - 1].IsVisited = true;
            }
            if (compX - 1 >= 0 && compY + 1 < FieldConstants.FieldHeight)
            {
                sea[compX - 1, compY + 1].IsVisited = true;
            }
            if (compX - 1 >= 0 && compY - 1 >= 0)
            {
                sea[compX - 1, compY - 1].IsVisited = true;
            }
            _enemyShip.Add(new Deck() { X = compX, Y = compY });
            _isHaunting = true;

            if (damagedShip.Decks.Count(deck => deck.IsDamaged) == damagedShip.DeckNumber)
            {
                DestroyedShipProcessing(damagedShip, sea);
            }
        }

        /// <summary>
        /// Обработка состояния уничтоженного корабля
        /// </summary>
        /// <param name="ship">Корабль</param>
        /// <param name="sea">Игровое поле</param>
        public void DestroyedShipProcessing(Ship ship, SeaCell[,] sea) 
        {
            ship.IsDestroyed = true;

            foreach (var deck in ship.Decks)
            {
                if (deck.Y + 1 < FieldConstants.FieldHeight && !sea[deck.X, deck.Y + 1].IsOccupied)
                {
                    sea[deck.X, deck.Y + 1].IsVisited = true;
                }
                if (deck.Y - 1 >= 0 && !sea[deck.X, deck.Y - 1].IsOccupied)
                {
                    sea[deck.X, deck.Y - 1].IsVisited = true;
                }
                if (deck.X - 1 >= 0 && !sea[deck.X - 1, deck.Y].IsOccupied)
                {
                    sea[deck.X - 1, deck.Y].IsVisited = true;
                }
                if (deck.X + 1 < FieldConstants.FieldWidth && !sea[deck.X + 1, deck.Y].IsOccupied)
                {
                    sea[deck.X + 1, deck.Y].IsVisited = true;
                }
            }
            _enemyShip.Clear();
            _previousMove = null;
            _isHaunting = false;
        }
    }
}
