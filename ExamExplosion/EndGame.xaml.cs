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
        private ReportManager reportManager = null;
        private AccountManager accountManager = null;
        private ILog log;
        public EndGame()
        {
            accountManager = new AccountManager();
            reportManager = new ReportManager();
            InitializeComponent();
            log = LogManager.GetLogger(typeof(App));
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
                            idPlayerReported = accountManager.GetAccountIdByGamertag(reportedPlayer);
                            break;
                        case "ReportPlayer2":
                            reportedPlayer = player2lbl.Content.ToString();
                            idPlayerReported = accountManager.GetAccountIdByGamertag(reportedPlayer);
                            break;
                        case "ReportPlayer3":
                            reportedPlayer = player3lbl.Content.ToString();
                            idPlayerReported = accountManager.GetAccountIdByGamertag(reportedPlayer);
                            break;
                        case "ReportPlayer4":
                            reportedPlayer = player4lbl.Content.ToString();
                            idPlayerReported = accountManager.GetAccountIdByGamertag(reportedPlayer);
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

        private void LeaveGame(object sender, RoutedEventArgs e)
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
        private void NavigateStartPage()
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
    }
}
