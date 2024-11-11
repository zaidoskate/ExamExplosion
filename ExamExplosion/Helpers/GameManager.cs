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
    }
}
