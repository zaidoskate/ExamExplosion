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
        public static int GetPointsByPlayerId(int playerId)
        {
            int points = 0;

            try
            {
                using (var proxy = new ExamExplotionService.PlayerManagerClient())
                {
                    points = proxy.GetPoints(playerId);
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

            return points;
        }
    }
}
