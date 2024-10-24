﻿using ExamExplosion.ExamExplotionService;
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
            int accountId = proxy.GetAccountIdFromCurrentSession();
            SessionManager.CurrentSession.accountId = accountId;
            SessionManager.CurrentSession.userId = accountId;  
            SessionManager.CurrentSession.gamertag = account.Gamertag;
        }

        public static bool AddAccount(Account _account)
        {
            AccountM account = new AccountM();
            account.Name = _account.name;
            account.Password = _account.password;
            account.Email = _account.email;
            account.Lastname = _account.lastname;
            account.Gamertag = _account.gamertag;

            return proxy.AddAccount(account);
        }

        public static bool VerifyExistingEmail(string email)
        {
            return proxy.VerifyExistingEmail(email);
        }

        public static bool VerifyExistingGamertag(string gamertag)
        {
            return proxy.VerifyExistingGamertag(gamertag);
        }

        internal static bool UpdatePassword(string gamertag, string newPassword)
        {
            AccountM account = new AccountM();
            account.Password = newPassword;
            account.Gamertag = gamertag;
            return proxy.UpdatePassword(account);
        }
    }
}
