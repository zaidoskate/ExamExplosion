using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public class AccountManager
    {
        private static ExamExplotionService.AuthenticationManagerClient proxy = new ExamExplotionService.AuthenticationManagerClient();

        public static bool validateCredentials(string gamertag, string password)
        {
            try
            {
                ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement();
                account.Gamertag = gamertag;
                account.Password = password;
                bool result = proxy.Login(account);

                if (result)
                {
                    LoadActualSession(gamertag);
                }

                return result;
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

        private static void LoadActualSession(string gamertag)
        {
            try
            {
                ExamExplotionService.PlayerManagerClient proxy = new ExamExplotionService.PlayerManagerClient();
                PlayerManagement player = proxy.GetPlayerByGamertag(gamertag);
                SessionManager.CurrentSession.accountId = player.AccountId;
                SessionManager.CurrentSession.userId = player.UserId;  
                SessionManager.CurrentSession.gamertag = gamertag;
                SessionManager.CurrentSession.isGuest = false;
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

        public static bool AddAccount(Account _account)
        {
            AccountManagement account = new AccountManagement();
            account.Name = _account.name;
            account.Password = _account.password;
            account.Email = _account.email;
            account.Lastname = _account.lastname;
            account.Gamertag = _account.gamertag;
            try
            {
                return proxy.AddAccount(account);
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

        public static bool VerifyExistingEmail(string email)
        {
            try
            {
                return proxy.VerifyExistingEmail(email);
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

        public static bool VerifyExistingGamertag(string gamertag)
        {
            try
            {
                return proxy.VerifyExistingGamertag(gamertag);
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

        internal static bool UpdatePassword(string gamertag, string newPassword)
        {
            try
            {
                AccountManagement account = new AccountManagement();
                account.Password = newPassword;
                account.Gamertag = gamertag;
                return proxy.UpdatePassword(account);
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
