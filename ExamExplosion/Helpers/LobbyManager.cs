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
    public class LobbyManager : ILobbyManagerCallback
    {
        private InstanceContext context = null;
        private Lobby lobbyPage = null;

        public LobbyManager(Lobby _lobbyPage)
        {
            context = new InstanceContext(this);
            lobbyPage = _lobbyPage;
        }

        public LobbyManager()
        {
            context = new InstanceContext(this);
        }

        public string CreateLobby(Game game)
        {
            try
            {
                using (var proxy = new LobbyManagerClient(context))
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

        public bool JoinLobby(string code, string gamertag)
        {
            try
            {
                using (var proxy = new LobbyManagerClient(context))
                {
                    return proxy.JoinLobby(code, gamertag);
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

        public void ConnectLobby(string gamertag, string code)
        {
            try
            {
                using (var proxy = new LobbyManagerClient(context))
                {
                    proxy.Connect(gamertag, code);
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

        public void SendMessage(string code, string message, string gamertag)
        {
            try
            {
                using (var proxy = new LobbyManagerClient(context))
                {
                    proxy.SendMessage(code, gamertag, message);
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

        public void ReceiveMessage(string gamertag, string message)
        {
            lobbyPage.PrintNewMessage(gamertag, message);
        }

        public void DisconnectLobby(string lobbyCode, string gamertag)
        {
            try
            {
                using (var proxy = new LobbyManagerClient(context))
                {
                    proxy.Disconnect(lobbyCode, gamertag);
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

        public void ChangeStatus(string lobbyCode, string gamertag, bool isReady)
        {
            try
            {
                using (var proxy = new LobbyManagerClient(context))
                {
                    proxy.UpdatePlayerStatus(lobbyCode, gamertag, isReady);
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

        public void StartGame(Dictionary<string, bool> lobbyPlayers)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                foreach (var player in lobbyPlayers)
                {
                    lobbyPage.NavigateToBoard();
                }
            });
        }

        public void PlayGame(string lobbyCode)
        {
            try
            {
                using (var proxy = new LobbyManagerClient(context))
                {
                    proxy.PlayGame(lobbyCode);
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
    }
}
