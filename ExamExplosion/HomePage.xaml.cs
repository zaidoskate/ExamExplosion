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
            lblGamertag.Content = SessionManager.CurrentSession.gamertag;
        }

        private void CreateLobby(object sender, RoutedEventArgs e)
        {

            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new GamePreferences());
            }
        }

        private void GoKomalli(object sender, RoutedEventArgs e)
        {

        }

        private void GoLeaderboard(object sender, RoutedEventArgs e)
        {

            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Leaderboard());
            }
        }

        private void JoinLobby(object sender, RoutedEventArgs e)
        {

            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new GameCode());
            }
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
        private void GoSettings(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Settings());
            }
        }
    }
}
