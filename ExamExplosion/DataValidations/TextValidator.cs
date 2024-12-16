using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ExamExplosion.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace ExamExplosion.DataValidations
{
    public static class TextValidator
    {
        public static void ValidateEmailFormat(string text)
        {
            string pattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-zA-Z]{2,}$";
            bool result = Regex.IsMatch(text, pattern);
            if(!result)
            {
                throw new DataValidationException(Resources.accountCreationLblInvalidEmail);
            }
        }
        public static void ValidateNameFormat(string text)
        {
            string pattern = @"^[A-ZÀ-ÿ][a-zA-ZÀ-ÿ'., ]*$";
            bool result = Regex.IsMatch(text, pattern);
            if (!result)
            {
                throw new DataValidationException(Resources.accountCreationLblInvalidName);
            }
        }
        public static void ValidateGamertagFormat(string text)
        {
            string pattern = @"^[a-zA-Z0-9-]+$";
            bool result = Regex.IsMatch(text, pattern);
            if (!result)
            {
                throw new DataValidationException(Resources.accountCreationLblInvalidGamertag);
            }
        }

        public static void ValidateGamertagNotGuest(string text)
        {
            string textToValidate = text.ToUpper();
            bool result = textToValidate.StartsWith("GUEST");
            if (result)
            {
                throw new DataValidationException(Resources.accountCreationLblGamertagNotGuest);
            }
        }
        public static void ValidateGamertagFirstLetter(string text)
        {
            string pattern = @"^\d";
            bool result = Regex.IsMatch(text, pattern);
            if (result)
            {
                throw new DataValidationException(Resources.accountCreationLblInvalidGamertagInitial);
            }
        }
        public static void ValidateNotBlanks(string text)
        {
            bool result = String.IsNullOrWhiteSpace(text);
            if (result)
            {
                throw new DataValidationException(Resources.globalLblNotBlanks);
            }
        }
        public static void ValidatePassword(string text)
        {
            string pattern = @"^[a-zA-Z0-9#$%&!]*$";
            bool result = Regex.IsMatch(text, pattern);
            if (!result)
            {
                throw new DataValidationException(Resources.globalLblSpecialCharacters);
            }
        }
        public static void ValidatePasswordLength(string text)
        {
            string pattern = @"^.{8,20}$";
            bool result = Regex.IsMatch(text, pattern);
            if (!result)
            {
                throw new DataValidationException(Resources.globalLblPasswordLength);
            }
        }

        public static void ValidateRepeatResponse(string repeatPassword, string password)
        {
            if(repeatPassword != password)
            {
                throw new DataValidationException(Resources.globalLblPasswordCoincidence);
            }
        }

        public static void ValidateChatFormat(string message)
        {
            string pattern = @"^[a-zA-Z0-9\s.,!?;:'""()\-]+$";
            bool result = Regex.IsMatch(message, pattern);
            if (!result)
            {
                throw new DataValidationException();
            }
        }
    }
}
