using ExamExplosion.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.DataValidations
{
    public static class ExistingDataValidator
    {
        public static int ValidateExistingGamertag(string gamertag)
        {
            try
            {
                return AccountManager.VerifyExistingGamertag(gamertag);
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
        public static int ValidateExistingEmail(string email)
        {
            try
            {
                return AccountManager.VerifyExistingEmail(email);

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

    }
}
