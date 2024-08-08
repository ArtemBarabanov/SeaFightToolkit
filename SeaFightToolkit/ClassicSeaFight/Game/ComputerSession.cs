using SeaFightToolkit.ClassicSeaFight.AI;
using SeaFightToolkit.Common.Models;
using SeaFightToolkit.Common.Enums;
using SeaFightToolkit.ClassicSeaFight.Constants;
using SeaFightToolkit.Common.Interfaces;

namespace SeaFightToolkit.ClassicSeaFight.Game
{
    /// <summary>
    /// Игровая сессия при игре с компьютером
    /// </summary>
    public class ComputerSession : ISeaFightSession
    {
        private SeaCell[,] _playerSea = default!;
        private SeaCell[,] _opponentSea = default!;
        private readonly Computer _opponent;
        private readonly List<Ship> _playerShips = [];
        private readonly List<Ship> _opponentShips = [];
        private bool _isOpponentTurn;
        private bool _isVictory;

        public event Action<List<(int, int)>>? OpponentShipDestroyed;
        public event Action<List<(int, int)>>? PlayerShipDestroyed;
        public event Action<(int, int)>? PlayerShipHit;
        public event Action<(int, int)>? OpponentShipHit;
        public event Action<(int, int)>? PlayerShipMiss;
        public event Action<(int, int)>? OpponentShipMiss;
        public event Action<Participants>? VictoryHappened;
        public event Action<int, int>? DecreaseOpponentShipCount;
        public event Action<int, int>? DecreasePlayerShipCount;
        public event Action<Participants>? FirstChosen;
        public event Action? MarkMyTurnEvent;
        public event Action? MarkOpponentTurnEvent;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="opponent">Оппонент-компьютер</param>
        public ComputerSession(Computer opponent)
        {
            CreateOcean();
            _opponent = opponent;
        }

        /// <summary>
        /// Возвращает корабли игрока
        /// </summary>
        /// <returns>Корабли игрока</returns>
        public List<Ship> GetPlayerShips()
        {
            return _playerShips;
        }

        /// <summary>
        /// Возвращает корабли оппонента
        /// </summary>
        /// <returns>Корабли оппонента</returns>
        public List<Ship> GetOpponentShips()
        {
            return _opponentShips;
        }

        /// <summary>
        /// Возвращает поле игрока
        /// </summary>
        /// <returns>Поле игрока</returns>
        public SeaCell[,] GetPlayerSea()
        {
            return _playerSea;
        }

        /// <summary>
        /// Возвращает поле оппонента
        /// </summary>
        /// <returns>Поле оппонента</returns>
        public SeaCell[,] GetOpponentSea()
        {
            return _opponentSea;
        }

        /// <summary>
        /// Старт игры
        /// </summary>
        public async void StartGame()
        {
            var whoIsFirst = WhoIsFirst();
            OnFirstChosen(whoIsFirst);

            if (whoIsFirst == Participants.Opponent)
            {
                _isOpponentTurn = true;
                OnMarkOpponentTurnEvent();
                await CompTurnAsync();
            }
            else
            {
                OnMarkMyTurnEvent();
            }
        }

        /// <summary>
        /// Проверка на уничтожение кораблей и победу в конце хода
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public void CompletingTurn(int x, int y)
        {
            if (_isOpponentTurn)
            {
                CheckForPlayerShipDestroyed(x, y);
            }
            else
            {
                CheckForOpponentShipDestroyed(x, y);
            }
        }

        /// <summary>
        /// Ход
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public async void Turn(int x, int y)
        {
            if (!_isOpponentTurn
                && !_isVictory
                && !_opponentSea[x, y].IsVisited)
            {
                HumanTurn(x, y);
                if (_isOpponentTurn && !_isVictory)
                {
                    await CompTurnAsync();
                }
            }
        }

        /// <summary>
        /// Создание полей игрока и противника
        /// </summary>
        private void CreateOcean()
        {
            var width = FieldConstants.FieldWidth;
            var height = FieldConstants.FieldHeight;
            _playerSea = new SeaCell[width, height];
            _opponentSea = new SeaCell[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _playerSea[i, j] = new SeaCell(i, j);
                    _opponentSea[i, j] = new SeaCell(i, j);
                }
            }
        }

        /// <summary>
        /// Определяет, кто ходит первым
        /// </summary>
        private Participants WhoIsFirst()
        {
            var values = Enum.GetValues(typeof(Participants));
            return (Participants)new Random().Next(values.Length);
        }

        /// <summary>
        /// Проверка на уничтожение корабля противника
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void CheckForOpponentShipDestroyed(int x, int y)
        {
            var ship = _opponentShips.FirstOrDefault(ship => ship.Decks.Any(deck => deck.X == x && deck.Y == y));
            if (ship != null && ship.Decks.Count(d => d.IsDamaged) == ship.DeckNumber)
            {
                ship.IsDestroyed = true;
                OnOpponentShipDestroyed((from d in ship.Decks select (d.X, d.Y)).ToList());
                CheckVictory();
                if (!_isVictory)
                {
                    OnDecreaseOpponentShipCount(ship.DeckNumber, _opponentShips.Count(s => s.DeckNumber == ship.DeckNumber && !s.IsDestroyed));
                }
            }
        }

