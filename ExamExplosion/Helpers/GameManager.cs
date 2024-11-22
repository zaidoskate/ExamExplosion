using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
                using (var proxy = new GameManagerClient(context))
                {
                    return proxy.GetGame(gameCode);
                }
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
                using (var proxy = new GameManagerClient(context))
                {
                    proxy.NotifyEndTurn(gameCode, gamertag);
                }
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
            try
            {
                using (var proxy = new GameManagerClient(context))
                {
                    return proxy.ConnectGame(gameCode, gamertag);
                }
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
        /// Inicializa un juego con una lista de jugadores.
        /// </summary>
        /// <param name="gameCode">Código único del juego.</param>
        /// <param name="gamertags">Lista de gamertags de los jugadores.</param>
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



        public List<string> GetInitialPlayerDeck(string gameCode, string gamertag)
        {
            List<string> playerCardsList = new List<string>();

            Dictionary<string, int> playerDeck = proxy.GetPlayerDeck(gameCode, gamertag);

            foreach (var cardInDeck in playerDeck)
            {
                string path = cardInDeck.Key;
                int count = cardInDeck.Value;
                for (int i = 0; i < count; i++)
                {
                    playerCardsList.Add(path);
                }
            }
            return playerCardsList;
        }
    }
}
