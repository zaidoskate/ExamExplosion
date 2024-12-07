using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using log4net;

namespace ExamExplosion.Helpers
{
    public static class EmailVerificationCode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public static string GenerateCode()
        {
            const string letters = "abcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            char[] code = new char[4];

            for (int i = 0; i < 4; i++)
            {
                code[i] = letters[random.Next(letters.Length)];
            }

            return new string(code);
        }
        public static bool SendEmail(string email, string code)
        {
            bool emailSent = false;
            try
            {
                string smtpUsername = Environment.GetEnvironmentVariable("SMTP_USERNAME");
                string smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = true
                };
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(email),
                    Subject = "Código de verificación",
                    Body = $"El código de verificación para tu cuenta de Exam Explosion es: {code}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(email);

                client.Send(mailMessage);
                emailSent = true;
            }
            catch (SmtpFailedRecipientException smtpFailRecipientException)
            {
                log.Warn("Correo no existente", smtpFailRecipientException);
            }
            catch (SmtpException smtpException)
            {
                log.Warn(smtpException);
            }
            return emailSent;
        }
    }
}
