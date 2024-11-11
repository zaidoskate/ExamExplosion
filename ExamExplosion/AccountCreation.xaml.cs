using ExamExplosion.DataValidations;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.ServiceModel;
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
        bool[] txtBoxesStatus = new bool[6] { false, false, false, false, false, false };

        public AccountCreation()
        {
            InitializeComponent();

        }
        private void CreateAccount()
        {
            Account account = new Account();
            account.email = email;
            account.password = password;
            account.lastname = lastname;
            account.name = name;
            account.gamertag = gamertag;

            try
            {
                bool accountAdded = AccountManager.AddAccount(account);
                if (accountAdded)
                {
                    if (this.NavigationService != null)
                    {
                        this.NavigationService.Navigate(new StartPage());
                    }
                    new AlertModal("Cuenta creada", "Se ha creado la nueva cuenta con exito.").ShowDialog();
                }
                else
                {
                    new AlertModal("Error", "No he podido crear la cuenta, intentalo mas tarde.").ShowDialog();
                }
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                throw timeoutException;
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

        private void ValidateName(object sender, TextChangedEventArgs e)
        {
            name = txtBoxName.Text;
            if (name.Length >= 50)
            {
                name = name.Substring(0, 50);
                txtBoxName.Text = name;
                txtBoxName.SelectionStart = 50;
            }
            try
            {
                TextValidator.ValidateNotBlanks(name);
                TextValidator.ValidateNameFormat(name);
                lblNameErrorMessage.Content = "";
                txtBoxName.BorderBrush = Brushes.Black;
                txtBoxesStatus[0] = true;
            }
            catch(DataValidationException ex)
            {
                lblNameErrorMessage.Content = ex.Message;
                txtBoxName.BorderBrush = Brushes.Red;
                txtBoxesStatus[0] = false;
                return;
            }
        }

        private void ValidateLastname(object sender, TextChangedEventArgs e)
        {
            lastname = txtBoxLastname.Text;
            lastname = txtBoxLastname.Text;
            if (lastname.Length >= 50)
            {
                lastname = lastname.Substring(0, 50);
                txtBoxLastname.Text = lastname;
                txtBoxLastname.SelectionStart = 50;
            }
            try
            {
                TextValidator.ValidateNotBlanks(lastname);
                TextValidator.ValidateNameFormat(lastname);
                lblLastnameErrorMessage.Content = "";
                txtBoxLastname.BorderBrush = Brushes.Black;
                txtBoxesStatus[1] = true;
            }
            catch (DataValidationException ex)
            {
                lblLastnameErrorMessage.Content = ex.Message;
                txtBoxLastname.BorderBrush = Brushes.Red;
                txtBoxesStatus[1] = false;
                return;
            }
        }
        private void ValidateEmail(object sender, TextChangedEventArgs e)
        {
            email = txtBoxEmail.Text;
            email = txtBoxEmail.Text;
            if (email.Length >= 50)
            {
                email = email.Substring(0, 50);
                txtBoxEmail.Text = email;
                txtBoxEmail.SelectionStart = 50;
            }
            try
            {
                TextValidator.ValidateNotBlanks(email);
                TextValidator.ValidateEmailFormat(email);
                lblEmailErrorMessage.Content = "";
                txtBoxEmail.BorderBrush = Brushes.Black;
                txtBoxesStatus[2] = true;
            }
            catch (DataValidationException ex)
            {
                lblEmailErrorMessage.Content = ex.Message;
                txtBoxEmail.BorderBrush = Brushes.Red;
                txtBoxesStatus[2] = false;
                return;
            }
        }
        private void ValidateGamertag(object sender, TextChangedEventArgs e)
        {
            gamertag = txtBoxGamertag.Text;
            gamertag = txtBoxGamertag.Text;
            gamertag = txtBoxGamertag.Text;
            if (gamertag.Length >= 50)
            {
                gamertag = gamertag.Substring(0, 50);
                txtBoxGamertag.Text = gamertag;
                txtBoxGamertag.SelectionStart = 50;
            }
            try
            { 
                TextValidator.ValidateNotBlanks(gamertag);
                TextValidator.ValidateGamertagFormat(gamertag);
                TextValidator.ValidateGamertagFirstLetter(gamertag);
                lblGamertagErrorMessage.Content = "";
                txtBoxGamertag.BorderBrush = Brushes.Black;
                txtBoxesStatus[3] = true;
            }
            catch (DataValidationException ex)
            {
                lblGamertagErrorMessage.Content = ex.Message;
                txtBoxGamertag.BorderBrush = Brushes.Red;
                txtBoxesStatus[3] = false;
                return;
            }
        }
        private void ValidatePassword(object sender, TextChangedEventArgs e)
        {
            password = txtBoxPassword.Text;
            ValidateRepeatPassword(null, null);
            ValidateRepeatPasswordBox(null, null);
            try
            {
                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidatePasswordLength(password);
                TextValidator.ValidatePassword(password);
                lblPasswordErrorMessage.Text = "";
                txtBoxPassword.BorderBrush = Brushes.Black;
                pswdBoxPassword.BorderBrush = Brushes.Black;
                txtBoxesStatus[4] = true;
            }
            catch (DataValidationException ex)
            {
                lblPasswordErrorMessage.Text = ex.Message;
                txtBoxPassword.BorderBrush = Brushes.Red;
                pswdBoxPassword.BorderBrush = Brushes.Red;
                txtBoxesStatus[4] = false;
                return;
            }
        }
        private void ValidatePasswordBox(object sender, RoutedEventArgs e)
        {
            password = pswdBoxPassword.Password;
            ValidateRepeatPassword(null, null);
            ValidateRepeatPasswordBox(null, null);
            try
            {
                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidatePasswordLength(password);
                TextValidator.ValidatePassword(password);
                lblPasswordErrorMessage.Text = "";
                txtBoxPassword.BorderBrush = Brushes.Black;
                pswdBoxPassword.BorderBrush = Brushes.Black;
                txtBoxesStatus[4] = true;
            }
            catch (DataValidationException ex)
            {
                lblPasswordErrorMessage.Text = ex.Message;
                txtBoxPassword.BorderBrush = Brushes.Red;
                pswdBoxPassword.BorderBrush = Brushes.Red;
                txtBoxesStatus[4] = false;
                return;
            }
        }
        private void ValidateRepeatPassword(object sender, TextChangedEventArgs e)
        {
            repeatPassword = txtBoxRepeatPassword.Text;
            try
            {
                TextValidator.ValidateNotBlanks(repeatPassword);
                TextValidator.ValidateRepeatResponse(repeatPassword, password);
                lblRepeatPasswordErrorMessage.Content = "";
                txtBoxRepeatPassword.BorderBrush = Brushes.Black;
                pswdBoxRepeatPassword.BorderBrush = Brushes.Black;
                txtBoxesStatus[5] = true;
            }
            catch (DataValidationException ex)
            {
                lblRepeatPasswordErrorMessage.Content = ex.Message;
                txtBoxRepeatPassword.BorderBrush = Brushes.Red;
                pswdBoxRepeatPassword.BorderBrush = Brushes.Red;
                txtBoxesStatus[5] = false;
                return;
            }
        }
        private void ValidateRepeatPasswordBox(object sender, RoutedEventArgs e)
        {
            repeatPassword = pswdBoxRepeatPassword.Password;
            try
            {
                TextValidator.ValidateNotBlanks(repeatPassword);
                TextValidator.ValidateRepeatResponse(repeatPassword, password);
                lblRepeatPasswordErrorMessage.Content = "";
                txtBoxRepeatPassword.BorderBrush = Brushes.Black;
                pswdBoxRepeatPassword.BorderBrush = Brushes.Black;
                txtBoxesStatus[5] = true;
            }
            catch (DataValidationException ex)
            {
                lblRepeatPasswordErrorMessage.Content = ex.Message;
                txtBoxRepeatPassword.BorderBrush = Brushes.Red;
                pswdBoxRepeatPassword.BorderBrush = Brushes.Red;
                txtBoxesStatus[5] = false;
                return;
            }
        }

        private void VerifyData(object sender, RoutedEventArgs e)
        {
            foreach(bool value in txtBoxesStatus)
            {
                if(value == false)
                {
                    new AlertModal("Datos incompletos","Corrige los campos que sean necesarios").ShowDialog();
                    return;
                }
            }
            try
            {
                bool gamertagExists = ExistingDataValidator.ValidateExistingGamertag(gamertag);
                bool emailExists = ExistingDataValidator.ValidateExistingEmail(email);
                string message = null;
                if(gamertagExists && emailExists )
                {
                    message = "El gamertag y correo proporcionados ya existen con otra cuenta.";
                }
                else if(emailExists)
                {
                    message = "El correo ya se encuentra asociado a otra cuenta.";
                }
                else if (gamertagExists)
                {
                    message = "El gamertag proporcionado ya está en uso. Prueba con otro";
                }
                else
                {
                    SendCode();
                    return;
                }
                new AlertModal("Datos existentes", message).ShowDialog();
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                throw timeoutException;
            }
        }
        private void SendCode()
        {
            string code = EmailVerificationCode.GenerateCode();
            bool result = EmailVerificationCode.SendEmail(email, code);
            if (result)
            {
                EmailVerification emailVerificationWindow = new EmailVerification(code);
                emailVerificationWindow.ShowDialog();
                bool emailVerifed = emailVerificationWindow.EmailVerified;

                if (emailVerifed)
                {
                    CreateAccount();
                }
            }
            else
            {
                new AlertModal("Error al enviar el correo", "Revisa tu conexion a internet o proporciona un correo existente").ShowDialog();
            }
        }
    }
}
