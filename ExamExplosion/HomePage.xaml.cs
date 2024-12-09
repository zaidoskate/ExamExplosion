using ExamExplosion.Helpers;
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
    /// Lógica de interacción para HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            gamertagLbl.Content = SessionManager.CurrentSession.gamertag;
        }

        private void NavigateGamePreferencesPage(object sender, RoutedEventArgs e)
        {
            SessionManager.CurrentSession.isLobbyOwner = true;
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new GamePreferences());
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 420;
                    window.Width = 420;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }
        }

        private void NavigateKomalliPage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Komalli());
            }
        }

        private void NavigateLeaderboardPage(object sender, RoutedEventArgs e)
        {

            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Leaderboard());
            }
        }

        private void NavigateGameCodePage(object sender, RoutedEventArgs e)
        {
            SessionManager.CurrentSession.isLobbyOwner = false;
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
        private void LogOut(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
        private void NavigateSettingsPage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Settings());
            }
        }

        private void NavigateCataloguePage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Catalogue());
            }
        }

        private void ViewFriendAndBlockList(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new BlockAndFriendsList());
            }
        }
    }
}
