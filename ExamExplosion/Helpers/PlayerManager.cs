using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public class PlayerManager
    {
        private static ExamExplotionService.PlayerManagerClient proxy;
        public static int GetPoinsByPlayerId(int playerId)
        {
            try
            {
                proxy = new ExamExplotionService.PlayerManagerClient();
                int points = proxy.GetPoints(playerId);
                proxy.Close();
                return points;
            }
            catch (FaultException faultException)
            {
                //Implementar log
                throw faultException;
            }
            catch(CommunicationException communicationException)
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

        public static Dictionary<string, int> GetGlobalLeaderboard()
        {
            try
            {
                proxy = new ExamExplotionService.PlayerManagerClient();
                Dictionary<string, int> globalLeaderboard = proxy.GetGlobalLeaderboard();
                proxy.Close();
                return globalLeaderboard;
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
        public static Dictionary<string, int> GetFriendsLeaderboard(int playerId)
        {
            try
            {
                proxy = new ExamExplotionService.PlayerManagerClient();
                Dictionary<string, int> globalLeaderboard = proxy.GetFriendsLeaderboard(playerId);
                proxy.Close();
                return globalLeaderboard;
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
