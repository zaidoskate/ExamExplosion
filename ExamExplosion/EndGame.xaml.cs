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
    /// Lógica de interacción para EndGame.xaml
    /// </summary>
    public partial class EndGame : Page
    {
        private readonly ReportManager reportManager = null;
        private readonly AccountManager accountManager = null;
        private readonly GameManager gameManager = null;
        private readonly ILog log;
        public EndGame(string gameCode, string winnerGamertag)
        {
            InitializeComponent();
            accountManager = new AccountManager();
            reportManager = new ReportManager();
            gameManager = new GameManager();
            InitializeLabels(gameCode, winnerGamertag);
            log = LogManager.GetLogger(typeof(App));
        }

        private void InitializeLabels(string gameCode, string winnerGamertag)
        {
            if (SessionManager.CurrentSession.gamertag.Equals(winnerGamertag))
            {
                gameResultLbl.Content = ExamExplosion.Properties.Resources.endGameLblYouWin;
            }
            else
            {
                gameResultLbl.Content = ExamExplosion.Properties.Resources.endGameLblYouLose;
            }

            winnerGamertagLbl.Content = winnerGamertag;

            List<string> playersGamertags = GetPlayersGamertags(gameCode);

            if (playersGamertags.Count > 0)
            {
                string gamertag = playersGamertags.ElementAtOrDefault(0) ?? string.Empty;
                player1lbl.Content = gamertag;
                player1lbl.Visibility = Visibility.Visible;
                if (!gamertag.StartsWith("GUEST") && !gamertag.Equals(SessionManager.CurrentSession.gamertag))
                {
                    AddFriendPlayer1.Visibility = Visibility.Visible;
                    ReportPlayer1.Visibility = Visibility.Visible;
                    BlockPlayer1.Visibility = Visibility.Visible;
                }
            }
            if (playersGamertags.Count > 1)
            {
                string gamertag = playersGamertags.ElementAtOrDefault(1) ?? string.Empty;
                player2lbl.Content = gamertag;
                player2lbl.Visibility = Visibility.Visible;
                if (!gamertag.StartsWith("GUEST") && !gamertag.Equals(SessionManager.CurrentSession.gamertag))
                {
                    AddFriendPlayer2.Visibility = Visibility.Visible;
                    ReportPlayer2.Visibility = Visibility.Visible;
                    BlockPlayer2.Visibility = Visibility.Visible;
                }
            }
            if (playersGamertags.Count > 2)
            {
                string gamertag = playersGamertags.ElementAtOrDefault(2) ?? string.Empty;
                player3lbl.Content = gamertag;
                player3lbl.Visibility = Visibility.Visible;
                if (!gamertag.StartsWith("GUEST") && !gamertag.Equals(SessionManager.CurrentSession.gamertag))
                {
                    AddFriendPlayer3.Visibility = Visibility.Visible;
                    ReportPlayer3.Visibility = Visibility.Visible;
                    BlockPlayer3.Visibility = Visibility.Visible;
                }
            }
            if (playersGamertags.Count > 3)
            {
                string gamertag = playersGamertags.ElementAtOrDefault(3) ?? string.Empty;
                player4lbl.Content = gamertag;
                player4lbl.Visibility = Visibility.Visible;
                if (!gamertag.StartsWith("GUEST") && !gamertag.Equals(SessionManager.CurrentSession.gamertag))
                {
                    AddFriendPlayer4.Visibility = Visibility.Visible;
                    ReportPlayer4.Visibility = Visibility.Visible;
                    BlockPlayer4.Visibility = Visibility.Visible;
                }
            }
        }


        private List<string> GetPlayersGamertags(string gameCode)
        {
            int gameId = gameManager.GetGameId(gameCode);
            List<int> playersId = gameManager.GetPlayersByGameCode(gameId);
            List<string> gamertags = new List<string>();
            foreach (var id in playersId)
            {
                gamertags.Add(accountManager.GetAccountGamertagById(id));
            }
            return gamertags;
        }

        private void ReportPlayer(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if(clickedButton != null)
            {
                string reportedPlayer = string.Empty;
                int idPlayerReported = -1;
                try
                {
                    switch (clickedButton.Name)
                    {
                        case "ReportPlayer1":
                            reportedPlayer = player1lbl.Content.ToString();
                            idPlayerReported = PlayerManager.GetPlayerIdByGamertag(reportedPlayer);
                            break;
                        case "ReportPlayer2":
                            reportedPlayer = player2lbl.Content.ToString();
                            idPlayerReported = PlayerManager.GetPlayerIdByGamertag(reportedPlayer);
                            break;
                        case "ReportPlayer3":
                            reportedPlayer = player3lbl.Content.ToString();
                            idPlayerReported = PlayerManager.GetPlayerIdByGamertag(reportedPlayer);
                            break;
                        case "ReportPlayer4":
                            reportedPlayer = player4lbl.Content.ToString();
                            idPlayerReported = PlayerManager.GetPlayerIdByGamertag(reportedPlayer);
                            break;
                        default:
                            break;
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
                if (idPlayerReported != -1)
                {
                    reportManager.ReportPlayer(idPlayerReported, reportedPlayer);
                    new AlertModal(ExamExplosion.Properties.Resources.endGameLblReportTitle, ExamExplosion.Properties.Resources.endGameLblPlayerReported).ShowDialog();
                }
                else
                {
                    new AlertModal(ExamExplosion.Properties.Resources.endGameLblReportError, ExamExplosion.Properties.Resources.endGameLblReportErrorDescription).ShowDialog();
                }
            }
        }
        private void AddFriend(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                string friendAdded = string.Empty;
                int playerIdFriendAdded = -1;
                try
                {
                    switch (clickedButton.Name)
                    {
                        case "AddFriendPlayer1":
                            friendAdded = player1lbl.Content.ToString();
                            playerIdFriendAdded = PlayerManager.GetPlayerIdByGamertag(friendAdded);
                            break;
                        case "AddFriendPlayer2":
                            friendAdded = player2lbl.Content.ToString();
                            playerIdFriendAdded = PlayerManager.GetPlayerIdByGamertag(friendAdded);
                            break;
                        case "AddFriendPlayer3":
                            friendAdded = player3lbl.Content.ToString();
                            playerIdFriendAdded = PlayerManager.GetPlayerIdByGamertag(friendAdded);
                            break;
                        case "AddFriendPlayer4":
                            friendAdded = player4lbl.Content.ToString();
                            playerIdFriendAdded = PlayerManager.GetPlayerIdByGamertag(friendAdded);
                            break;
                        default:
                            break;
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
                if (playerIdFriendAdded != -1)
                {
                    FriendsAndBloquedPlayersManager.AddFriend(SessionManager.CurrentSession.userId ,playerIdFriendAdded);
                    new AlertModal(ExamExplosion.Properties.Resources.endGameLblAddFriendTitle, ExamExplosion.Properties.Resources.endGameLblPlayerAdded).ShowDialog();
                }
                else
                {
                    new AlertModal(ExamExplosion.Properties.Resources.endGameLblAddFriendError, ExamExplosion.Properties.Resources.endGameLblAddFriendErrorDescription).ShowDialog();
                }
            }
        }
        private void BlockPlayer(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                string blockedPlayer = string.Empty;
                int playerIdBlocked = -1;
                try
                {
                    switch (clickedButton.Name)
                    {
                        case "BlockPlayer1":
                            blockedPlayer = player1lbl.Content.ToString();
                            playerIdBlocked = PlayerManager.GetPlayerIdByGamertag(blockedPlayer);
                            break;
                        case "BlockPlayer2":
                            blockedPlayer = player2lbl.Content.ToString();
                            playerIdBlocked = PlayerManager.GetPlayerIdByGamertag(blockedPlayer);
                            break;
                        case "BlockPlayer3":
                            blockedPlayer = player3lbl.Content.ToString();
                            playerIdBlocked = PlayerManager.GetPlayerIdByGamertag(blockedPlayer);
                            break;
                        case "BlockPlayer4":
                            blockedPlayer = player4lbl.Content.ToString();
                            playerIdBlocked = PlayerManager.GetPlayerIdByGamertag(blockedPlayer);
                            break;
                        default:
                            break;
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
                if (playerIdBlocked != -1)
                {
                    FriendsAndBloquedPlayersManager.AddBlockedPlayer(SessionManager.CurrentSession.userId, playerIdBlocked);
                    new AlertModal(ExamExplosion.Properties.Resources.endGameLblBlockTitle, ExamExplosion.Properties.Resources.endGameLblPlayerBlocked).ShowDialog();
                }
                else
                {
                    new AlertModal(ExamExplosion.Properties.Resources.endGameLblBlockError, ExamExplosion.Properties.Resources.endGameLblBlockErrorDescription).ShowDialog();
                }
            }
        }

        private void LeaveGame(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                if(SessionManager.CurrentSession.isGuest)
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
        private void NavigateStartPage()
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
    }
}
