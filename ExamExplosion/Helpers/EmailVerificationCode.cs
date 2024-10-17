using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public class EmailVerificationCode
    {
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
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("coillogs4@gmail.com", "fwspilsgfxmeoyna"),
                    EnableSsl = true
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(email),
                    Subject = "Código de verificación",
                    Body = $"Tu código de verificación es: {code}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(email);

                client.Send(mailMessage);
            }
            catch (SmtpFailedRecipientException ex)
            {
                return false;
            }
            catch (SmtpException ex)
            {
                return false;
            }
            return true;
        }
    }
}
