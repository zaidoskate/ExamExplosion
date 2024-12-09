using ExamExplosion.DataValidations;
using ExamExplosion.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private bool _newValidPassword = false;
        private readonly ILog log;

        public Settings()
        {
            InitializeComponent();
            InitializeLanguages();
            log = LogManager.GetLogger(typeof(App));
        }

        private void InitializeLanguages()
        {
            if (Thread.CurrentThread.CurrentUICulture.Name == "es-MX")
            {
                languagesCmbBox.SelectedItem = spanishOption;
            }
            else
            {
                languagesCmbBox.SelectedItem = englishOption;
            }
        }

        private void GoHmePage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
            }
        }
        private void ChangePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (currentPasswordTxtBox.Visibility == Visibility.Collapsed)
            {
                currentPasswordTxtBox.Text = currentPasswordPswdBox.Password;
                newPasswordTxtBox.Text = newPasswordPswdBox.Password;

                currentPasswordTxtBox.Visibility = Visibility.Visible;
                currentPasswordPswdBox.Visibility = Visibility.Collapsed;
                newPasswordTxtBox.Visibility = Visibility.Visible;
                newPasswordPswdBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                currentPasswordPswdBox.Password = currentPasswordTxtBox.Text;
                newPasswordPswdBox.Password = newPasswordTxtBox.Text;

                currentPasswordTxtBox.Visibility = Visibility.Collapsed;
                currentPasswordPswdBox.Visibility = Visibility.Visible;
                newPasswordTxtBox.Visibility = Visibility.Collapsed;
                newPasswordPswdBox.Visibility = Visibility.Visible;
            }
        }

        private void ValidateNewPassword(object sender, TextChangedEventArgs e)
        {
            string password = newPasswordTxtBox.Text;
            try
            {
                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidatePasswordLength(password);
                TextValidator.ValidatePassword(password);
                newPasswordMessageTxtBlock.Text = "";
                newPasswordTxtBox.BorderBrush = Brushes.Black;
                newPasswordPswdBox.BorderBrush = Brushes.Black;
                _newValidPassword = true;
            }
            catch (DataValidationException ex)
            {
                newPasswordMessageTxtBlock.Text = ex.Message;
                newPasswordTxtBox.BorderBrush = Brushes.Red;
                newPasswordPswdBox.BorderBrush = Brushes.Red;
                _newValidPassword = false;
                return;
            }
        }
        private void ValidateNewPasswordBox(object sender, RoutedEventArgs e)
        {
            string password = newPasswordPswdBox.Password;
            try
            {
                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidatePasswordLength(password);
                TextValidator.ValidatePassword(password);
                newPasswordMessageTxtBlock.Text = "";
                newPasswordTxtBox.BorderBrush = Brushes.Black;
                newPasswordPswdBox.BorderBrush = Brushes.Black;
                _newValidPassword = true;
            }
            catch (DataValidationException ex)
            {
                newPasswordMessageTxtBlock.Text = ex.Message;
                newPasswordTxtBox.BorderBrush = Brushes.Red;
                newPasswordPswdBox.BorderBrush = Brushes.Red;
                _newValidPassword = false;
                log.Info("Formato de contrasenia incorrecto.", ex);
                return;
            }
        }

        private void LoadLanguage(object sender, RoutedEventArgs e)
        {
            if (languagesCmbBox.SelectedItem == englishOption)
            {
                ChangeLanguage("en");
            }
            else if (languagesCmbBox.SelectedItem == spanishOption)
            {
                ChangeLanguage("es-MX");
            }
        }
        private void ChangeLanguage(string cultureCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Settings());
        }

        private void ValdatePasswords(object sender, RoutedEventArgs e)
        {
            string currentPassword = currentPasswordTxtBox.Text;
            if(currentPasswordTxtBox.Visibility == Visibility.Collapsed )
            {
                currentPassword = currentPasswordPswdBox.Password;
            }
            if (!_newValidPassword || currentPassword.Length == 0) 
            {
                new AlertModal("Datos incompletos", "Corrige los campos que sean necesarios").ShowDialog();
                return;
            }
            else
            {
                string gamertag = SessionManager.CurrentSession.gamertag;
                int currentValidPassword = 0;
                try
                {
                    currentValidPassword = AccountManager.ValidateCredentials(gamertag, currentPassword);
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
                if (currentValidPassword > 0)
                {
                    UpdateNewPassword();
                    clearFields();
                }
                else
                {
                    new AlertModal(ExamExplosion.Properties.Resources.settingsLblCurrentPasswordWrongTitle, ExamExplosion.Properties.Resources.settingsLblCurrentPasswordWrong).ShowDialog();
                    return;
                }
            }
        }

        private void clearFields()
        {
            newPasswordTxtBox.Text = "";
            currentPasswordTxtBox.Text = "";
            currentPasswordPswdBox.Password = "";
            newPasswordPswdBox.Password = "";
        }

        public void UpdateNewPassword()
        {
            string gamertag = SessionManager.CurrentSession.gamertag;
            string newPassword = newPasswordTxtBox.Text;
            if (newPasswordTxtBox.Visibility == Visibility.Collapsed)
            {
                newPassword = newPasswordPswdBox.Password;
            }
            try
            {
                bool passwordUpdated = AccountManager.UpdatePassword(gamertag, newPassword);
                if(passwordUpdated)
                {
                    new AlertModal(ExamExplosion.Properties.Resources.settingsLblPasswordUpdatedTitle, ExamExplosion.Properties.Resources.settingsLblPasswordUpdated).ShowDialog();
                }
                else
                {
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.settingsLblPasswordUpdateError).ShowDialog();
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

        private void NavigateStartPage()
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
    }
}