        /// <summary>
        /// Проверка на уничтожение корабля игрока
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void CheckForPlayerShipDestroyed(int x, int y)
        {
            var ship = _playerShips.FirstOrDefault(ship => ship.Decks.Any(deck => deck.X == x && deck.Y == y));
            if (ship != null && ship.Decks.Count(d => d.IsDamaged) == ship.DeckNumber)
            {
                ship.IsDestroyed = true;
                OnPlayerShipDestroyed((from d in ship.Decks select (d.X, d.Y)).ToList());

                if (!_isVictory)
                {
                    OnDecreasePlayerShipCount(ship.DeckNumber, _playerShips.Count(s => s.DeckNumber == ship.DeckNumber && !s.IsDestroyed));
                }
            };
        }

        /// <summary>
        /// Ход компьютера
        /// </summary>
        private async Task CompTurnAsync()
        {
            while (_isOpponentTurn && !_isVictory)
            {
                (int x, int y) = await _opponent.OpponentMoveAsync(_playerSea, _playerShips);

                if (_playerSea[x, y].IsOccupied)
                {
                    OnOpponentShipHit((x, y));
                    CheckVictory();
                }
                else
                {
                    OnOpponentShipMiss((x, y));
                    _isOpponentTurn = false;
                    OnMarkMyTurnEvent();
                }
            }
        }

        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void HumanTurn(int x, int y)
        {
            if (_opponentSea[x, y].IsOccupied)
            {
                OnPlayerShipHit((x, y));
                _opponentShips.SelectMany(ship => ship.Decks).First(deck => deck.X == x && deck.Y == y).IsDamaged = true;
            }
            else
            {
                OnPlayerShipMiss((x, y));
                _isOpponentTurn = true;
                OnMarkOpponentTurnEvent();
            }
            _opponentSea[x, y].IsVisited = true;
        }

        /// <summary>
        /// Проверка на победу
        /// </summary>
        private void CheckVictory()
        {
            var goodPlayerShips = _playerShips.FindAll(s => !s.IsDestroyed).Count;
            var goodCompShips = _opponentShips.FindAll(s => !s.IsDestroyed).Count;

            if (goodCompShips == 0)
            {
                _isVictory = true;
                OnVictoryHappened(Participants.Human);
            }

            if (goodPlayerShips == 0)
            {
                _isVictory = true;
                OnVictoryHappened(Participants.Opponent);
            }
        }

        protected virtual void OnVictoryHappened(Participants participant)
        {
            var raiseEvent = VictoryHappened;
            if (raiseEvent != null)
            {
                raiseEvent(participant);
            }
        }

        protected virtual void OnPlayerShipHit((int, int) coordinates) 
        {
            var raiseEvent = PlayerShipHit;
            if (raiseEvent != null)
            {
                raiseEvent(coordinates);
            }
        }

        protected virtual void OnPlayerShipMiss((int, int) coordinates)
        {
            var raiseEvent = PlayerShipMiss;
            if (raiseEvent != null)
            {
                raiseEvent(coordinates);
            }
        }

        protected virtual void OnOpponentShipHit((int, int) coordinates)
        {
            var raiseEvent = OpponentShipHit;
            if (raiseEvent != null)
            {
                raiseEvent(coordinates);
            }
        }

        protected virtual void OnOpponentShipMiss((int, int) coordinates)
        {
            var raiseEvent = OpponentShipMiss;
            if (raiseEvent != null)
            {
                raiseEvent(coordinates);
            }
        }

        protected virtual void OnMarkMyTurnEvent()
        {
            var raiseEvent = MarkMyTurnEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnMarkOpponentTurnEvent()
        {
            var raiseEvent = MarkOpponentTurnEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnFirstChosen(Participants participants) 
        {
            var raiseEvent = FirstChosen;
            if (raiseEvent != null)
            {
                raiseEvent(participants);
            }
        }

        protected virtual void OnPlayerShipDestroyed(List<(int, int)> coordinates) 
        {
            var raiseEvent = PlayerShipDestroyed;
            if (raiseEvent != null)
            {
                raiseEvent(coordinates);
            }
        }

        protected virtual void OnOpponentShipDestroyed(List<(int, int)> coordinates)
        {
            var raiseEvent = OpponentShipDestroyed;
            if (raiseEvent != null)
            {
                raiseEvent(coordinates);
            }
        }

        protected virtual void OnDecreaseOpponentShipCount(int deckNumber, int shipsLeft) 
        {
            var raiseEvent = DecreaseOpponentShipCount;
            if (raiseEvent != null)
            {
                raiseEvent(deckNumber, shipsLeft);
            }
        }

        protected virtual void OnDecreasePlayerShipCount(int deckNumber, int shipsLeft)
        {
            var raiseEvent = DecreasePlayerShipCount;
            if (raiseEvent != null)
            {
                raiseEvent(deckNumber, shipsLeft);
            }
        }
    }
}
