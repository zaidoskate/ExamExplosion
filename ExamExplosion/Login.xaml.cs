using ExamExplosion.Helpers;
using log4net;
using System;
using System.Collections.Generic;
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
using ExamExplosion.DataValidations;

namespace ExamExplosion
{
    public partial class Login : Page
    {
        private readonly ILog log;
        public Login()
        {
            InitializeComponent();
            gamertagTxtBox.Focus();
            log = LogManager.GetLogger(typeof(App));
        }
        private void CancelLogIn(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }

        private void ValidateLogIn(object sender, RoutedEventArgs e)
        {
            string password;
            string gamertag = gamertagTxtBox.Text;

            if (passwordPswdBox.Visibility == Visibility.Visible)
            {
                password = passwordPswdBox.Password;
            }
            else
            {
                password = passwordTxtBox.Text;
            }
            try
            {
                int idAccount = AccountManager.ValidateCredentials(gamertag, password);
                if (idAccount > 0)
                {
                    if (this.NavigationService != null)
                    {
                        this.NavigationService.Navigate(new HomePage());
                    }
                }
                else if(idAccount == -1)
                {
                    new AlertModal(ExamExplosion.Properties.Resources.loginLblCannotLogin, ExamExplosion.Properties.Resources.loginLblInvalidCredentials).ShowDialog();
                }
                else if (idAccount == -2)
                {
                    new AlertModal(ExamExplosion.Properties.Resources.loginLblCannotLogin, ExamExplosion.Properties.Resources.loginLblAccountInactive).ShowDialog();
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

        private void GamertagTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Contains(" "))
            {
                e.Handled = true;
            }
        }


        private void ValidatePassword(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (passwordBox != null && passwordBox.Password.Contains(" "))
            {
                passwordBox.Password = passwordBox.Password.Replace(" ", string.Empty);
            }
        }

        private void ChangePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (passwordPswdBox.Visibility == Visibility.Visible)
            {
                passwordTxtBox.Text = passwordPswdBox.Password;
                passwordTxtBox.Visibility = Visibility.Visible; 
                passwordPswdBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                passwordPswdBox.Password = passwordTxtBox.Text; 
                passwordPswdBox.Visibility = Visibility.Visible;
                passwordTxtBox.Visibility = Visibility.Collapsed;
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
