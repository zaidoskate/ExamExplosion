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
    /// Lógica de interacción para Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : Page
    {
        private Dictionary<string, int> globalLeaderboard;
        private Dictionary<string, int> friendsLeaderboard;
        public Leaderboard()
        {
            InitializeComponent();
            InitializeLeaderboardsLists();
            UpdateLeaderboard(null, null);
        }

        private void InitializeLeaderboardsLists()
        {
            int playerId = SessionManager.CurrentSession.userId;
            try
            {
                globalLeaderboard = PlayerManager.GetGlobalLeaderboard();
                friendsLeaderboard = PlayerManager.GetFriendsLeaderboard(playerId);
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error con el servidor","Ha ocurrido un error interno con el servidor.").ShowDialog();
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de conexion", "Se ha perdido conexion con el servidor, intentalo mas tarde.").ShowDialog();
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Error con el servidor", "Ha ocurrido un error interno con el servidor.").ShowDialog();
            }
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
            }
        }

        private void UpdateLeaderboard(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> leaderboardToShow = globalLeaderboard;
            if (checkOnlyFriends.IsChecked == true)
            {
                leaderboardToShow = friendsLeaderboard;
            }
            itemsCtrlLeaderboard.ItemsSource = leaderboardToShow;
        }
    }
}
