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

namespace ExamExplosion
{
    public partial class Login : Page
    {
        private ILog log;
        public Login()
        {
            InitializeComponent();
            txtBoxGamertag.Focus();
            ILog log = LogManager.GetLogger(typeof(App));
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
            string gamertag = txtBoxGamertag.Text;

            if (PasswordBox.Visibility == Visibility.Visible)
            {
                password = PasswordBox.Password;
            }
            else
            {
                password = PasswordText.Text;
            }
            try
            {
                if (AccountManager.ValidateCredentials(gamertag, password))
                {
                    if (this.NavigationService != null)
                    {
                        this.NavigationService.Navigate(new HomePage());
                    }
                }
                else
                {
                    new AlertModal("Datos incorrectos", "Gamertag y/o contraseña incorrectos").ShowDialog();
                }
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                log.Error("Error del servidor (FaultException)", faultException);
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                log.Warn("Problema de comunicación con el servidor", communicationException);
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
            }
        }

        private void ChangePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                PasswordText.Text = PasswordBox.Password;
                PasswordText.Visibility = Visibility.Visible; 
                PasswordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordBox.Password = PasswordText.Text; 
                PasswordBox.Visibility = Visibility.Visible;
                PasswordText.Visibility = Visibility.Collapsed;
            }
        }
    }
}
