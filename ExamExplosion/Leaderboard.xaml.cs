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
        private ILog log;
        public Leaderboard()
        {
            InitializeComponent();
            InitializeLeaderboardsLists();
            UpdateLeaderboard(null, null);
            log = LogManager.GetLogger(typeof(App));
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
