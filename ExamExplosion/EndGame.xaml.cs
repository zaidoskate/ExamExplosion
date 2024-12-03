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
        public EndGame()
        {
            accountManager = new AccountManager();
            reportManager = new ReportManager();
            InitializeComponent();
        }

        private void ReportPlayer(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if(clickedButton != null)
            {
                string reportedPlayer = string.Empty;
                int idPlayerReported = -1;
                switch (clickedButton.Name)
                {
                    case "ReportPlayer1":
                        idPlayerReported = accountManager.GetAccountIdByGamertag(player1lbl.Content.ToString());
                        break;
                    case "ReportPlayer2":
                        idPlayerReported = accountManager.GetAccountIdByGamertag(player2lbl.Content.ToString());
                        break;
                    case "ReportPlayer3":
                        idPlayerReported = accountManager.GetAccountIdByGamertag(player3lbl.Content.ToString());
                        break;
                    case "ReportPlayer4":
                        idPlayerReported = accountManager.GetAccountIdByGamertag(player4lbl.Content.ToString());
                        break;
                }
                if (idPlayerReported != -1)
                {
                    reportManager.ReportPlayer(idPlayerReported);
                    new AlertModal("Jugador reportado", "Has reportado a este jugador.").ShowDialog();
                }
                else
                {
                    new AlertModal("Error al reportar al jugador", "Ocurrió un error interno al reportar al jugador.").ShowDialog();
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
    }
}
