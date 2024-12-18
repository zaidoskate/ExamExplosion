using ExamExplosion;
using ExamExplotionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace XUnitTest.CallbackImplementation
{
    public class GameCallbacks : ExamExplotionService.IGameManagerCallback
    {
        private InstanceContext context;
        private ExamExplotionService.GameManagerClientBase proxy;

        public string NotificationMessage { get; private set; }
        public Stack<CardManagement> GameDeck { get; private set; }
        public CardManagement[] PlayerDeck { get; private set; }
        public string CurrentTurnPlayer { get; private set; }
        public bool IsExamBombReceived { get; private set; }

        public GameCallbacks()
        {
            context = new InstanceContext(this);
            proxy = new ExamExplotionService.GameManagerClientBase(context);

            string gameCode = "SA21";
            string gamertag = "Tlapa11";

            proxy.ConnectGame(gamertag, gameCode);
        }

        public void EndTheGame(string gameCode, string winnerGamertag)
        {
            NotificationMessage = $"Game {gameCode} ended. Winner: {winnerGamertag}";
        }

        public void NotifyCardReceived(CardManagement card)
        {
            NotificationMessage = $"Card received: {card.Name}";
        }

        public void NotifyCardRequested(string gameCode, string playerRequesting)
        {
            NotificationMessage = $"Player {playerRequesting} requested a card in game {gameCode}.";
        }

        public void PrintCardOnBoard(string path)
        {
            NotificationMessage = $"Card displayed from path: {path}";
        }

        public void ReceiveGameDeck(Stack<CardManagement> gameDeck)
        {
            GameDeck = gameDeck;
        }

        public void ReciveAndAddExamBomb(int index)
        {
            IsExamBombReceived = true;
        }

        public void ReciveNotification(string message)
        {
            NotificationMessage = message;
        }

        public void RecivePlayerAndGameDeck(Stack<CardManagement> gameDeck, CardManagement[] playerDeck)
        {
            GameDeck = gameDeck;
            PlayerDeck = playerDeck;
        }

        public void RemoveCardFromStack(bool isTopCard)
        {
            if (GameDeck != null && GameDeck.Count > 0)
            {
                if (isTopCard)
                {
                    GameDeck.Pop();
                }
                else
                {
                    GameDeck = new Stack<CardManagement>(GameDeck.Reverse().Skip(1).Reverse());
                }
            }
        }

        public void SyncTimer()
        {
            NotificationMessage = "Timer synced.";
        }

        public void UpdateCurrentTurn(string gamertag)
        {
            CurrentTurnPlayer = gamertag;
        }
    }
}