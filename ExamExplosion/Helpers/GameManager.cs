using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ExamExplosion.Helpers
{
    /// <summary>
    /// Clase que gestiona las operaciones del juego, como la inicialización, sincronización de y administración de los stacks de cartas.
    /// </summary>
    public class GameManager : IGameManagerCallback
    {
        private readonly ExamExplotionService.GameManagerClient proxy = null;
        private GameResourcesManager gameResources = null;

        private readonly Board boardPage = null;

        public GameManager()
        {
            InstanceContext context = new InstanceContext(this);
            proxy = new GameManagerClient(context);
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase GameManager.
        /// </summary>
        /// <param name="boardPage">Instancia de la página del tablero asociada al juego.</param>
        public GameManager(Board boardPage)
        {
            InstanceContext context = new InstanceContext(this);
            proxy = new GameManagerClient(context);
            gameResources = new GameResourcesManager();
            this.boardPage = boardPage;
        }

        /// <summary>
        /// Obtiene los detalles actuales de un juego mediante su código.
        /// </summary>
        /// <param name="gameCode">Código único del juego.</param>
        /// <returns>Detalles del juego como un objeto GameManagement (gameCode, maxPlayers, timePerTurn).</returns>
        public GameManagement GetCurrentGameDetails(string gameCode)
        {
            try
            {
                return proxy.GetGame(gameCode);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        /// <summary>
        /// Notifica al servidor el fin de turno de un jugador.
        /// </summary>
        /// <param name="gameCode">Código único del juego.</param>
        /// <param name="gamertag">Gamertag del jugador que termina su turno.</param>
        public void NotifyEndTurn(string gameCode, string gamertag)
        {
            try
            {
                proxy.NotifyEndTurn(gameCode, gamertag);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        /// <summary>
        /// Actualiza el turno actual en la interfaz gráfica.
        /// </summary>
        /// <param name="gamertag">Gamertag del jugador cuyo turno es el actual.</param>
        public void UpdateCurrentTurn(string gamertag)
        {
            boardPage.UpdateButtonsVisibility(gamertag);
            Application.Current.Dispatcher.Invoke(() =>
            {
                boardPage.UpdateTurnLabel(gamertag);
            });
        }
        /// <summary>
        /// Conecta a un jugador a un juego existente.
        /// </summary>
        /// <param name="gameCode">Código único del juego.</param>
        /// <param name="gamertag">Gamertag del jugador que se conecta.</param>
        /// <returns>Verdadero si la conexión fue exitosa, falso en caso contrario.</returns>
        public bool ConnectGame(string gameCode, string gamertag)
        {
            bool playerIsConnected = false;
            try
            {
                playerIsConnected = proxy.ConnectGame(gameCode, gamertag);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
            return playerIsConnected;
        }

        /// <summary>
        /// Inicializa un juego con una lista de jugadores.
        /// </summary>
        /// <param name="gameCode">Código único del juego.</param>
        /// <param name="gamertags">Lista de gamertags de los jugadores.</param>
        public void InitializeGame(string gameCode, List<string> gamertags)
        {
            try
            {
                proxy.InitializeGameTurns(gameCode, gamertags.ToArray());
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        /// <summary>
        /// Sincroniza el temporizador del juego.
        /// </summary>
        public void SyncTimer()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                boardPage.ResetTimer();
            });
        }

        public void InitializeGameDeck(string gameCode, int playerCount)
        {
            try
            {
                string gamertag = SessionManager.CurrentSession.gamertag;
                proxy.InitializeDeck(gameCode, playerCount, gamertag);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        public void RecivePlayerAndGameDeck(Stack<CardManagement> gameDeck, CardManagement[] playerDeck)
        {
            Stack<Card> mappedGameDeck = new Stack<Card>();
            foreach (var cardManagement in gameDeck)
            {
                mappedGameDeck.Push(new Card
                {
                    Name = cardManagement.CardName,
                    Path = cardManagement.CardPath
                });
            }
            List<Card> mappedPlayerDeck = playerDeck.Select(cardManagement => new Card
            {
                Name = cardManagement.CardName,
                Path = cardManagement.CardPath,
                IsSelected = false
            }).ToList();

            gameResources.GameDeck = mappedGameDeck;
            gameResources.PlayerCards = mappedPlayerDeck;
            gameResources.CurrentIndex = 0;

            boardPage.UpdatePlayerDeck(mappedPlayerDeck, 0);
            boardPage.UpdateGameDeckCount(mappedGameDeck.Count);
        }

        public bool DrawCard(string gameCode, string gamertag)
        {
            if(gameResources.GameDeck.Count == 0)
            {
                return false;
            }
            gameResources.DrawTopCard();
            boardPage.UpdateGameDeckCount(gameResources.GameDeck.Count);
            try
            {
                proxy.NotifyDrawCard(gameCode, gamertag, true);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
            if (gameResources.HasBomb)
            {
                boardPage.DisplayNotification(ExamExplosion.Properties.Resources.boardLblReregistrationNeeded);
                boardPage.DisplayExamBomb();
                return false;
            }
            else
            {
                boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
                this.NotifyEndTurn(gameCode, gamertag);
                return true;
            }
        }

        public void DrawBottomCard(string gameCode, string gamertag)
        {
            if (gameResources.GameDeck.Count != 0)
            {
                gameResources.DrawBottomCard();
                boardPage.UpdateGameDeckCount(gameResources.GameDeck.Count);
                boardPage.DrawCardAnimation();
                try
                {
                    proxy.NotifyDrawCard(gameCode, gamertag, false);
                }
                catch (FaultException faultException)
                {
                    throw faultException;
                }
                catch (CommunicationException communicationException)
                {
                    throw communicationException;
                }
                catch (TimeoutException timeoutException)
                {
                    throw timeoutException;
                }
                if (gameResources.HasBomb)
                {
                    boardPage.DisplayNotification(ExamExplosion.Properties.Resources.boardLblReregistrationNeeded);
                    boardPage.DisplayExamBomb();
                }
                else
                {
                    boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
                    this.NotifyEndTurn(gameCode, gamertag);
                }
            }
        }

        public void SeeTheFuture()
        {
            List<Card> topCards = gameResources.SeeTheFuture();
            boardPage.ShowTopCards(topCards);
        }

        public void ShuffleDeck(string gameCode)
        {
            Stack<Card> gameDeckShuffled = gameResources.ShuffleGameDeck();
            Stack<CardManagement> mappedGameDeck = new Stack<CardManagement>();
            foreach (var card in gameDeckShuffled)
            {
                mappedGameDeck.Push(new CardManagement
                {
                   CardName = card.Name,
                   CardPath = card.Path,
                });
            }
            proxy.SendShuffleDeck(gameCode, mappedGameDeck);
        }

        public void RequestCard(string gameCode, string playerRequested, string playerRequesting)
        {
            try
            {
                proxy.RequestCard(gameCode, playerRequested, playerRequesting);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }
        public void ReceiveGameDeck(Stack<CardManagement> gameDeck)
        {
            Stack<Card> mappedGameDeck = new Stack<Card>();
            foreach (var cardManagement in gameDeck)
            {
                mappedGameDeck.Push(new Card
                {
                    Name = cardManagement.CardName,
                    Path = cardManagement.CardPath
                });
            }
            gameResources.GameDeck = mappedGameDeck;
        }

        public void RemoveCardFromStack(bool isTopCard)
        {
            if (isTopCard)
            {
                gameResources.RemoveTopCard();
            }
            else
            {
                gameResources.RemoveBottomCard();
            }
            boardPage.UpdateGameDeckCount(gameResources.GameDeck.Count);
        }

        public void UpdatePlayerDeck(bool showLeftCards)
        {
            if (showLeftCards)
            {
                if (gameResources.CurrentIndex > 0)
                {
                    gameResources.CurrentIndex -= 6;
                }
            }
            else
            {
                if (gameResources.CurrentIndex < gameResources.PlayerCards.Count - 6)
                {
                    gameResources.CurrentIndex += 6;
                }
            }
            boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
        }

        public bool SelectCard(int index)
        {
            bool cardSelected = false;
            var selectedCards = gameResources.GetSelectedCardsPaths();
            Card cardToSelect = gameResources.PlayerCards[index];
            if (selectedCards.Count == 0)
            {
                cardSelected = true;
                gameResources.PlayerCards[index].IsSelected = true;
            }
            else if (selectedCards.Count > 1)
            {
                cardSelected = false;
            }
            else if (cardToSelect.Path == selectedCards[0])
            {
                cardSelected = true;
                gameResources.PlayerCards[index].IsSelected = true;
            }
            return cardSelected;
        }

        public void DeselectCard(int tag)
        {
            gameResources.PlayerCards[tag].IsSelected = false;
        }
        public void PrintCardOnBoard(string path)
        {
            boardPage.PrintCardOnBoard(path);
        }

        public void PlayCards(string gameCode)
        {
            var selectedCards = gameResources.GetSelectedCardsPaths();
            if(selectedCards.Count < 1)
            {
                return;
            }
            if (selectedCards.Count == 1)
            {
                string cardSelected = selectedCards[0];
                if (gameResources.HasBomb)
                {
                    if(cardSelected == "reRegistration")
                    {
                        ReinsertExamBomb(gameCode);
                    }
                }
                else
                {
                    switch (cardSelected)
                    {
                        case "takeFromBelow":
                            DrawBottomCard(gameCode, SessionManager.CurrentSession.gamertag);
                            break;
                        case "exempt":
                            SendDoubleTurn(gameCode, "next");
                            break;
                        case "leftTeam":
                            boardPage.StartPlayerSelection(gameCode);
                            break;
                        case "shuffle":
                            ShuffleDeck(gameCode);
                            break;
                        case "viewTheFuture":
                            SeeTheFuture();
                            break;
                        case "please":
                            boardPage.StartPlayerSelection(gameCode);
                            break;
                    }
                }
            }
            else if(selectedCards.Count == 2)
            {
                List<string> teacherCards = new List<string> { "profeA", "profeM", "profeO", "profeR", "profeS" };
                if (selectedCards.Count == 2 && selectedCards[0] == selectedCards[1] && teacherCards.Contains(selectedCards[0]))
                {
                    boardPage.StartPlayerSelection(gameCode);
                }
            }
            if (selectedCards.Count > 0)
            {
                if((gameResources.HasBomb && selectedCards[0] == "reRegistration") || (!gameResources.HasBomb && selectedCards[0] != "reRegistration"))
                {
                    try
                    {
                        proxy.NotifyCardOnBoard(gameCode, selectedCards[0]);
                    }
                    catch (FaultException faultException)
                    {
                        throw faultException;
                    }
                    catch (CommunicationException communicationException)
                    {
                        throw communicationException;
                    }
                    catch (TimeoutException timeoutException)
                    {
                        throw timeoutException;
                    }
                    boardPage.ClearSelectedCards();
                    if (gameResources.HasBomb)
                    {
                        gameResources.HasBomb = false;
                        NotifyEndTurn(gameCode, SessionManager.CurrentSession.gamertag);
                    }
                }
            }
            
        }

        public void SendDoubleTurn(string gameCode, string gamertag)
        {
            try{
                proxy.SendDoubleTurn(gameCode, gamertag);
                proxy.NotifyMessage(gameCode, SessionManager.CurrentSession.gamertag, gamertag, ExamExplosion.Properties.Resources.boardLblDoubleTurn);
                proxy.NotifyEndTurn(gameCode, SessionManager.CurrentSession.gamertag);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
            
        }

        public void NotifyCardRequested(string gameCode, string playerRequesting)
        {
            if (gameResources.PlayerCards.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(gameResources.PlayerCards.Count);

                Card cardToSend = gameResources.PlayerCards[randomIndex];
                CardManagement cardManagement = new CardManagement{ CardName = cardToSend.Name, CardPath = cardToSend.Path };

                gameResources.PlayerCards.RemoveAt(randomIndex);
                RemoveCardFromPlayerHand(randomIndex);
                boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
                try
                {
                    proxy.SendCardToPlayer(gameCode, playerRequesting, cardManagement);
                }
                catch (FaultException faultException)
                {
                    throw faultException;
                }
                catch (CommunicationException communicationException)
                {
                    throw communicationException;
                }
                catch (TimeoutException timeoutException)
                {
                    throw timeoutException;
                }
                boardPage.ShowCardRequested(playerRequesting);
            }
        }

        public void NotifyCardReceived(CardManagement card)
        {
            if (card != null)
            {
                Card cardObtained = new Card{ Name = card.CardName, Path = card.CardPath };
                gameResources.AddCard(cardObtained);
                boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
                boardPage.ShowCardObtained(card.CardName);
            }
        }

        public void RemoveCardFromPlayerHand(int cardId)
        {
            var playerHand = gameResources.PlayerCards;
            if (cardId >= playerHand.Count)
            {
                cardId--;
            }
            playerHand.RemoveAt(cardId);
            boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
        }

        public void ReciveNotification(string message)
        {
            boardPage.DisplayNotification(message);
        }

        public void EndTheGame(string gameCode, string winnerGamertag)
        {
            boardPage.GoEndGame(gameCode, winnerGamertag);
        }

        public bool ValidateLostGame(string gameCode)
        {
            bool lostGame = false;
            string gamertag = SessionManager.CurrentSession.gamertag;

            if (gameResources.HasBomb)
            {
                if(gameResources.Hp > 1)
                {
                    gameResources.ReduceHp();
                    boardPage.RemoveHp(gameResources.Hp);
                    ReinsertExamBomb(gameCode);
                }
                else
                {
                    boardPage.DeletePlayerDeck();
                    boardPage.DisplayNotification(ExamExplosion.Properties.Resources.boardLblPlayerLost);
                    try
                    {
                        proxy.NotifyMessage(gameCode, gamertag, "all", gamertag + " " + ExamExplosion.Properties.Resources.boardLblAnotherPlayerLost);
                        proxy.RemovePlayerByGame(gameCode, gamertag);
                    }
                    catch (FaultException faultException)
                    {
                        throw faultException;
                    }
                    catch (CommunicationException communicationException)
                    {
                        throw communicationException;
                    }
                    catch (TimeoutException timeoutException)
                    {
                        throw timeoutException;
                    }

                    lostGame = true;
                }
                try
                {
                    proxy.NotifyEndTurn(gameCode, gamertag);
                }
                catch (FaultException faultException)
                {
                    throw faultException;
                }
                catch (CommunicationException communicationException)
                {
                    throw communicationException;
                }
                catch (TimeoutException timeoutException)
                {
                    throw timeoutException;
                }
            }

            return lostGame;
        }

        private void ReinsertExamBomb(string gameCode)
        {
            proxy.SendExamBomb(gameCode, gameResources.GameDeck.Count);
        }

        public void ReciveAndAddExamBomb(int index)
        {
            gameResources.AddExamBomb(index);
            boardPage.UpdateGameDeckCount(gameResources.GameDeck.Count);
        }

        public int GetGameId(string gameCode)
        {
            int gameId = -1;
            try
            {
                gameId = proxy.GetGameId(gameCode);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
            return gameId;

        }

        public List<int> GetPlayersByGameCode(int gameId)
        {
            List<int> playersId = new List<int>();
            List<int> accountsId = new List<int>();
            try
            {
                playersId = proxy.GetGamePlayers(gameId).ToList();
                accountsId = proxy.GetAccountsIdByPlayerId(playersId.ToArray()).ToList();
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
            return accountsId;
        }

        public List<int> GetPlayersId(List<string> gamertags)
        {
            List<int> playerIds = new List<int>();
            AccountManager accountManager = new AccountManager();
            foreach (var gamertag in gamertags)
            {
                playerIds.Add(accountManager.GetAccountIdByGamertag(gamertag));
            }
            return playerIds;
        }
        public void AddPlayersToGame(List<string> playerGamertags, string gameCode)
        {
            try
            {
                proxy.AddPlayersToGame(playerGamertags.ToArray(), gameCode);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }
        public void CloseConnection()
        {
            //proxy.Close();
        }

        public void AddHitPoints(int lives)
        {
            gameResources.Hp = lives;
        }
    }
}
