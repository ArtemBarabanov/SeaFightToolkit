using SeaFightToolkit.ClassicSeaFight.Constants;
using SeaFightToolkit.Common.Enums;
using SeaFightToolkit.Common.Interfaces;
using SeaFightToolkit.Common.Models;

namespace SeaFightToolkit.ClassicSeaFight.Game
{
    /// <summary>
    /// Вспомогательный класс для подготовки игровой сессии
    /// </summary>
    public class SessionPrepareHelper
    {
        private readonly ISeaFightSession _session;
        private List<(int, int)> _tempShip = [];
        private int _currentShipDecks;
        private ShipOrientation _currentShipOrientation;
        private int _oneDeckCount;
        private int _twoDeckCount;
        private int _threeDeckCount;
        private int _fourDeckCount;

        public event Action<List<(int, int)>>? IndifferentPositionEvent;
        public event Action<List<(int, int)>>? GoodPositionEvent;
        public event Action<List<(int, int)>>? BadPositionEvent;
        public event Action<int>? ChangeOneDeckCount;
        public event Action<int>? ChangeTwoDeckCount;
        public event Action<int>? ChangeThreeDeckCount;
        public event Action<int>? ChangeFourDeckCount;
        public event Action<int>? ShipTypeChanged;
        public event Action? ShowStartButton;
        public event Action? HideStartButton;
        public event Action? ArrowVertical;
        public event Action? ArrowHorizontal;
        public event Action<List<(int, int)>>? PlacePlayerShip;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="session">Игровая сессия</param>
        public SessionPrepareHelper(ISeaFightSession session)
        {
            _oneDeckCount = SessionConstants.OneDeckShipsCount;
            _twoDeckCount = SessionConstants.TwoDeckShipsCount;
            _threeDeckCount = SessionConstants.ThreeDeckShipsCount;
            _fourDeckCount = SessionConstants.FourDeckShipsCount;
            _session = session;
        }

        /// <summary>
        /// Обработка выхода курсора мыши из клетки
        /// </summary>
        public void MouseOut()
        {
            if (!IsOccupied(_tempShip, _session.GetPlayerSea()))
            {
                OnIndifferentPositionEvent(_tempShip);
            }
        }

        /// <summary>
        /// Обработка входа курсора мыши в клетку
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public void MouseIn(int x, int y)
        {
            MoveShip(x, y);

            if (!IsWithinField(_tempShip) || IsOccupied(_tempShip, _session.GetPlayerSea()))
            {
                OnIndifferentPositionEvent(_tempShip);
                return;
            }

            if (!IsColliding(_tempShip, _session.GetPlayerSea()))
            {
                OnGoodPositionEvent(_tempShip);
            }
            else
            {
                OnBadPositionEvent(_tempShip);
            }
        }

        /// <summary>
        /// Клик по полю игрока
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public void PlayerFieldClick(int x, int y)
        {
            if (!_session.GetPlayerSea()[x, y].IsOccupied)
            {
                AddPlayerShip();
            }
            else
            {
                RemovePlayerShip(x, y);
            }

            if (_session.GetPlayerShips().Count == 10)
            {
                OnShowStartButtonEvent();
            }
            else
            {
                OnHideStartButtonEvent();
            }
        }

        /// <summary>
        /// Изменение направления корабля
        /// </summary>
        public void ChangeShipOrientation()
        {
            if (_currentShipOrientation == ShipOrientation.Horizontal)
            {
                _currentShipOrientation = ShipOrientation.Vertical;
                OnShipVerticalEvent();
            }
            else
            {
                _currentShipOrientation = ShipOrientation.Horizontal;
                OnShipHorizontalEvent();
            }
        }

        /// <summary>
        /// Выбор типа корабля
        /// </summary>
        /// <param name="decks">Количество палуб</param>
        public void ChangeShipType(int decks)
        {
            _currentShipDecks = decks;
            CreateTempShip(0, 0);
        }

