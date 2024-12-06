using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace ExamExplosion.Helpers
{
    /// <summary>
    /// Clase que gestiona las operaciones del juego, como la inicialización, sincronización de y administración de los stacks de cartas.
    /// </summary>
    public class GameManager : IGameManagerCallback
    {
        // Contexto para la comunicación con el servicio WCF.
        private InstanceContext context = null;

        private ExamExplotionService.GameManagerClient proxy = null;
        private GameResourcesManager gameResources = null;

        // Página de tablero utilizada para actualizar la interfaz gráfica.
        private Board boardPage = null;
        //Pila que representa el mazo de cartas
        //private Stack<Card> deck = new Stack<Card>();

        /// <summary>
        /// Inicializa una nueva instancia de la clase GameManager.
        /// </summary>
        /// <param name="boardPage">Instancia de la página del tablero asociada al juego.</param>
        public GameManager(Board boardPage)
        {
            context = new InstanceContext(this);
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

        /// <summary>
        /// Extrae una carta del mazo.
        /// </summary>
        /// <returns>La carta extraída, o null si el mazo está vacío.</returns>

        //public Card DrawCard()
        //{
        //    string gamertag = SessionManager.CurrentSession.gamertag;
        //    proxy.InitializeDeck(gameCode, playerCount, gamertag);
        //}

        /// <summary>
        /// Mezcla las cartas del mazo.
        /// </summary>
        //public void ShuffleDeck()
        //{
        //    var cards = deck.ToList();
        //    //cards.Shuffle(); NO EXISTE EL METODO SHUFFLE PARA cards
        //    deck = new Stack<Card>(cards);
        //}

        /// <summary>
        /// Agrega una carta al mazo.
        /// </summary>
        /// <param name="card">Carta a agregar.</param>
        //public void AddCardToDeck(Card card)
        //{
        //    List<string> playerCardsList = new List<string>();

        //    Dictionary<string, int> playerDeck = proxy.GetPlayerDeck(gameCode, gamertag);



        //public List<string> GetInitialPlayerDeck(string gameCode, string gamertag)
        //{
        //    List<string> playerCardsList = new List<string>();

        //    Dictionary<string, int> playerDeck = proxy.GetPlayerDeck(gameCode, gamertag);

        //    foreach (var cardInDeck in playerDeck)
        //    {
        //        string path = cardInDeck.Key;
        //        int count = cardInDeck.Value;
        //        for (int i = 0; i < count; i++)
        //        {
        //            playerCardsList.Add(path);
        //        }
        //    }
        //    return playerCardsList;
        //}
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

        public void DrawCard(string gameCode, string gamertag)
        {
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
            boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
            this.NotifyEndTurn(gameCode, gamertag);
        }

        public void DrawBottomCard(string gameCode, string gamertag)
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
            boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
            this.NotifyEndTurn(gameCode, gamertag);
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
            proxy.RequestCard(gameCode, playerRequested, playerRequesting);
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
                    gameResources.CurrentIndex -= 8;
                }
            }
            else
            {
                if (gameResources.CurrentIndex < gameResources.PlayerCards.Count - 8)
                {
                    gameResources.CurrentIndex += 8;
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
            else if (cardToSelect.Path.Substring(0, 4) == selectedCards[0].Substring(0, 4))
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

        public bool PlayCards(string gameCode)
        {
            var selectedCards = gameResources.GetSelectedCardsPaths();
            if (selectedCards.Count == 1)
            {
                string cardSelected = selectedCards[0];
                switch (cardSelected)
                {
                    case "takeFromBelow":
                        DrawBottomCard(gameCode, SessionManager.CurrentSession.gamertag);
                        break;
                    case "exempt":
                        //exempt
                        break;
                    case "leftTeam":
                        //leftTeam
                        break;
                    case "shuffle":
                        ShuffleDeck(gameCode);
                        break;
                    case "viewTheFuture":
                        SeeTheFuture();
                        break;
                    case "please":
                        boardPage.StartPlayerSelection(gameCode); ;
                        break;
                }
            }
            else
            {
                List<string> teacherCards = new List<string> { "profeA", "profeM", "profeO", "profeR", "profeS" };
                if (selectedCards.Count == 2 && selectedCards[0] == selectedCards[1] && teacherCards.Contains(selectedCards[0]))
                {
                    boardPage.StartPlayerSelection(gameCode);
                }
            }
            //primero se debe validar que se pueda tirar la o las cartas
            //validar que no estes en condicion de perder por un exam bomb
            //validar que los dos profes sean iguales
            //validar que sean dos profes
            //si es alguna carta en especial como ataque o ver el futuro, se llaman los metodos al boardPage
            //si pasa las validaciones, se pinta en todos los clientes
            proxy.NotifyCardOnBoard(gameCode, selectedCards[0]);
            boardPage.ClearSelectedCards();

            //antes de terminar se deben limpiar las cartas seleccionadas del resources y del boardPage.SelectedCards
            return true;
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
                proxy.SendCardToPlayer(gameCode, playerRequesting, cardManagement);
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

        internal void RemoveCardFromPlayerHand(int cardId)
        {
            var playerHand = gameResources.PlayerCards;
            playerHand.RemoveAt(cardId);
            boardPage.UpdatePlayerDeck(gameResources.PlayerCards, gameResources.CurrentIndex);
        }
    }
}
