using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace ExamExplosion.Helpers
{
    public class GameManager : IGameManagerCallback
    {
        private InstanceContext context = null;
        private Board boardPage = null;
        private Stack<Card> deck = new Stack<Card>();

        public GameManager(Board boardPage)
        {
            context = new InstanceContext(this);
            this.boardPage = boardPage;
        }

        public static GameManagement GetCurrentGameDetails(string gameCode)
        {
            try
            {
                using (var proxy = new GameManagerClient(new InstanceContext(null)))
                {
                    return proxy.GetGame(gameCode);
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
                throw timeoutException;
            }
        }

        public static void NotifyEndTurn(string gameCode, string gamertag)
        {
            try
            {
                using (var proxy = new GameManagerClient(new InstanceContext(null)))
                {
                    proxy.NotifyEndTurn(gameCode, gamertag);
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
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
                using (var proxy = new GameManagerClient(context))
                {
                    return proxy.ConnectGame(gameCode, gamertag);
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
                throw timeoutException;
            }
        }

        public void InitializeGame(string gameCode, List<string> gamertags)
        {
            try
            {
                using (var proxy = new GameManagerClient(context))
                {
                    proxy.InitializeGameTurns(gameCode, gamertags.ToArray());
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
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

        public Card DrawCard()
        {
            return deck.Count > 0 ? deck.Pop() : null;
        }

        public void ShuffleDeck()
        {
            var cards = deck.ToList();
            // cards.Shuffle(); NO EXISTE EL MÉTODO SHUFFLE PARA cards
            deck = new Stack<Card>(cards);
        }

        public void AddCardToDeck(Card card)
        {
            deck.Push(card);
        }

        public Card GetTopCard()
        {
            return deck.Peek();
        }

        public void InitializeDeck(string gameCode, int playerCount)
        {
            try
            {
                using (var proxy = new GameManagerClient(context))
                {
                    proxy.InitializeDeck(gameCode, playerCount);
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
                throw timeoutException;
            }
        }

        private void AddStandardCardsToDeck(int cardCount)
        {
            throw new NotImplementedException();
        }
    }
}
