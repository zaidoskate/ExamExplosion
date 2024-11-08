using ExamExplosion.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.DataValidations
{
    public class ExistingDataValidator
    {
        public static bool ValidateExistingGamertag(string gamertag)
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
        public static bool ValidateExistingEmail(string email)
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
