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
    public partial class StartPage : Page
    {
        private ILog log;
        public StartPage()
        {
            InitializeComponent();
            buttonPlay.Focus();
            log = LogManager.GetLogger(typeof(App));
        }

        private void DisplayLogin(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Login());
            }
        }
        private void DisplayJoinLobby(object sender, RoutedEventArgs e)
        {
            bool newGuestAdded = false;
            try
            {
                newGuestAdded = GuestManager.AddGuest();
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
            if (!newGuestAdded)
            {
                new AlertModal("Error","Ha ocurrido un error, crea una cuenta o intentalo mas tarde.").ShowDialog();
                return;
            }
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new GameCode());
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 420;
                    window.Width = 420;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }

        }
        private void DisplayCreateAccount(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new AccountCreation());
            }
        }
    }
}