        /// <summary>
        /// Автоматическое размещение кораблей
        /// </summary>
        public void AutoPlayerShips()
        {
            AllShipsDelete(_session.GetPlayerShips());
            AutoShipPosition(_session.GetPlayerShips(), _session.GetPlayerSea());
            foreach (var ship in _session.GetPlayerShips())
            {
                OnPlacePlayerShipEvent(ship.Decks.Select(d => (d.X, d.Y)).ToList());
            }

            _oneDeckCount = 0;
            _twoDeckCount = 0;
            _threeDeckCount = 0;
            _fourDeckCount = 0;
            OnChangeOneDeckCountEvent(_oneDeckCount);
            OnChangeTwoDeckCountEvent(_twoDeckCount);
            OnChangeThreeDeckCountEvent(_threeDeckCount);
            OnChangeFourDeckCountEvent(_fourDeckCount);
            OnShowStartButtonEvent();
        }

        /// <summary>
        /// Автоматическое размещение кораблей оппонента
        /// </summary>
        public void AutoOpponentShips()
        {
            AutoShipPosition(_session.GetOpponentShips(), _session.GetOpponentSea());
        }

        /// <summary>
        /// Обработка перемещения курсора-корабля
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void MoveShip(int x, int y)
        {
            CreateTempShip(x, y);

            if (_currentShipOrientation == ShipOrientation.Horizontal)
            {
                MoveHorizontalShip(x, y);
            }
            else
            {
                MoveVerticalShip(x, y);
            }
        }

        /// <summary>
        /// Обработка перемещения горизонтального курсора-корабля
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void MoveHorizontalShip(int x, int y) 
        {
            for (int i = 0; i < _tempShip.Count; i++)
            {
                _tempShip[i] = (x + i, y);
            }
        }

        /// <summary>
        /// Обработка перемещения вертикального курсора-корабля
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void MoveVerticalShip(int x, int y) 
        {
            for (int i = 0; i < _tempShip.Count; i++)
            {
                _tempShip[i] = (x, y + i);
            }
        }

        /// <summary>
        /// Добавление корабля игрока
        /// </summary>
        private void AddPlayerShip() 
        {
            if (_currentShipDecks == 0 || _tempShip.Count == 0 || IsColliding(_tempShip, _session.GetPlayerSea()))
            {
                return;
            }

            if (_currentShipDecks == 1 && _oneDeckCount > 0)
            {
                OneDeck oneDeck = new OneDeck();
                _session.GetPlayerShips().Add(oneDeck);
                _oneDeckCount--;
                OnChangeOneDeckCountEvent(_oneDeckCount);
                oneDeck.CreateShip(_tempShip, _session.GetPlayerSea());
                OnPlacePlayerShipEvent(_tempShip);
                _tempShip.Clear();
            }
            else if (_currentShipDecks == 2 && _twoDeckCount > 0)
            {
                TwoDeck twoDeck = new TwoDeck(_currentShipOrientation);
                _session.GetPlayerShips().Add(twoDeck);
                _twoDeckCount--;
                OnChangeTwoDeckCountEvent(_twoDeckCount);
                twoDeck.CreateShip(_tempShip, _session.GetPlayerSea());
                OnPlacePlayerShipEvent(_tempShip);
                _tempShip.Clear();
            }
            else if (_currentShipDecks == 3 && _threeDeckCount > 0)
            {
                ThreeDeck threeDeck = new ThreeDeck(_currentShipOrientation);
                _session.GetPlayerShips().Add(threeDeck);
                _threeDeckCount--;
                OnChangeThreeDeckCountEvent(_threeDeckCount);
                threeDeck.CreateShip(_tempShip, _session.GetPlayerSea());
                OnPlacePlayerShipEvent(_tempShip);
                _tempShip.Clear();
            }
            else if (_currentShipDecks == 4 && _fourDeckCount > 0)
            {
                FourDeck fourDeck = new FourDeck(_currentShipOrientation);
                _session.GetPlayerShips().Add(fourDeck);
                _fourDeckCount--;
                OnChangeFourDeckCountEvent(_fourDeckCount);
                fourDeck.CreateShip(_tempShip, _session.GetPlayerSea());
                OnPlacePlayerShipEvent(_tempShip);
                _tempShip.Clear();
            }
        }

