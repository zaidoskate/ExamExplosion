using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public class ReportManager
    {
        public bool ReportPlayer(int idPlayerReported)
        {
            try
            {
                using (var proxy = new ExamExplotionService.ReportManagerClient())
                {
                    return proxy.ReportPlayer(idPlayerReported);
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                throw;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                throw;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
                throw;
            }
        }
    }
}
