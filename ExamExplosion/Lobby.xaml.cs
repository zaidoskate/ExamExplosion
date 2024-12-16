using ExamExplosion.DataValidations;
using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamExplosion
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Page
    {
        private readonly LobbyManager lobbyManager = null;
        private string lobbyCode;
        private readonly int maxPlayers;
        private readonly int timePerTurn;
        private readonly int maxHP;
        private string hostGamertag;
        private readonly Dictionary<string, bool> playerReadyStatus = new Dictionary<string, bool>();
        private readonly List<Label> labelsGamertags = new List<Label>();
        private readonly List<Image> imagePlayers = new List<Image>();
        private readonly ILog log;
        public Lobby(int maxPlayers, int timePerTurn, int maxHP, string owner)
        {
            log = LogManager.GetLogger(typeof(App));
            lobbyManager = new LobbyManager(this);
            Game game = new Game();
            game.NumberPlayers = maxPlayers;
            game.HostPlayerId = SessionManager.CurrentSession.userId;
            game.TimePerTurn = timePerTurn;
            game.Lives = maxHP;

            this.maxPlayers = maxPlayers;
            this.maxHP = maxHP;
            this.timePerTurn = timePerTurn;
            this.hostGamertag = owner;
            InitializeLobby(game);
            InitializeComponent();
            var parentWindow = (MainWindow)Application.Current.MainWindow;
            if (parentWindow != null)
            {
                parentWindow.Closing += OnClosing;
            }
            InitializeLobbyManager(); 
            lobbyCodeLbl.Content = lobbyCode;
            this.KeyDown += Lobby_KeyDown;
            this.Focusable = true;
            this.Focus();

        }
        public Lobby(string lobbyCode)
        {
            lobbyManager = new LobbyManager(this);
            InitializeComponent();
            var parentWindow = (MainWindow)Application.Current.MainWindow;
            if (parentWindow != null)
            {
                parentWindow.Closing += OnClosing;
            }
            this.lobbyCode = lobbyCode;
            this.lobbyCodeLbl.Content = lobbyCode;
            InitializeLobbyManager();
            try
            {
                lobbyManager.ConnectLobby(SessionManager.CurrentSession.gamertag, this.lobbyCode);
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
                log.Fatal($"{communicationException.StackTrace}", communicationException);
                NavigateStartPage();
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblTimeoutException).ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                NavigateStartPage();
            }
            this.KeyDown += Lobby_KeyDown;
            this.Focusable = true;
            this.Focus();
        }

        private void InitializeLobby(Game game)
        {
            try
            {
                lobbyCode = lobbyManager.CreateLobby(game);
                lobbyManager.ConnectLobby(SessionManager.CurrentSession.gamertag, this.lobbyCode);
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
                log.Fatal($"{communicationException.StackTrace}", communicationException);
                NavigateStartPage();
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblTimeoutException).ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                NavigateStartPage();
            }
        }

        private void Lobby_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (!messageTxtBox.IsFocused)
                {
                    messageTxtBox.Focus();
                }
                else
                {
                    SendMessageButton_Click(this, new RoutedEventArgs());
                }
            }
        }

        private void InitializeLobbyManager()
        {
            labelsGamertags.Add(player1Lbl);
            labelsGamertags.Add(player2Lbl);
            labelsGamertags.Add(player3Lbl);
            labelsGamertags.Add(player4Lbl);
            imagePlayers.Add(player1Img);
            imagePlayers.Add(player2Img);
            imagePlayers.Add(player3Img);
            imagePlayers.Add(player4Img);

        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                lobbyManager.DisconnectLobby(lobbyCode, SessionManager.CurrentSession.gamertag);
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

        public void PrintNewMessage(string gamertag, string message)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                var messageBlock = new TextBlock
                {
                    Text = $"{gamertag}: {message}",
                    Foreground = Brushes.White,
                    FontSize = 20,
                    FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#VT323"),
                    Margin = new Thickness(5),
                    MaxWidth = 300,
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                chatStackPanel.Children.Add(messageBlock);
            });
        }

        public void PrintLobbyPlayers(string gamertag, bool isReady, int index)
        {
            labelsGamertags[index].Content = gamertag;
            BitmapImage readyImage = new BitmapImage();
            readyImage.BeginInit();
            if (isReady)
            {
                readyImage.UriSource = new Uri("pack://application:,,,/Images/ReadyImage.png", UriKind.Absolute);
            }
            else
            {
                readyImage.UriSource = new Uri("pack://application:,,,/Images/NoReadyImage.png", UriKind.Absolute);
            }
            readyImage.EndInit();
            imagePlayers[index].Source = readyImage;

        }
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = GetTextBoxMessage();
                TextValidator.ValidateNotBlanks(message);
                TextValidator.ValidateChatFormat(message);
                lobbyManager.SendMessage(lobbyCode, message, SessionManager.CurrentSession.gamertag);
                ClearTextBox();
            }
            catch (DataValidationException)
            {
                /* There's no need to log or manipulate this exception, and not even notify the user about it because it just validates that not necessary spaces or chars are being entered
                   inside a message */
                ClearTextBox();
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
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string gamertag = SessionManager.CurrentSession.gamertag;
            int playerIndex = labelsGamertags.FindIndex(lbl => lbl.Content.ToString() == gamertag);

            if (playerIndex >= 0)
            {
                Image playerImage = imagePlayers[playerIndex];
                bool ready = playerImage.Source != null && playerImage.Source.ToString() == "pack://application:,,,/Images/ReadyImage.png";
                if (AtLeastTwoPlayers())
                {
                    if (ready && this.hostGamertag == SessionManager.CurrentSession.gamertag)
                    {
                        SynchronizeClients();
                    }
                    else
                    {
                        try
                        {
                            lobbyManager.ChangeStatus(lobbyCode, gamertag, true);
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
                }
            }
        }

        private bool AtLeastTwoPlayers()
        {
            return !player2Lbl.Content.Equals("");
        }

        private void SynchronizeClients()
        {
            if (CheckAllPlayersReady())
            {
                try
                {
                    lobbyManager.PlayGame(lobbyCode);
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
            else
            {
                new AlertModal(ExamExplosion.Properties.Resources.lobbyLblWait, ExamExplosion.Properties.Resources.lobbyLblPlayersArentReady).ShowDialog();
            }
        }

        private bool CheckAllPlayersReady()
        {
            var players = new (Label Label, Image Image)[]
            {
                (player1Lbl, player1Img),
                (player2Lbl, player2Img),
                (player3Lbl, player3Img),
                (player4Lbl, player4Img),
            };

            string readyImagePath = "pack://application:,,,/Images/ReadyImage.png";
            foreach (var player in players)
            {
                if(player.Label.Visibility == Visibility.Visible && !string.IsNullOrEmpty(player.Label.Content?.ToString()))
                {
                    if (!(player.Image.Source is BitmapImage source) || source.UriSource.ToString() != readyImagePath)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void NavigateToBoard()
        {
            bool allPlayersReady = imagePlayers
                            .Take(this.maxPlayers)
                            .Where(img => img.Visibility == Visibility.Visible)
                            .All(img => img.Source != null && img.Source.ToString() != "pack://application:,,,/Images/NoReadyImage.png");

            if (allPlayersReady)
            {
                if (this.NavigationService != null)
                {
                    List<Label> players = labelsGamertags.Where(label => !label.Content.Equals("")).ToList();
                    this.NavigationService.Navigate(new Board(players,lobbyCode, hostGamertag));
                    var window = Window.GetWindow(this);
                    if (window != null)
                    {
                        window.Height = 720;
                        window.Width = 1200;
                        window.SizeToContent = SizeToContent.Manual;
                    }
                }
            }
        }

        private void ClearTextBox()
        {
            messageTxtBox.Clear();
        }

        private string GetTextBoxMessage()
        {
            return messageTxtBox.Text;
        }

        private void PreviousMenu(object sender, RoutedEventArgs e)
        {
            try
            {
                lobbyManager.DisconnectLobby(lobbyCode, SessionManager.CurrentSession.gamertag);
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
            finally
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.MainFrame.Navigate(new HomePage());
            }
        }

        public void ClearPlayers()
        {
            foreach (var labelGamertag in labelsGamertags)
            {
                labelGamertag.Content = "";
            }

            player1Img.Source = null;
            player2Img.Source = null;
            player3Img.Source = null;
            player4Img.Source = null;
        }

        internal void UpdateHost()
        {
            hostGamertag = player1Lbl.Content.ToString();
            if (SessionManager.CurrentSession.gamertag.Equals(player1Lbl.Content.ToString()))
            {
                SessionManager.CurrentSession.isLobbyOwner = true;
            }
            else
            {
                SessionManager.CurrentSession.isLobbyOwner = false;
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
