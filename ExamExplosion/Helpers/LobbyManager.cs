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
    /// <summary>
    /// Administra la funcionalidad del lobby, incluyendo la creación, unión e interacción con un lobby de juego multijugador.
    /// Implementa la interfaz <see cref="ILobbyManagerCallback"/> para comunicación duplex.
    /// </summary>
    public class LobbyManager : ILobbyManagerCallback
    {
        /// <summary>
        /// Contexto de instancia utilizado para la comunicación duplex con el servidor.
        /// </summary>
        private ExamExplotionService.LobbyManagerClient proxy = null;

        /// <summary>
        /// Referencia a la página de la interfaz de usuario del lobby para actualizar su contenido o navegar a otras vistas.
        /// </summary>
        private Lobby lobbyPage = null;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LobbyManager"/> con una página de interfaz de usuario del lobby especificada.
        /// </summary>
        /// <param name="_lobbyPage">La página de interfaz de usuario del lobby con la que se interactuará.</param>
        public LobbyManager(Lobby _lobbyPage)
        {
            lobbyPage = _lobbyPage;
            InstanceContext context = new InstanceContext(this);
            proxy = new ExamExplotionService.LobbyManagerClient(context);
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LobbyManager"/> sin especificar una página de interfaz de usuario del lobby.
        /// </summary>
        public LobbyManager()
        {
            InstanceContext context = new InstanceContext(this);
            proxy = new ExamExplotionService.LobbyManagerClient(context);
        }

        /// <summary>
        /// Crea un nuevo lobby para un juego especificado.
        /// </summary>
        /// <param name="game">Los detalles de configuración del juego.</param>
        /// <returns>El código de invitación del lobby creado.</returns>
        public string CreateLobby(Game game)
        {
            try
            {
                GameManagement gameM = new GameManagement
                {
                    NumberPlayers = game.NumberPlayers,
                    TimePerTurn = game.TimePerTurn,
                    HostPlayerId = game.HostPlayerId,
                    InvitationCode = game.InvitationCode,
                    Lives = game.Lives
                };

                return proxy.CreateLobby(gameM);
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
        /// Permite a un jugador unirse a un lobby existente utilizando un código de invitación y su gamertag.
        /// </summary>
        /// <param name="code">El código de invitación del lobby.</param>
        /// <param name="gamertag">El gamertag del jugador que se unirá.</param>
        /// <returns>True si la unión fue exitosa, de lo contrario False.</returns>
        public bool JoinLobby(string code, string gamertag)
        {
            try
            {
                return proxy.JoinLobby(code, gamertag);
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
        /// Conecta a un jugador a un lobby existente.
        /// </summary>
        /// <param name="gamertag">El gamertag del jugador que se conectará.</param>
        /// <param name="code">El código del lobby al que conectarse.</param>
        public void ConnectLobby(string gamertag, string code)
        {
            try
            {
                proxy.Connect(gamertag, code);
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
        /// Envía un mensaje en el lobby.
        /// </summary>
        /// <param name="code">El código del lobby.</param>
        /// <param name="message">El mensaje a enviar.</param>
        /// <param name="gamertag">El gamertag del remitente.</param>
        public void SendMessage(string code, string message, string gamertag)
        {
            try
            {
                proxy.SendMessage(code, gamertag, message);
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
        /// Recibe un mensaje de otro jugador en el lobby.
        /// </summary>
        /// <param name="gamertag">El gamertag del remitente.</param>
        /// <param name="message">El mensaje recibido.</param>
        public void ReceiveMessage(string gamertag, string message)
        {
            lobbyPage.PrintNewMessage(gamertag, message);
        }

        /// <summary>
        /// Desconecta a un jugador de un lobby.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby.</param>
        /// <param name="gamertag">El gamertag del jugador que se desconecta.</param>
        public void DisconnectLobby(string lobbyCode, string gamertag)
        {
            try
            {
                proxy.Disconnect(lobbyCode, gamertag);
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
        /// Actualiza la interfaz del lobby con el estado de los jugadores.
        /// </summary>
        /// <param name="playerStatus">Un diccionario con los gamertags y su estado (listo o no).</param>
        public void Repaint(Dictionary<string, bool> playerStatus)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                lobbyPage.ClearPlayers();
                int index = 0;
                foreach (var player in playerStatus)
                {
                    lobbyPage.PrintLobbyPlayers(player.Key, player.Value, index);
                    index++;
                }
            });
        }

        /// <summary>
        /// Cambia el estado de un jugador (listo o no listo) en el lobby.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby.</param>
        /// <param name="gamertag">El gamertag del jugador.</param>
        /// <param name="isReady">True si el jugador está listo, False de lo contrario.</param>
        public void ChangeStatus(string lobbyCode, string gamertag, bool isReady)
        {
            try
            {
                proxy.UpdatePlayerStatus(lobbyCode, gamertag, isReady);
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
        /// Inicia el juego y navega hacia la interfaz del tablero a cada player dentro de la lobby.
        /// </summary>
        /// <param name="lobbyPlayers">Un diccionario con los jugadores del lobby y su estado.</param>
        public void StartGame(Dictionary<string, bool> lobbyPlayers)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                lobbyPage.NavigateToBoard();
            });
        }

        /// <summary>
        /// Inicia el flujo del juego para un lobby específico.
        /// </summary>
        /// <param name="lobbyCode">El código del lobby.</param>
        public void PlayGame(string lobbyCode)
        {
            try
            {
                proxy.PlayGame(lobbyCode);
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

        public void UpdateHost()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                lobbyPage.UpdateHost();
            });
        }
    }
}
