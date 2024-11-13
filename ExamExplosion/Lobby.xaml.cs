using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
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
        private LobbyManager lobbyManager = null;
        private string lobbyCode;
        private int maxPlayers;
        private int timePerTurn;
        private int maxHP;
        private string hostGamertag;
        private Dictionary<string, bool> playerReadyStatus = new Dictionary<string, bool>();
        private List<Label> labelsGamertags = new List<Label>();
        private List<Image> imagePlayers = new List<Image>();
        public Lobby(int maxPlayers, int timePerTurn, int maxHP, string owner)
        {
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
            lobbyCodelbl.Content = lobbyCode;
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
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                throw timeoutException;
            }
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
            this.lobbyCodelbl.Content = lobbyCode;
            InitializeLobbyManager();
            try
            {
                lobbyManager.ConnectLobby(SessionManager.CurrentSession.gamertag, this.lobbyCode);
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                throw timeoutException;
            }
            this.KeyDown += Lobby_KeyDown;
            this.Focusable = true;
            this.Focus();
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
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                throw timeoutException;
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
                lobbyManager.SendMessage(lobbyCode, message, SessionManager.CurrentSession.gamertag);
                ClearTextBox();
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                throw timeoutException;
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
                        new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                        throw faultException;
                    }
                    catch (CommunicationException communicationException)
                    {
                        new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                        throw communicationException;
                    }
                    catch (TimeoutException timeoutException)
                    {
                        new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                        throw timeoutException;
                    }
                }
            }
        }

        private void SynchronizeClients()
        {
            try
            {
                lobbyManager.PlayGame(lobbyCode);
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                throw timeoutException;
            }
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
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                throw timeoutException;
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
    }
}
