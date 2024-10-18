using ExamExplosion.DataValidations;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamExplosion
{
    /// <summary>
    /// Lógica de interacción para AccountCreation.xaml
    /// </summary>
    public partial class AccountCreation : Page
    {
        private string name;
        string lastname;
        string gamertag;
        string email;
        string password;
        string repeatPassword;
        public AccountCreation()
        {
            InitializeComponent();
        }

        private void ValidateEmail(string email)
        {
            string code = EmailVerificationCode.GenerateCode();
            bool result = EmailVerificationCode.SendEmail(email, code);
            if(result)
            {
                EmailVerification emailVerificationWindow = new EmailVerification(code);
                emailVerificationWindow.ShowDialog();
                bool emailVerifed = emailVerificationWindow.EmailVerified;
                
                if(emailVerifed)
                {
                    CreateAccount();
                }
            }
            else
            {
                new AlertModal("Error al enviar el correo","Revisa tu conexion a internet o proporciona un correo valido").ShowDialog();
            }
        }

        private void CreateAccount()
        {
            Account account = new Account();
            account.email = email;
            account.password = password;
            account.lastname = lastname;
            account.name = name;
            account.gamertag = gamertag;

            bool accountAdded = AccountManager.AddAccount(account);
            if (accountAdded)
            {
                new AlertModal("Cuenta creada", "Se ha creado la nueva cuenta con exito.").ShowDialog();
                if (this.NavigationService != null)
                {
                    this.NavigationService.Navigate(new StartPage());
                }
            }
            else
            {
                new AlertModal("Error", "No he podido crear la cuenta, intentalo mas tarde.").ShowDialog();
            }
        }

        private void CancelAccountCreation(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
        private void ChangePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (txtBoxPassword.Visibility == Visibility.Collapsed)
            {
                txtBoxPassword.Text = pswdBoxPassword.Password;
                txtBoxRepeatPassword.Text = pswdBoxRepeatPassword.Password;

                txtBoxPassword.Visibility = Visibility.Visible;
                pswdBoxPassword.Visibility = Visibility.Collapsed;
                txtBoxRepeatPassword.Visibility = Visibility.Visible;
                pswdBoxRepeatPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                pswdBoxPassword.Password = txtBoxPassword.Text;
                pswdBoxRepeatPassword.Password = txtBoxRepeatPassword.Text;

                txtBoxPassword.Visibility = Visibility.Collapsed;
                pswdBoxPassword.Visibility = Visibility.Visible;
                txtBoxRepeatPassword.Visibility = Visibility.Collapsed;
                pswdBoxRepeatPassword.Visibility = Visibility.Visible;
            }
        }

        private void ValidateData(object sender, RoutedEventArgs e)
        {
            name = txtBoxName.Text;
            lastname = txtBoxLastname.Text;
            gamertag = txtBoxGamertag.Text;
            email = txtBoxEmail.Text;
            password = txtBoxPassword.Text;
            repeatPassword = txtBoxRepeatPassword.Text;
            if (txtBoxPassword.Visibility == Visibility.Collapsed)
            {
                password = pswdBoxPassword.Password;
                repeatPassword = pswdBoxRepeatPassword.Password;
            }
            if(password != repeatPassword)
            {
                new AlertModal("Contraseñas no coinciden","Asegurate de repetir la misma contraseña.").ShowDialog();
                return;
            }
            try
            {
                TextValidator.ValidateNotBlanks(name);
                TextValidator.ValidateLenght(name, 50);
                TextValidator.ValidateNameFormat(name);

                TextValidator.ValidateNotBlanks(lastname);
                TextValidator.ValidateLenght(lastname, 50);
                TextValidator.ValidateNameFormat(lastname);

                TextValidator.ValidateNotBlanks(gamertag);
                TextValidator.ValidateLenght(gamertag, 50);
                TextValidator.ValidateGamertagFormat(gamertag);

                TextValidator.ValidateNotBlanks(email);
                TextValidator.ValidateLenght(email, 50);
                TextValidator.ValidateEmailFormat(email);

                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidateLenght(password, 50);
            }
            catch(DataValidationException ex)
            {
                new AlertModal("Error", ex.Message.ToString()).ShowDialog();
                return;
            }

            ValidateEmail(email);
        }
    }
}
