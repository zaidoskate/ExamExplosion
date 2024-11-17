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
        private static ExamExplotionService.PlayerManagerClient proxy = new ExamExplotionService.PlayerManagerClient();

        public static int GetPoinsByPlayerId(int playerId)
        {
            try
            {
                int points = proxy.GetPoints(playerId);
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
    }
}
