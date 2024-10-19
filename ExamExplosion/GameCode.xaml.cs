using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
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
        private LobbyManager lobbyManager = null;
        

        public GameCode()
        {
            InitializeComponent();
            lobbyManager = new LobbyManager();
        }


        private void GoHome(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
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
                string player = SessionManager.CurrentSession.gamertag;
                bool joined = lobbyManager.JoinLobby(code, player);

                if (joined)
                {
                    NavigateToLobbyPage(code);
                }
                return joined;
            }
            return false;
        }

        private void JoinClick(object sender, RoutedEventArgs e)
        {
            string enteredCode = lobbyCodeTxtBox.Text.ToUpper();
            JoinLobby(enteredCode, SessionManager.CurrentSession.gamertag);
        }
    }
}
