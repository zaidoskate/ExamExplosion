using ExamExplosion.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ExamExplosion.Properties;

namespace ExamExplosion
{
    /// <summary>
    /// Lógica de interacción para BlockAndFriendsList.xaml
    /// </summary>
    public partial class BlockAndFriendsList : Page
    {
        public ObservableCollection<string> Friends { get; set; }
        public ObservableCollection<string> BlockedPlayers { get; set; }
        public Dictionary<string, int> playersData;
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public BlockAndFriendsList()
        {
            playersData = new Dictionary<string, int>();
            InitializeComponent();
            UpdateFriendsAndBlockedPlayers();
            DataContext = this;
        }
        private void UpdateFriendsAndBlockedPlayers()
        {
            ResetObservableCollections();
            UpdateFriends();
            UpdateBlockedPlayers();
        }

        private void ResetObservableCollections()
        {
            if(Friends == null)
            {
                Friends = new ObservableCollection<string>();
            }
            if(BlockedPlayers == null)
            {
                BlockedPlayers = new ObservableCollection<string>();
            }
            Friends.Clear();
            BlockedPlayers.Clear();
        }

        private void NavigateHomePage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
            }
        }
        private void UnBlockPlayer(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string gamertag = (string) button.Tag;
            int playerId = SessionManager.CurrentSession.userId;
            
            QuestionModal questionModal = new QuestionModal(ExamExplosion.Properties.Resources.blockAndFriendsListLblUnblock,ExamExplosion.Properties.Resources.blockAndFriendsListLblQuestion +  $" ({gamertag})");
            questionModal.ShowDialog();
            if(questionModal.DialogResult == true)
            {
                try
                {
                    int bloquedPlayerId = playersData[gamertag];
                    if(FriendsAndBloquedPlayersManager.RemoveBlockedPlayer(playerId, bloquedPlayerId)){
                        BlockedPlayers.Remove(gamertag);
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
            }
        }

        private void UpdateBlockedPlayers()
        {
            int playerId = SessionManager.CurrentSession.userId;
            Dictionary<int, string> blockedPlayers = new Dictionary<int, string>();
            try
            {
                blockedPlayers = FriendsAndBloquedPlayersManager.GetBlockedPlayers(playerId);
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
            BlockedPlayers.Clear();
            foreach (var bloquedPlayer in blockedPlayers)
            {
                string gamertag = bloquedPlayer.Value;
                BlockedPlayers.Add(gamertag);
                if (!playersData.ContainsKey(gamertag))
                {
                    playersData[gamertag] = bloquedPlayer.Key;
                }
            }
        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string gamertag = (string)button.Tag;
            int playerId = SessionManager.CurrentSession.userId;

            QuestionModal questionModal = new QuestionModal(ExamExplosion.Properties.Resources.blockAndFriendsListLblDeleteFriend, ExamExplosion.Properties.Resources.blockAndFriendsListLblUnfriendQuestion + $" ({gamertag})");
            questionModal.ShowDialog();
            if (questionModal.DialogResult == true)
            {
                try
                {
                    int playerId2 = playersData[gamertag];
                    if(FriendsAndBloquedPlayersManager.RemoveFriends(playerId, playerId2)){
                        Friends.Remove(gamertag);
                    }
                    else
                    {
                        new AlertModal("No hay conexion", "No se puede eliminar de tus amigos, inténtalo más tarde").ShowDialog();
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
            }
        }
        private void UpdateFriends()
        {
            int playerId = SessionManager.CurrentSession.userId;
            Dictionary<int, string> friendsList = new Dictionary<int, string>();
            try
            {
                friendsList = FriendsAndBloquedPlayersManager.GetFriendsList(playerId);
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

            Friends.Clear();
            foreach (var friend in friendsList)
            {
                string gamertag = friend.Value;
                Friends.Add(gamertag);
                if (!playersData.ContainsKey(gamertag))
                {
                    playersData[gamertag] = friend.Key;
                }
            }
        }

        private void AddFriendBtn_Click(object sender, EventArgs e)
        {
            string gamertagTyped = this.gamertagTxtBox.Text;
            try
            {
                if (AccountManager.VerifyExistingGamertag(gamertagTyped) == 1)
                {
                    int playerToAddId = PlayerManager.GetPlayerIdByGamertag(gamertagTyped);
                    if (playerToAddId > 0 && playerToAddId != SessionManager.CurrentSession.userId)
                    {
                        int friendsAdded = FriendsAndBloquedPlayersManager.AddFriend(SessionManager.CurrentSession.userId, playerToAddId);
                        if (friendsAdded > 0)
                        {
                            UpdateFriends();
                            new AlertModal(ExamExplosion.Properties.Resources.blockAndFriendsListLblFriendAdded, ExamExplosion.Properties.Resources.blockAndFriendsListLblFriendAddedDescription).ShowDialog();
                        }
                        else if(friendsAdded == -2)
                        {
                            new AlertModal(ExamExplosion.Properties.Resources.blockAndFriendsListLblAlreadyFriends, ExamExplosion.Properties.Resources.blockAndFriendsListLblAlreadyFriendsDescription).ShowDialog();
                        }
                        else
                        {
                            new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.blockAndFriendsListLblFriendAddError).ShowDialog();
                        }
                    }
                    else
                    {
                        new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.blockAndFriendsListLblUserFoundError).ShowDialog();
                    }
                }
                else
                {
                    new AlertModal(ExamExplosion.Properties.Resources.blockAndFriendsListLblGamertagNotFound, ExamExplosion.Properties.Resources.blockAndFriendsListLblGamertagNotFound).ShowDialog();
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
