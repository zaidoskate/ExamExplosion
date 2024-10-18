using ExamExplosion.ExamExplotionService;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamExplosion
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Page, ExamExplotionService.ILobbyManagerCallback
    {
        private ILobbyManager _lobbyManager;
        private int MaxPlayers {  get; set; }
        private int TimePerTurn { get; set; }
        private int MaxHP { get; set; }

        private string owner;
        private string lobbyCode;
        public Lobby(int maxPlayers, int timePerTurn, int maxHP, string owner)
        {
            InitializeComponent();
            MaxPlayers = maxPlayers;
            TimePerTurn = timePerTurn;
            MaxHP = maxHP;
            this.owner = owner;
            var parentWindow = (MainWindow)Application.Current.MainWindow;
            if (parentWindow != null)
            {
                parentWindow.Closing += OnClosing;
            }
            InitializeLobbyManager();
            Initialize_LobbyResources();
            this.KeyDown += Lobby_KeyDown;
            this.Focusable = true;
            this.Focus();
        }

        public Lobby(string lobbyCode)
        {
            InitializeComponent();
            var parentWindow = (MainWindow)Application.Current.MainWindow;
            if (parentWindow != null)
            {
                parentWindow.Closing += OnClosing;
            }
            this.lobbyCode = lobbyCode;
            InitializeLobbyManager();
            Initialize_LobbyResources();
            this.KeyDown += Lobby_KeyDown;
            this.Focusable = true;
            this.Focus();
        }

        private void Initialize_LobbyResources()
        {
            player1Lbl.Content = owner;
            player2Lbl.Visibility = Visibility.Hidden;
            player3Lbl.Visibility = Visibility.Hidden;
            player4Lbl.Visibility = Visibility.Hidden;
            player1Img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/NoReadyImage.png"));
            player2Img.Visibility = Visibility.Hidden;
            player3Img.Visibility = Visibility.Hidden;
            player4Img.Visibility = Visibility.Hidden;

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
            var context = new InstanceContext(this);
            var proxy = new LobbyManagerClient(context);

            _lobbyManager = proxy.ChannelFactory.CreateChannel();

            bool connected = _lobbyManager.Connect(SessionManager.CurrentSession.gamertag);
            if (connected)
            {
                string owner = SessionManager.CurrentSession.gamertag;
                lobbyCode = _lobbyManager.CreateLobby(owner);
                lobbyCodelbl.Content = $"Lobby: {lobbyCode}";
            }
        }

        private void Reconnect()
        {
            var context = new InstanceContext(this);
            var proxy = new LobbyManagerClient(context);

            _lobbyManager = proxy.ChannelFactory.CreateChannel();

            bool connected = false;
            connected = _lobbyManager.Connect(SessionManager.CurrentSession.gamertag);
            if (connected)
            {
                Console.WriteLine("Reconectado al lobby.");
            }
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_lobbyManager != null)
            {
                _lobbyManager.Disconnect(SessionManager.CurrentSession.gamertag);
            }
        }



        void ILobbyManagerCallback.ReceiveMessage(string gamertag, string message)
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

        private void UpdatePlayerLabel(string gamertag)
        {
            if (player1Lbl.Visibility == Visibility.Hidden)
            {
                player1Lbl.Content = gamertag;
                player1Lbl.Visibility = Visibility.Visible;
            }
            else if (player2Lbl.Visibility == Visibility.Hidden)
            {
                player2Lbl.Content = gamertag;
                player2Lbl.Visibility = Visibility.Visible;
            }
            else if (player3Lbl.Visibility == Visibility.Hidden)
            {
                player3Lbl.Content = gamertag;
                player3Lbl.Visibility = Visibility.Visible;
            }
            else if (player4Lbl.Visibility == Visibility.Hidden)
            {
                player4Lbl.Content = gamertag;
                player4Lbl.Visibility = Visibility.Visible;
            }
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string message = GetTextBoxMessage();
            if (!string.IsNullOrEmpty(message))
            {
                _lobbyManager.SendMessage(lobbyCode, SessionManager.CurrentSession.gamertag, message);
                ClearTextBox();
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
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new HomePage());
        }
    }
}
