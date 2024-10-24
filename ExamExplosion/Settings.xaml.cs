using ExamExplosion.DataValidations;
using ExamExplosion.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        private bool newValidPassword = false;
        
        public Settings()
        {
            InitializeComponent();
            InitializeLanguages();
        }

        private void InitializeLanguages()
        {
            if (Thread.CurrentThread.CurrentUICulture.Name == "es-MX")
            {
                CmbBoxLanguages.SelectedItem = SpanishOption;
            }
            else
            {
                CmbBoxLanguages.SelectedItem = EnglishOption;
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
            if (txtBoxCurrentPsswd.Visibility == Visibility.Collapsed)
            {
                txtBoxCurrentPsswd.Text = pswdBoxCurrentPsswd.Password;
                txtBoxNewPsswd.Text = pswdBoxNewPsswd.Password;

                txtBoxCurrentPsswd.Visibility = Visibility.Visible;
                pswdBoxCurrentPsswd.Visibility = Visibility.Collapsed;
                txtBoxNewPsswd.Visibility = Visibility.Visible;
                pswdBoxNewPsswd.Visibility = Visibility.Collapsed;
            }
            else
            {
                pswdBoxCurrentPsswd.Password = txtBoxCurrentPsswd.Text;
                pswdBoxNewPsswd.Password = txtBoxNewPsswd.Text;

                txtBoxCurrentPsswd.Visibility = Visibility.Collapsed;
                pswdBoxCurrentPsswd.Visibility = Visibility.Visible;
                txtBoxNewPsswd.Visibility = Visibility.Collapsed;
                pswdBoxNewPsswd.Visibility = Visibility.Visible;
            }
        }

        private void ValidateNewPassword(object sender, TextChangedEventArgs e)
        {
            string password = txtBoxNewPsswd.Text;
            try
            {
                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidatePasswordLength(password);
                TextValidator.ValidatePassword(password);
                txtBlockNewPasswordMessage.Text = "";
                txtBoxNewPsswd.BorderBrush = Brushes.Black;
                pswdBoxNewPsswd.BorderBrush = Brushes.Black;
                newValidPassword = true;
            }
            catch (DataValidationException ex)
            {
                txtBlockNewPasswordMessage.Text = ex.Message;
                txtBoxNewPsswd.BorderBrush = Brushes.Red;
                pswdBoxNewPsswd.BorderBrush = Brushes.Red;
                newValidPassword = false;
                return;
            }
        }
        private void ValidateNewPasswordBox(object sender, RoutedEventArgs e)
        {
            string password = pswdBoxNewPsswd.Password;
            try
            {
                TextValidator.ValidateNotBlanks(password);
                TextValidator.ValidatePasswordLength(password);
                TextValidator.ValidatePassword(password);
                txtBlockNewPasswordMessage.Text = "";
                txtBoxNewPsswd.BorderBrush = Brushes.Black;
                pswdBoxNewPsswd.BorderBrush = Brushes.Black;
                newValidPassword = true;
            }
            catch (DataValidationException ex)
            {
                txtBlockNewPasswordMessage.Text = ex.Message;
                txtBoxNewPsswd.BorderBrush = Brushes.Red;
                pswdBoxNewPsswd.BorderBrush = Brushes.Red;
                newValidPassword = false;
                return;
            }
        }

        private void LoadLanguage(object sender, RoutedEventArgs e)
        {
            if (CmbBoxLanguages.SelectedItem == EnglishOption)
            {
                ChangeLanguage("en");
            }
            else if (CmbBoxLanguages.SelectedItem == SpanishOption)
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
            string currentPassword = txtBoxCurrentPsswd.Text;
            if(txtBoxCurrentPsswd.Visibility == Visibility.Collapsed )
            {
                currentPassword = pswdBoxCurrentPsswd.Password;
            }
            if (!newValidPassword || currentPassword.Length == 0) 
            {
                new AlertModal("Datos incompletos", "Corrige los campos que sean necesarios").ShowDialog();
                return;
            }
            else
            {
                string gamertag = SessionManager.CurrentSession.gamertag;
                bool currentValidPassword = AccountManager.validateCredentials(gamertag, currentPassword);
                if(currentValidPassword)
                {
                    UpdateNewPassword();
                }
                else
                {
                    new AlertModal("Contrasenia incorrecta", "La contrasenia ingresada no coincide con tu contrasenia actual.").ShowDialog();
                    return;
                }
            }
        }

        public void UpdateNewPassword()
        {
            string gamertag = SessionManager.CurrentSession.gamertag;
            string newPassword = txtBoxNewPsswd.Text;
            if (txtBoxNewPsswd.Visibility == Visibility.Collapsed)
            {
                newPassword = pswdBoxNewPsswd.Password;
            }
            bool passwordUpdated = AccountManager.UpdatePassword(gamertag, newPassword);
            if(passwordUpdated)
            {
                new AlertModal("Contrasenia actualizada", "Se ha actualzado la contrasenia de tu cuenta con exito.").ShowDialog();
            }
            else
            {
                new AlertModal("Error", "No se ha actualzado la contrasenia. Intentalo mas tarde.").ShowDialog();
            }
        }
    }
}
