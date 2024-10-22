using ExamExplosion.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.DataValidations
{
    public class ExistingDataValidator
    {
        public static bool ValidateExistingGamertag(string gamertag)
        {
            return AccountManager.VerifyExistingGamertag(gamertag);
        }
        public static bool ValidateExistingEmail(string email)
        {
            return AccountManager.VerifyExistingEmail(email);
        }

    }
}
