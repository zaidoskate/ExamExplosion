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
        private LobbyManagerClient proxy = null;
        private Lobby lobbyPage = null;
        public LobbyManager(Lobby _lobbyPage)
        {
            context = new InstanceContext(this);
            proxy = new LobbyManagerClient(context);
            lobbyPage = _lobbyPage;
        }

        public LobbyManager()
        {
            context = new InstanceContext(this);
            proxy = new LobbyManagerClient(context);
        }
        public string CreateLobby(Game game)
        {
            try
            {
                GameManagement gameM = new GameManagement();
                gameM.NumberPlayers = game.NumberPlayers;
                gameM.TimePerTurn = game.TimePerTurn;
                gameM.HostPlayerId = game.HostPlayerId;
                gameM.InvitationCode = game.InvitationCode;
                gameM.Lives = game.Lives;

                string newCode = proxy.CreateLobby(gameM);
            
                return newCode;
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

        public bool JoinLobby(string code, string gamertag)
        {
            try
            {
                return proxy.JoinLobby(code, gamertag);

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

        public void ConnectLobby(string gamertag, string code)
        {
            try
            {
                proxy.Connect(gamertag, code);
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

        public void SendMessage(string code, string message, string gamertag)
        {
            try
            {
                proxy.SendMessage(code, gamertag, message);
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
        public void ReceiveMessage(string gamertag, string message)
        {
            lobbyPage.PrintNewMessage(gamertag, message);
        }

        public void DisconnectLobby(string lobbyCode, string gamertag)
        {
            try
            {
                proxy.Disconnect(lobbyCode, gamertag);
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
                proxy.UpdatePlayerStatus(lobbyCode, gamertag, isReady);
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
                proxy.PlayGame(lobbyCode);
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
    }
}
