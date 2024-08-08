namespace SeaFightToolkit.SignalR.Contracts
{
    /// <summary>
    /// Контракт игрового хаба SignalR
    /// </summary>
    public interface ISeaFightHub
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        public Task Register(string name);

        /// <summary>
        /// Предупреждение о том, что имя уже занято
        /// </summary>
        public Task NameIsOccupied();

        /// <summary>
        /// Корабль игрока уничтожен
        /// </summary>
        /// <param name="ship">Корабль</param>
        /// <param name="deckCount">Количество палуб</param>
        /// <param name="liveShips">Количество живых кораблей того же класса</param>
        /// <returns></returns>
        public Task MyShipDestroyed(string ship, string deckCount, string liveShips);

        /// <summary>
        /// Корабль оппонента уничтожен
        /// </summary>
        /// <param name="ship">Корабль</param>
        /// <param name="deckCount">Количество палуб</param>
        /// <param name="liveShips">Количество живых кораблей того же класса</param>
        /// <returns></returns>
        public Task OpponentShipDestroyed(string ship, string deckCount, string liveShips);

        /// <summary>
        /// Промах оппонента
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Task OpponentMiss(string x, string y);

        /// <summary>
        /// Попадание оппонента
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Task OpponentHit(string x, string y);

        /// <summary>
        /// Промах игрока
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Task MyMiss(string x, string y);

        /// <summary>
        /// Попадание игрока
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Task MyHit(string x, string y);

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="name">Имя игрока</param>
        /// <param name="message">Сообщение</param>
        public Task SendMessage(string name, string message);

        /// <summary>
        /// Рассылка сообщения всем подключенным игрокам
        /// </summary>
        /// <param name="name">От кого сообщение (Имя)</param>
        /// <param name="message">Сообщение</param>
        public Task BroadcastMessage(string name, string message);

        /// <summary>
        /// Отказ игрока предложению поиграть
        /// </summary>
        public Task DenyOfferYouAreBusy();

        /// <summary>
        /// Отказ оппонента предложению поиграть
        /// </summary>
        public Task DenyOfferOpponentIsBusy();

        /// <summary>
        /// Предложение игры
        /// </summary>
        /// <param name="nameFrom">От кого (Имя)</param>
        /// <param name="idFrom">От кого (Идентификатор)</param>
        /// <param name="nameTo">Кому (Имя)</param>
        public Task OfferGame(string nameFrom, string idFrom, string nameTo);

        /// <summary>
        /// Ответ на предложение
        /// </summary>
        /// <param name="firstPlayerName">Имя первого игрока</param>
        /// <param name="secondPlayerName">Имя второго игрока</param>
        /// <param name="answer">Ответ</param>
        /// <param name="sessionId">Идентификатор сессии</param>
        public Task AnswerOffer(string firstPlayerName, string secondPlayerName, string answer, string sessionId);

        /// <summary>
        /// Победа
        /// </summary>
        /// <param name="sessionVictoryId">Идентификатор сессии</param>
        public Task Victory(string sessionVictoryId);

        /// <summary>
        /// Получить перечень игроков
        /// </summary>
        /// <param name="players">Игроки</param>
        public Task GetPlayers(string players);

        /// <summary>
        /// Прерывание игры
        /// </summary>
        /// <param name="senderName">Имя игрока</param>
        public Task OpponentAbortGame(string senderName);

        /// <summary>
        /// Начало игры
        /// </summary>
        /// <param name="whoFirstId">Идентифкатор того, чей ход первый</param>
        /// <param name="whoFirstName">Имя того, чей ход первый</param>
        public Task StartGame(string whoFirstId, string whoFirstName);

        /// <summary>
        /// Готовность начать игру
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="playerId">Идентификатор игрока</param>
        /// <param name="ships">Корабли</param>
        public Task ReadyToStart(string sessionId, string playerId, string ships);

        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="playerId">Идентификатор игрока</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Task Move(string sessionId, string playerId, string x, string y);

        /// <summary>
        /// Завершение хода
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="playerId">Идентификатор игрока</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Task CompletingTurn(string sessionId, string playerId, string x, string y);

        /// <summary>
        /// Прерывание игровой сессии
        /// </summary>
        public Task NetAbortGame();
    }
}
