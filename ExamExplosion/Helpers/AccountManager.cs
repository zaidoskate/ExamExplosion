using ExamExplosion.ExamExplotionService;
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
            ExamExplotionService.AccountM account = new ExamExplotionService.AccountM();
            account.Gamertag = gamertag;
            account.Password = password;
            bool result = proxy.Login(account);

            if (result)
            {
                LoadActualSession(account);
            }

            return result;
        }

        private static void LoadActualSession(AccountM account)
        {
            int userId = proxy.GetUserIdFromCurrentSession();
            SessionManager.CurrentSession.accountId = userId;
            SessionManager.CurrentSession.userId = userId;  
            SessionManager.CurrentSession.gamertag = account.Gamertag;
        }
    }
}
