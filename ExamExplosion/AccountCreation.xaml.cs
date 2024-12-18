using ExamExplosion.DataValidations;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
using log4net;
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
using ExamExplosion.Properties;

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
        private readonly bool[] txtBoxesStatus = new bool[6] { false, false, false, false, false, false };
        private readonly ILog log;

        public AccountCreation()
        {
            InitializeComponent();
            log = LogManager.GetLogger(typeof(App));
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
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblDone, ExamExplosion.Properties.Resources.accountCreationLblAccountCreated).ShowDialog();
                }
                else
                {
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.accountCreationCreationError).ShowDialog();
                }
            }
            catch (FaultException faultException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblFaultException).ShowDialog();
                log.Error("Error del servidor (FaultException)", faultException);
                NavigateStartPage();
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblCommunicationException).ShowDialog();
                log.Warn("Problema de comunicación con el servidor", communicationException);
                NavigateStartPage();
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblTimeoutException).ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                NavigateStartPage();
            }
        }

        private void NavigateStartPage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
        private void ChangePasswordVisibilityBtn_Click(object sender, RoutedEventArgs e)
        {
            if (passwordTxtBox.Visibility == Visibility.Collapsed)
            {
                passwordTxtBox.Text = passwordPswdBox.Password;
                repeatPasswordTxtBox.Text = repeatPasswordPswdBox.Password;

                passwordTxtBox.Visibility = Visibility.Visible;
                passwordPswdBox.Visibility = Visibility.Collapsed;
                repeatPasswordTxtBox.Visibility = Visibility.Visible;
                repeatPasswordPswdBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                passwordPswdBox.Password = passwordTxtBox.Text;
                repeatPasswordPswdBox.Password = repeatPasswordTxtBox.Text;

                passwordTxtBox.Visibility = Visibility.Collapsed;
                passwordPswdBox.Visibility = Visibility.Visible;
                repeatPasswordTxtBox.Visibility = Visibility.Collapsed;
                repeatPasswordPswdBox.Visibility = Visibility.Visible;
            }
        }

        private void ValidateName(object sender, TextChangedEventArgs e)
        {
            name = nameTxtBox.Text;
            if (name.Length >= 50)
            {
                name = name.Substring(0, 50);
                nameTxtBox.Text = name;
                nameTxtBox.SelectionStart = 50;
            }
            try
            {
                TextValidator.ValidateNotBlanks(name);
                TextValidator.ValidateNameFormat(name);
                nameErrorMessageLbl.Content = "";
                nameTxtBox.BorderBrush = Brushes.Black;
                txtBoxesStatus[0] = true;
            }
            catch(DataValidationException dataValidationException)
            {
                nameErrorMessageLbl.Content = dataValidationException.Message;
                nameTxtBox.BorderBrush = Brushes.Red;
                txtBoxesStatus[0] = false;
            }
        }

        private void ValidateLastname(object sender, TextChangedEventArgs e)
        {
            lastname = lastnameTxtBox.Text;
            lastname = lastnameTxtBox.Text;
            if (lastname.Length >= 50)
            {
                lastname = lastname.Substring(0, 50);
                lastnameTxtBox.Text = lastname;
                lastnameTxtBox.SelectionStart = 50;
            }
            try
            {
                TextValidator.ValidateNotBlanks(lastname);
                TextValidator.ValidateNameFormat(lastname);
                lastnameErrorMessageLbl.Content = "";
                lastnameTxtBox.BorderBrush = Brushes.Black;
                txtBoxesStatus[1] = true;
            }
            catch (DataValidationException dataValidationException)
            {
                lastnameErrorMessageLbl.Content = dataValidationException.Message;
                lastnameTxtBox.BorderBrush = Brushes.Red;
                txtBoxesStatus[1] = false;
            }
        }
        private void ValidateEmail(object sender, TextChangedEventArgs e)
        {
            email = emailTxtBox.Text;
            email = emailTxtBox.Text;
            if (email.Length >= 50)
            {
                email = email.Substring(0, 50);
                emailTxtBox.Text = email;
                emailTxtBox.SelectionStart = 50;
            }
            try
            {
                TextValidator.ValidateNotBlanks(email);
                TextValidator.ValidateEmailFormat(email);
                emailErrorMessageLbl.Content = "";
                emailTxtBox.BorderBrush = Brushes.Black;
                txtBoxesStatus[2] = true;
            }
            catch (DataValidationException dataValidationException)
            {
                emailErrorMessageLbl.Content = dataValidationException.Message;
                emailTxtBox.BorderBrush = Brushes.Red;
                txtBoxesStatus[2] = false;
            }
        }
        private void ValidateGamertag(object sender, TextChangedEventArgs e)
        {
            gamertag = gamertagTxtBox.Text;
            gamertag = gamertagTxtBox.Text;
            gamertag = gamertagTxtBox.Text;
            if (gamertag.Length >= 10)
            {
                gamertag = gamertag.Substring(0, 10);
                gamertagTxtBox.Text = gamertag;
                gamertagTxtBox.SelectionStart = 20;
            }
            try
            { 
                TextValidator.ValidateNotBlanks(gamertag);
                TextValidator.ValidateGamertagFormat(gamertag);
                TextValidator.ValidateGamertagNotGuest(gamertag);
                TextValidator.ValidateGamertagFirstLetter(gamertag);
                gamertagErrorMessageLbl.Content = "";
                gamertagTxtBox.BorderBrush = Brushes.Black;
                txtBoxesStatus[3] = true;
            }
            catch (DataValidationException dataValidationException)
            {
                gamertagErrorMessageLbl.Content = dataValidationException.Message;
                gamertagTxtBox.BorderBrush = Brushes.Red;
                txtBoxesStatus[3] = false;
            }
        }
        private void ValidatePassword(object sender, TextChangedEventArgs e)
        {
            password = passwordTxtBox.Text;
            ValidateRepeatPassword(null, null);
            ValidateRepeatPasswordBox(null, null);
            try
            {
                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidatePasswordLength(password);
                TextValidator.ValidatePassword(password);
                passwordErrorMessageLbl.Content = "";
                passwordTxtBox.BorderBrush = Brushes.Black;
                passwordPswdBox.BorderBrush = Brushes.Black;
                txtBoxesStatus[4] = true;
            }
            catch (DataValidationException dataValidationException)
            {
                passwordErrorMessageLbl.Content = dataValidationException.Message;
                passwordTxtBox.BorderBrush = Brushes.Red;
                passwordPswdBox.BorderBrush = Brushes.Red;
                txtBoxesStatus[4] = false;
            }
        }
        private void ValidatePasswordBox(object sender, RoutedEventArgs e)
        {
            password = passwordPswdBox.Password;
            ValidateRepeatPassword(null, null);
            ValidateRepeatPasswordBox(null, null);
            try
            {
                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidatePasswordLength(password);
                TextValidator.ValidatePassword(password);
                passwordErrorMessageLbl.Content = "";
                passwordTxtBox.BorderBrush = Brushes.Black;
                passwordPswdBox.BorderBrush = Brushes.Black;
                txtBoxesStatus[4] = true;
            }
            catch (DataValidationException dataValidationException)
            {
                passwordErrorMessageLbl.Content = dataValidationException.Message;
                passwordTxtBox.BorderBrush = Brushes.Red;
                passwordPswdBox.BorderBrush = Brushes.Red;
                txtBoxesStatus[4] = false;
            }
        }
        private void ValidateRepeatPassword(object sender, TextChangedEventArgs e)
        {
            repeatPassword = repeatPasswordTxtBox.Text;
            try
            {
                TextValidator.ValidateNotBlanks(repeatPassword);
                TextValidator.ValidateRepeatResponse(repeatPassword, password);
                repeatPasswordErrorMessageLbl.Content = "";
                repeatPasswordTxtBox.BorderBrush = Brushes.Black;
                repeatPasswordPswdBox.BorderBrush = Brushes.Black;
                txtBoxesStatus[5] = true;
            }
            catch (DataValidationException dataValidationException)
            {
                repeatPasswordErrorMessageLbl.Content = dataValidationException.Message;
                repeatPasswordTxtBox.BorderBrush = Brushes.Red;
                repeatPasswordPswdBox.BorderBrush = Brushes.Red;
                txtBoxesStatus[5] = false;
            }
        }
        private void ValidateRepeatPasswordBox(object sender, RoutedEventArgs e)
        {
            repeatPassword = repeatPasswordPswdBox.Password;
            try
            {
                TextValidator.ValidateNotBlanks(repeatPassword);
                TextValidator.ValidateRepeatResponse(repeatPassword, password);
                repeatPasswordErrorMessageLbl.Content = "";
                repeatPasswordTxtBox.BorderBrush = Brushes.Black;
                repeatPasswordPswdBox.BorderBrush = Brushes.Black;
                txtBoxesStatus[5] = true;
            }
            catch (DataValidationException dataValidationException)
            {
                repeatPasswordErrorMessageLbl.Content = dataValidationException.Message;
                repeatPasswordTxtBox.BorderBrush = Brushes.Red;
                repeatPasswordPswdBox.BorderBrush = Brushes.Red;
                txtBoxesStatus[5] = false;
            }
        }

        private void VerifyDataBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach(bool value in txtBoxesStatus)
            {
                if(!value)
                {
                    log.Info("Datos de registro de usuario incompletos o con errores");
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.accountCreationLblMissingInformation).ShowDialog();
                    return;
                }
            }
            try
            {
                int gamertagExists = ExistingDataValidator.ValidateExistingGamertag(gamertag);
                int emailExists = ExistingDataValidator.ValidateExistingEmail(email);
                string message = null;
                if (gamertagExists != 2 || emailExists != 2)
                {
                    if (gamertagExists == 1 && emailExists == 1)
                    {
                        message = ExamExplosion.Properties.Resources.accountCreationEmailAndGamertagTaken;
                    }
                    else if(emailExists == 1)
                    {
                        message = ExamExplosion.Properties.Resources.accountCreationLblLinkedEmail;
                    }
                    else if (gamertagExists == 1)
                    {
                        message = ExamExplosion.Properties.Resources.accountCreationLblExistingGamertag;
                    }
                    else
                    {
                        SendCode();
                        return;
                    }
                    new AlertModal(ExamExplosion.Properties.Resources.accountCreationExistingData, message).ShowDialog();
                }
                else
                {
                    message = ExamExplosion.Properties.Resources.accountCreationCreationError;
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, message).ShowDialog();
                }
            }
            catch (FaultException faultException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblFaultException).ShowDialog();
                log.Error("Error del servidor (FaultException)", faultException);
                NavigateStartPage();
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblCommunicationException).ShowDialog();
                log.Warn("Problema de comunicación con el servidor", communicationException);
                NavigateStartPage();
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblTimeoutException).ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                NavigateStartPage();
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
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.accountCreationLblEmailError).ShowDialog();
            }
        }
        private void NavigateStartPage()
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
    }
}
