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
        private readonly ILog log;
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
            if (onlyFriendsChkBox.IsChecked == true)
            {
                leaderboardToShow = friendsLeaderboard;
            }
            if (!leaderboardToShow.Any())
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.leaderboardLblObtainingError).ShowDialog();
            }
            leaderboardItemsCtrl.ItemsSource = leaderboardToShow;
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
