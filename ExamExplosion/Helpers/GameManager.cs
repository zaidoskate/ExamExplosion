using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExamExplosion.Helpers
{
    public class GameManager : IGameManagerCallback
    {
        private static ExamExplotionService.GameManagerClient proxy = null;
        private InstanceContext context = null;
        private Board boardPage = null;
        //private Stack<Card> deck = new Stack<Card>();

        public GameManager(Board boardPage)
        {
            context = new InstanceContext(this);
            proxy = new GameManagerClient(context);
            this.boardPage = boardPage;
        }

        public static GameManagement GetCurrentGameDetails(string gameCode)
        {
            try
            {
                ExamExplotionService.GameManagement gameObtained = new GameManagement();
                gameObtained = proxy.GetGame(gameCode);
                return gameObtained;
            }
            catch (FaultException faultException)
            {
                //Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                //Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                //Implementar log
                throw timeoutException;
            }
        }

        public static void NotifyEndTurn(string gameCode, string gamertag)
        {
            try
            {
                proxy.NotifyEndTurn(gameCode, gamertag);
            }
            catch (FaultException faultException)
            {
                //Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                //Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                //Implementar log
                throw timeoutException;
            }
        }

        public void UpdateCurrentTurn(string gamertag)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                boardPage.UpdateTurnLabel(gamertag);
            });
        }

        public bool ConnectGame(string gameCode, string gamertag)
        {
            try
            {
                bool connected = proxy.ConnectGame(gameCode, gamertag);
                return connected;
            }
            catch (FaultException faultException)
            {
                //Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                //Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                //Implementar log
                throw timeoutException;
            }
        }

        public void InitializeGame(string gameCode, List<string> gamertags)
        {
            try
            {
                proxy.InitializeGameTurns(gameCode, gamertags.ToArray());
            }
            catch (FaultException faultException)
            {
                //Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                //Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                //Implementar log
                throw timeoutException;
            }
        }

        public void SyncTimer()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                boardPage.ResetTimer();
            });
        }

        //public Card DrawCard()
        //{
        //    return deck.Count > 0 ? deck.Pop() : null;
        //}

        //public void ShuffleDeck()
        //{
        //    var cards = deck.ToList();
        //    //cards.Shuffle(); NO EXISTE EL METODO SHUFFLE PARA cards
        //    deck = new Stack<Card>(cards);
        //}

        //public void AddCardToDeck(Card card)
        //{
        //    deck.Push(card);
        //}

        //public Card GetTopCard()
        //{
        //    return deck.Peek();
        //}

        public void InitializeDeck(string gameCode, int playerCount)
        {
            string gamertag = SessionManager.CurrentSession.gamertag;
            proxy.InitializeDeck(gameCode, playerCount, gamertag);
        }

        private void AddStandardCardsToDeck(int cardCount)
        {
            throw new NotImplementedException();
        }

        public List<string> GetInitialPlayerDeck(string gameCode, string gamertag)
        {
            List<string> playerCardsList = new List<string>();

            Dictionary<string, int> playerDeck = proxy.GetPlayerDeck(gameCode, gamertag);

            foreach (var cardInDeck in playerDeck)
            {
                string path = cardInDeck.Key;
                int count = cardInDeck.Value;
                for(int i = 0; i < count; i++)
                {
                    playerCardsList.Add(path);
                }
            }
            return playerCardsList;
        }
    }
}