        /// <summary>
        /// Удаление корабля игрока
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void RemovePlayerShip(int x, int y)
        {
            var ship = _session.GetPlayerShips()
                .First(ship => ship.Decks
                .Any(deck => deck.X == x && deck.Y == y));

            if (ship.DeckNumber == 1)
            {
                _oneDeckCount++;
                OnChangeOneDeckCountEvent(_oneDeckCount);
            }
            else if (ship.DeckNumber == 2)
            {
                _twoDeckCount++;
                OnChangeTwoDeckCountEvent(_twoDeckCount);
            }
            else if (ship.DeckNumber == 3)
            {
                _threeDeckCount++;
                OnChangeThreeDeckCountEvent(_threeDeckCount);
            }
            else if (ship.DeckNumber == 4)
            {
                _fourDeckCount++;
                OnChangeFourDeckCountEvent(_fourDeckCount);
            }
            _session.GetPlayerShips().Remove(ship);
            ship.DeleteShip(_session.GetPlayerSea());

            OnShipTypeChangedEvent(ship.DeckNumber);
            _tempShip.Clear();
            foreach (var deck in ship.Decks)
            {
                _tempShip.Add((deck.X, deck.Y));
            }
            _currentShipDecks = ship.DeckNumber;
            OnIndifferentPositionEvent(_tempShip);
        }

