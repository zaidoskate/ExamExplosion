using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Properties;
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
using System.Windows.Shapes;

namespace ExamExplosion
{
    /// <summary>
    /// Lógica de interacción para GameCode.xaml
    /// </summary>
    public partial class GameCode : Page
    {
        private readonly LobbyManager lobbyManager = null;
        private readonly ILog log;

        public GameCode()
        {
            InitializeComponent();
            lobbyManager = new LobbyManager();
            log = LogManager.GetLogger(typeof(App));
        }


        private void NavigateHomePage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                if (SessionManager.CurrentSession.isGuest)
                {
                    this.NavigationService.Navigate(new StartPage());
                }
                else
                {
                    this.NavigationService.Navigate(new HomePage());
                }
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 450;
                    window.Width = 800;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }
        }

        private void NavigateToLobbyPage(string enteredCode)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Lobby(enteredCode));
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 450;
                    window.Width = 800;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }
        }

        public bool JoinLobby(string code, string gamertag)
        {
            if (code.Length == 4)
            {
                try
                {
                    string player = SessionManager.CurrentSession.gamertag;
                    bool joined = lobbyManager.JoinLobby(code, player);
                    if (joined)
                    {
                        NavigateToLobbyPage(code);
                    }
                    else
                    {
                        new AlertModal(ExamExplosion.Properties.Resources.gameCodeLblInexistentLobbyTitle, ExamExplosion.Properties.Resources.gameCodeLblInexistentLobby).ShowDialog();
                    }
                    return joined;
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
            return false;
        }

        private void JoinBtn_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = lobbyCodeTxtBox.Text.ToUpper();
            JoinLobby(enteredCode, SessionManager.CurrentSession.gamertag);
        }

        private void NavigateStartPage()
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 450;
                    window.Width = 800;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }
        }
    }
}
