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
        public bool ReportPlayer(int idPlayerReported, string gamertag)
        {
            bool reported = false;
            try
            {
                using (var proxy = new ExamExplotionService.ReportManagerClient())
                {
                    reported = proxy.ReportPlayer(idPlayerReported);
                    int playerReportCount = proxy.GetReportCount(idPlayerReported);
                    if (playerReportCount >= 3)
                    {
                        AccountManager accountManager = new AccountManager();
                        accountManager.DeactivateAccount(gamertag);
                    }
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
            return reported;
        }
    }
}