        /// <summary>
        /// Определение границ поля
        /// </summary>
        /// <param name="tempShip">Курсор-корабль</param>
        private bool IsWithinField(List<(int, int)> tempShip)
        {
            for (int i = 0; i < tempShip.Count; i++)
            {
                if ((tempShip[i].Item1 > 9) 
                    || (tempShip[i].Item1 == -1) 
                    || (tempShip[i].Item2 > 9) 
                    || (tempShip[i].Item2 == -1))
                {
                    tempShip.Clear();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Определение касаний с другими кораблями
        /// </summary>
        /// <param name="tempShip">Курсор-корабль</param>
        /// <param name="sea">Игровое поле</param>
        private bool IsColliding(List<(int, int)> tempShip, SeaCell[,] sea)
        {
            for (int i = 0; i < tempShip.Count; i++)
            {
                int x = tempShip[i].Item1;
                int y = tempShip[i].Item2;

                if ((x < 10) 
                    && (x > -1) 
                    && (y < 10) 
                    && (y > -1)
                    && sea[x, y].BorderCount > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Проверка поля на занятость
        /// </summary>
        /// <param name="tempShip">Курсор-корабль</param>
        /// <param name="sea">Игровое поле</param>
        private bool IsOccupied(List<(int, int)> tempShip, SeaCell[,] sea)
        {
            for (int i = 0; i < tempShip.Count; i++)
            {
                int x = tempShip[i].Item1;
                int y = tempShip[i].Item2;

                if ((x < 10) 
                    && (x > -1) 
                    && (y < 10) 
                    && (y > -1)
                    && sea[x, y].IsOccupied)
                {
                    tempShip.Clear();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Автоматическое размещение кораблей
        /// </summary>
        /// <param name="ships">Корабли</param>
        /// <param name="sea">Игровое пространство</param>
        private void AutoShipPosition(List<Ship> ships, SeaCell[,] sea)
        {
            var random = new Random();
            var orientationValuesCount = Enum.GetValues(typeof(ShipOrientation)).Length;
            ShipOrientation orientation;

            for (int i = 0; i < SessionConstants.FourDeckShipsCount;)
            {
                int X = random.Next(0, 10);
                int Y = random.Next(0, 10);
                orientation = (ShipOrientation)random.Next(orientationValuesCount);
                if (orientation == ShipOrientation.Horizontal)
                {
                    _tempShip = [(X, Y), (X + 1, Y), (X + 2, Y), (X + 3, Y)];
                }
                else
                {
                    _tempShip = [(X, Y), (X, Y + 1), (X, Y + 2), (X, Y + 3)];
                }

                if (IsWithinField(_tempShip) 
                    && !IsOccupied(_tempShip, sea) 
                    && !IsColliding(_tempShip, sea))
                {
                    FourDeck fourDeck = new FourDeck(orientation);
                    fourDeck.CreateShip(_tempShip, sea);
                    ships.Add(fourDeck);
                    i++;
                }
            }
            for (int i = 0; i < SessionConstants.ThreeDeckShipsCount;)
            {
                int X = random.Next(0, 10);
                int Y = random.Next(0, 10);
                orientation = (ShipOrientation)random.Next(orientationValuesCount);
                if (orientation == ShipOrientation.Horizontal)
                {
                    _tempShip = [(X, Y), (X + 1, Y), (X + 2, Y)];
                }
                else
                {
                    _tempShip = [(X, Y), (X, Y + 1), (X, Y + 2)];
                }

                if (IsWithinField(_tempShip) 
                    && !IsOccupied(_tempShip, sea) 
                    && !IsColliding(_tempShip, sea))
                {
                    ThreeDeck threeDeck = new ThreeDeck(orientation);
                    threeDeck.CreateShip(_tempShip, sea);
                    ships.Add(threeDeck);
                    i++;
                }
            }
            for (int i = 0; i < SessionConstants.TwoDeckShipsCount;)
            {
                int X = random.Next(0, 10);
                int Y = random.Next(0, 10);
                orientation = (ShipOrientation)random.Next(orientationValuesCount);
                if (orientation == ShipOrientation.Horizontal)
                {
                    _tempShip = [(X, Y), (X + 1, Y)];
                }
                else
                {
                    _tempShip = [(X, Y), (X, Y + 1)];
                }

                if (IsWithinField(_tempShip) 
                    && !IsOccupied(_tempShip, sea) 
                    && !IsColliding(_tempShip, sea))
                {
                    TwoDeck twoDeck = new TwoDeck(orientation);
                    twoDeck.CreateShip(_tempShip, sea);
                    ships.Add(twoDeck);
                    i++;
                }
            }
            for (int i = 0; i < SessionConstants.OneDeckShipsCount;)
            {
                int X = random.Next(0, 10);
                int Y = random.Next(0, 10);
                _tempShip = [(X, Y)];
                if (IsWithinField(_tempShip) 
                    && !IsOccupied(_tempShip, sea) 
                    && !IsColliding(_tempShip, sea))
                {
                    OneDeck oneDeck = new OneDeck();
                    oneDeck.CreateShip(_tempShip, sea);
                    ships.Add(oneDeck);
                    i++;
                }
            }
        }

        /// <summary>
        /// Временный корабль-указатель
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void CreateTempShip(int x, int y)
        {
            _tempShip.Clear();
            if (_currentShipOrientation == ShipOrientation.Horizontal)
            {
                for (int i = 0; i < _currentShipDecks; i++)
                {
                    _tempShip.Add((x + i, y));
                }
                return;
            }

            for (int i = 0; i < _currentShipDecks; i++)
            {
                _tempShip.Add((x, y + i));
            }
        }

        /// <summary>
        /// Удаление кораблей при автогенерации
        /// </summary>
        /// <param name="ships">Корабли</param>
        private void AllShipsDelete(List<Ship> ships)
        {
            for (int i = 0; i < ships.Count;)
            {
                ships[i].DeleteShip(_session.GetPlayerSea());
                OnIndifferentPositionEvent(ships[i].Decks.Select(d => (d.X, d.Y)).ToList());
                ships.Remove(ships[i]);
            }
        }

        /// <summary>
        /// Удаление одного окрабля при повторном клике по нему
        /// </summary>
        /// <param name="ships">Корабли</param>
        private void ShipDeleteEvent(List<Ship> ships)
        {
            var ship = ships.Find(s => s.IsChosen);

            if (ship != null)
            {
                if (ship.DeckNumber == 1)
                {
                    _oneDeckCount++;
                    OnChangeOneDeckCountEvent(_oneDeckCount);
                }
                else if (ship.DeckNumber == 2)
                {
                    _twoDeckCount++;
                    OnChangeTwoDeckCountEvent(_twoDeckCount);
                }
                else if (ship.DeckNumber == 3)
                {
                    _threeDeckCount++;
                    OnChangeThreeDeckCountEvent(_threeDeckCount);
                }
                else if (ship.DeckNumber == 4)
                {
                    _fourDeckCount++;
                    OnChangeFourDeckCountEvent(_fourDeckCount);
                }
                ships.Remove(ship);
                ship.DeleteShip(_session.GetPlayerSea());
                OnIndifferentPositionEvent(ship.Decks.Select(d => (d.X, d.Y)).ToList());
            }
            OnHideStartButtonEvent();
        }

        protected virtual void OnBadPositionEvent(List<(int, int)> tempShip) 
        {
            var raiseEvent = BadPositionEvent;
            if (raiseEvent != null)
            {
                raiseEvent(tempShip);
            }
        }

        protected virtual void OnGoodPositionEvent(List<(int, int)> tempShip)
        {
            var raiseEvent = GoodPositionEvent;
            if (raiseEvent != null)
            {
                raiseEvent(tempShip);
            }
        }

        protected virtual void OnIndifferentPositionEvent(List<(int, int)> tempShip)
        {
            var raiseEvent = IndifferentPositionEvent;
            if (raiseEvent != null)
            {
                raiseEvent(tempShip);
            }
        }

        protected virtual void OnShowStartButtonEvent()
        {
            var raiseEvent = ShowStartButton;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnHideStartButtonEvent()
        {
            var raiseEvent = HideStartButton;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnShipHorizontalEvent()
        {
            var raiseEvent = ArrowHorizontal;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnShipVerticalEvent()
        {
            var raiseEvent = ArrowVertical;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnPlacePlayerShipEvent(List<(int, int)> tempShip) 
        {
            var raiseEvent = PlacePlayerShip;
            if (raiseEvent != null)
            {
                raiseEvent(tempShip);
            }
        }

        protected virtual void OnChangeOneDeckCountEvent(int deckCount) 
        {
            var raiseEvent = ChangeOneDeckCount;
            if (raiseEvent != null)
            {
                raiseEvent(deckCount);
            }
        }

        protected virtual void OnChangeTwoDeckCountEvent(int deckCount)
        {
            var raiseEvent = ChangeTwoDeckCount;
            if (raiseEvent != null)
            {
                raiseEvent(deckCount);
            }
        }

        protected virtual void OnChangeThreeDeckCountEvent(int deckCount)
        {
            var raiseEvent = ChangeThreeDeckCount;
            if (raiseEvent != null)
            {
                raiseEvent(deckCount);
            }
        }

        protected virtual void OnChangeFourDeckCountEvent(int deckCount)
        {
            var raiseEvent = ChangeFourDeckCount;
            if (raiseEvent != null)
            {
                raiseEvent(deckCount);
            }
        }

        protected virtual void OnShipTypeChangedEvent(int deckCount) 
        {
            var raiseEvent = ShipTypeChanged;
            if (raiseEvent != null)
            {
                raiseEvent(deckCount);
            }
        }
    }   
}
