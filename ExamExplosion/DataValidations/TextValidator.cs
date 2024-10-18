using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ExamExplosion.DataValidations
{
    public class TextValidator
    {
        public static void ValidateLenght(string text, int maxLenght)
        {
            if(text.Length > maxLenght)
            {
                throw new DataValidationException($"El texto ingresado para el campo excede el tamaño límite. ({maxLenght} caracteres)");
            }
        }
        public static void ValidateEmailFormat(string text)
        {
            string pattern = @"^[a-z-0-9._%+-]+@[a-z-0-9.-]+\\.[a-zA-Z]{2,}$";
            bool result = Regex.IsMatch(pattern, text);
            if(result == false)
            {
                throw new DataValidationException("El correo no tiene el formato esperado (correo@ejemplo.com)");
            }
        }
        public static void ValidateNameFormat(string text)
        {
            string pattern = @"^[A-ZÀ-ÿ][a-zA-ZÀ-ÿ.,' ]+$";
            bool result = Regex.IsMatch(pattern, text);
            if (result == false)
            {
                throw new DataValidationException("Formato de nombre incorrecto. El nombre debe comenzar con mayuscula y sin caracteres especiales");
            }
        }
        public static void ValidateGamertagFormat(string text)
        {
            string pattern = @"^[a-zA-Z1-9-]+$";
            bool result = Regex.IsMatch(pattern, text);
            if (result == false)
            {
                throw new DataValidationException("El gamertag no tiene el formato adecuado. No debe tener espacios ni caracteres especiales.");
            }
        }
        public static void ValidateNotBlanks(string text)
        {
            bool result = String.IsNullOrEmpty(text);
            if (result == true)
            {
                throw new DataValidationException("El texto ingresado esta vacio.");
            }
        }
    }
}
