using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
                throw new DataValidationException("Correo invalido");
            }
        }
        public static void ValidateNameFormat(string text)
        {
            string pattern = @"^[A-ZÀ-ÿ][a-zA-ZÀ-ÿ'., ]*$";
            bool result = Regex.IsMatch(text, pattern);
            if (!result)
            {
                throw new DataValidationException("Formato de nombre incorrecto.");
            }
        }
        public static void ValidateGamertagFormat(string text)
        {
            string pattern = @"^[a-zA-Z0-9-]+$";
            bool result = Regex.IsMatch(text, pattern);
            if (!result)
            {
                throw new DataValidationException("Solo letras y numeros.");
            }
        }
        public static void ValidateGamertagFirstLetter(string text)
        {
            string pattern = @"^\d";
            bool result = Regex.IsMatch(text, pattern);
            if (result)
            {
                throw new DataValidationException("Debe empezar con letra.");
            }
        }
        public static void ValidateNotBlanks(string text)
        {
            bool result = String.IsNullOrEmpty(text);
            if (result)
            {
                throw new DataValidationException("Este es un campo obligatorio.");
            }
        }
        public static void ValidatePassword(string text)
        {
            string pattern = @"^[a-zA-Z0-9#$%&!]*$";
            bool result = Regex.IsMatch(text, pattern);
            if (!result)
            {
                throw new DataValidationException("Caracteres especiales permitidos: #,$,%,&,!");
            }
        }
        public static void ValidatePasswordLength(string text)
        {
            string pattern = @"^.{8,20}$";
            bool result = Regex.IsMatch(text, pattern);
            if (!result)
            {
                throw new DataValidationException("De 8 a 20 caracteres");
            }
        }

        public static void ValidateRepeatResponse(string repeatPassword, string password)
        {
            if(repeatPassword != password)
            {
                throw new DataValidationException("No coinciden.");
            }
        }
    }
}
