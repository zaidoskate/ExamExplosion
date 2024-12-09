using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using ExamExplosion.Properties;

namespace ExamExplosion
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : Page
    {
        private GameManager gameManager = null;
        private DispatcherTimer turnTimer;
        private readonly List<Label> playersInGame = new List<Label>();
        private readonly List<Image> playerImages = new List<Image>();
        private readonly List<string> orderedGamertags;
        private int hitPoints;
        private int timePerTurn;
        private int remainingTime;
        private string gameCode;
        private string hostGamertag;
        private readonly ILog log;
        private string defaultPackage;
        private Dictionary<string, string> cardsNames;

        public Board(List<Label> playerGamertags, string gameCode, string hostGamertag)
        {
            InitializeComponent();
            LoadPackageInUse();
            LoadCardNames();
            gameManager = new GameManager(this);
            this.remainingTime = timePerTurn;
            StartTurnTimer();
            this.hostGamertag = hostGamertag;
            InitializeBoard(playerGamertags, gameCode);
            orderedGamertags = playerGamertags.Select(label => label.Content.ToString()).ToList();
            log = LogManager.GetLogger(typeof(App));
            InitializeGameResources(gameCode, orderedGamertags);
        }

        private void LoadCardNames()
        {
            cardsNames = new Dictionary<string, string>();
            cardsNames.Add("examBomb", ExamExplosion.Properties.Resources.globalLblExamBombCard);
            cardsNames.Add("exempt", ExamExplosion.Properties.Resources.globalLblExemptCard);
            cardsNames.Add("leftTeam", ExamExplosion.Properties.Resources.globalLblLeftTeamCard);
            cardsNames.Add("please", ExamExplosion.Properties.Resources.globalLblFavorCard);
            cardsNames.Add("profeA", ExamExplosion.Properties.Resources.globalLblTeacherACard);
            cardsNames.Add("profeO", ExamExplosion.Properties.Resources.globalLblTeacherOCard);
            cardsNames.Add("profeS", ExamExplosion.Properties.Resources.globalLblTeacherSCard);
            cardsNames.Add("profeR", ExamExplosion.Properties.Resources.globalLblTeacherRCard);
            cardsNames.Add("profeM", ExamExplosion.Properties.Resources.globalLblTeacherMCard);
            cardsNames.Add("reRegistration", ExamExplosion.Properties.Resources.globalReRegistrationCard);
            cardsNames.Add("shuffle", ExamExplosion.Properties.Resources.globalLblShuffleCard);
            cardsNames.Add("takeFromBelow", ExamExplosion.Properties.Resources.globalLblTakeBelowCard);
            cardsNames.Add("viewTheFuture", ExamExplosion.Properties.Resources.globalLblViewFutureCard);
        }

        private void LoadPackageInUse()
        {
            if (SessionManager.CurrentSession.isGuest)
            {
                defaultPackage = "NormalPackage";
                return;
            }
            int playerId = SessionManager.CurrentSession.userId;
            try
            {
                var accessoryInUse = PurchasedAccessoryManager.GetAccessoryInUse(playerId);
                defaultPackage = accessoryInUse.path;
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

        private void InitializeGameResources(string gameCode, List<string> orderedGamertags)
        {
            bool isHost = SessionManager.CurrentSession.isLobbyOwner;
            try
            {
                gameManager.InitializeGame(gameCode, orderedGamertags);
                if (isHost)
                {
                    gameManager.InitializeGameDeck(gameCode, orderedGamertags.Count);
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

        public void StartTurnTimer()
        {
            turnTimer = new DispatcherTimer();
            turnTimer.Interval = TimeSpan.FromSeconds(1);
            turnTimer.Tick += TurnTimer_Tick;
            remainingTime = timePerTurn;
            UpdateTimerLabel();
            turnTimer.Start();
        }

        private void TurnTimer_Tick(object sender, EventArgs e)
        {
            if(remainingTime > 0)
            {
                remainingTime --;
                UpdateTimerLabel();
            }
            else
            {
                turnTimer.Stop();
                ResetTopCardsPath();
                ResetPlayerButtonVisibility();
                if (SessionManager.CurrentSession.gamertag == hostGamertag)
                {
                    try
                    {
                        GameManager gameManager = new GameManager(this);
                        gameManager.NotifyEndTurn(gameCode, currentTurnLbl.Content.ToString());
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

        private void UpdateTimerLabel()
        {
            timerLbl.Content = remainingTime.ToString();
        }

        public void UpdateCurrentTurn(string gamertag)
        {
            currentTurnLbl.Content = gamertag;
            remainingTime = timePerTurn;
            UpdateTimerLabel();
        }

        private void InitializeBoard(List<Label> playerGamertags, string gameCode)
        {
            InitializeListComponents();
            for (int i = 0; i < playerGamertags.Count; i++)
            {
                playersInGame[i].Content = playerGamertags[i].Content;
            }

            for (int i = 0; i < playersInGame.Count; i++)
            {
                Label label = playersInGame[i];
                Image image = playerImages[i];

                if (!string.IsNullOrEmpty(label.Content?.ToString()))
                {
                    label.Visibility = Visibility.Visible;
                    image.Visibility = Visibility.Visible;
                }
                else
                {
                    label.Visibility = Visibility.Collapsed;
                    image.Visibility = Visibility.Collapsed;
                }
            }
            InitializeGameDetails(gameCode);
            ConnectToGame();
        }

        private void ConnectToGame()
        {
            try
            {
                gameManager.ConnectGame(gameCode, SessionManager.CurrentSession.gamertag);
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
        private void InitializeGameDetails(string gameCode)
        {
            try
            {
                GameManager gameManager = new GameManager(this);
                GameManagement game = gameManager.GetCurrentGameDetails(gameCode);
                this.hitPoints = game.Lives;
                this.timePerTurn = game.TimePerTurn;
                this.gameCode = game.InvitationCode;
                switch (hitPoints)
                {
                    case 1:
                        this.heart1Img.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        this.heart1Img.Visibility = Visibility.Visible;
                        this.heart2Img.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        this.heart1Img.Visibility = Visibility.Visible;
                        this.heart2Img.Visibility = Visibility.Visible;
                        this.heart3Img.Visibility = Visibility.Visible;
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
        }

        private void InitializeListComponents()
        {
            playersInGame.Add(this.player1Lbl);
            playersInGame.Add(this.player2Lbl);
            playersInGame.Add(this.player3Lbl);
            playersInGame.Add(this.player4Lbl);

            playerImages.Add(this.player1Img);
            playerImages.Add(this.player2Img);
            playerImages.Add(this.player3Img);
            playerImages.Add(this.player4Img);
        }
        internal void UpdateTurnLabel(string gamertag)
        {
            this.currentTurnLbl.Content = gamertag;
        }

        public void ResetTimer()
        {
            remainingTime = timePerTurn;
            UpdateTimerLabel();
            turnTimer.Start();
        }

        public void UpdatePlayerDeck(List<Card> playerDeckImages, int currentIndex)
        {
            playerDeckStackPanel.Children.Clear();
            int maxDisplay = Math.Min(8, playerDeckImages.Count - currentIndex);
            for (int i = 0; i < maxDisplay; i++)
            {
                Card card = playerDeckImages[currentIndex + i];
                var image = new Image
                {
                    Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{card.Path}.png", UriKind.Absolute)),
                    Height = 80,
                    Margin = new System.Windows.Thickness(0, 20, 0, 0), 
                    Style = (Style)FindResource("CardStyle"),
                    Tag = (int)(currentIndex + i)
                };
                image.MouseLeftButtonDown += CardClickedBtn_Click;
                image.MouseEnter += CardOver;
                if (card.IsSelected)
                {
                    image.Opacity = 0.5;
                }
                else
                {
                    image.Opacity = 1;
                }
                playerDeckStackPanel.Children.Add(image);
            }
        }

        private void CardOver(object sender, MouseEventArgs e)
        {
            var card = sender as Image;

            if (card.Source is BitmapImage bitmapImage)
            {
                string imageUri = bitmapImage.UriSource.ToString();
                string imageName = imageUri.Substring(imageUri.LastIndexOf('/') + 1);
                string path = System.IO.Path.GetFileNameWithoutExtension(imageName);
                if (cardsNames.ContainsKey(path))
                {
                    cardNameTxtBlock.Text = cardsNames[path];
                }
            }
        }


        private void CardClickedBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var clickedCard = sender as Image;
            if (clickedCard.Opacity == 1.0 && gameManager.SelectCard((int)clickedCard.Tag))
            {
                var newCard = new Image
                {
                    Source = clickedCard.Source,
                    Height = 80,
                    Margin = new System.Windows.Thickness(0, 10, 0, 0),
                    Style = (Style)FindResource("CardStyle"),
                    Tag = clickedCard.Tag
                };
                selectedCardsWrapPanel.Children.Add(newCard);
                clickedCard.Opacity = 0.5;
            }
            else
            {
                RemoveSelectedCards(clickedCard);
            }
        }

        private void RemoveSelectedCards(Image clickedCard)
        {
            var cardToRemove = selectedCardsWrapPanel.Children
                .OfType<Image>()
                .FirstOrDefault(img => (int)img.Tag == (int)clickedCard.Tag);

            if (cardToRemove != null)
            {
                gameManager.DeselectCard((int)clickedCard.Tag);
                selectedCardsWrapPanel.Children.Remove(cardToRemove);
                clickedCard.Opacity = 1.0;
            }
        }

        private void ShowLeftCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            gameManager.UpdatePlayerDeck(true);
        }

        private void ShowRightCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            gameManager.UpdatePlayerDeck(false);
        }
        private void PlayCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            //aqui solo se valida si se pudo o no jugar la carta, si se pudo se muestra un mensaje predeterminado de que no se puede
            if(gameManager.PlayCards(gameCode))
            {
                
            }
        }
        private void DrawCardBtn_Click(object sender, RoutedEventArgs e)
        {
            gameManager.DrawCard(gameCode, SessionManager.CurrentSession.gamertag);
            DrawCardAnimation();
        }

        public void DrawCardAnimation()
        {
            animatedCardImg.RenderTransform = new TranslateTransform(0, 0);
            animatedCardImg.Opacity = 1;
            animatedCardImg.Visibility = Visibility.Visible;

            var storyboard = (Storyboard)FindResource("CardDrawAnimation");
            storyboard.Begin();
        }
        internal void UpdateButtonsVisibility(string gamertagInTurn)
        {
            string gamertag = SessionManager.CurrentSession.gamertag;
            if (gamertag == gamertagInTurn)
            {
                playCardsBtn.IsEnabled = true;
                stackBtn.IsEnabled = true;
            }
            else
            {
                playCardsBtn.IsEnabled = false;
                stackBtn.IsEnabled = false;
            }
        }

        public void UpdateGameDeckCount(int count)
        {
            remainingCardsLbl.Content = count;
        }
        public void PrintCardOnBoard(string path)
        {
            cardOnBoardImg.Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{path}.png", UriKind.Absolute));
        }

        public void ShowTopCards(List<Card> topCards)
        {
            if (topCards.Count >= 1)
            {
                string cardPath = topCards[0].Path;
                firstCardImg.Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{cardPath}.png", UriKind.Absolute));
            }

            if (topCards.Count >= 2)
            {
                string cardPath = topCards[1].Path;
                secondCardImg.Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{cardPath}.png", UriKind.Absolute));
            }

            if (topCards.Count == 3)
            {
                string cardPath = topCards[2].Path;
                thirdCardImg.Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{cardPath}.png", UriKind.Absolute));
            }
        }

        public void ResetTopCardsPath()
        {
            this.firstCardImg.Source = null;
            this.secondCardImg.Source = null;
            this.thirdCardImg.Source = null;
        }

        internal void StartPlayerSelection(string gameCode)
        {
            MakePlayerButtonVisible();
            firstPlayerBtn.Click += (sender, e) => HandlePlayerSelected(this.player1Lbl.Content.ToString(), gameCode);
            secondPlayerBtn.Click += (sender, e) => HandlePlayerSelected(this.player2Lbl.Content.ToString(), gameCode);
            thirdPlayerBtn.Click += (sender, e) => HandlePlayerSelected(this.player3Lbl.Content.ToString(), gameCode);
            fourthPlayerBtn.Click += (sender, e) => HandlePlayerSelected(this.player4Lbl.Content.ToString(), gameCode);
        }

        private void MakePlayerButtonVisible()
        {
            if(this.player1Lbl.Visibility == Visibility.Visible)
            {
                this.firstPlayerBtn.Visibility = Visibility.Visible;
            }
            if (this.player2Lbl.Visibility == Visibility.Visible)
            {
                this.secondPlayerBtn.Visibility = Visibility.Visible;
            }
            if (this.player3Lbl.Visibility == Visibility.Visible)
            {
                this.thirdPlayerBtn.Visibility = Visibility.Visible;
            }
            if (this.player4Lbl.Visibility == Visibility.Visible)
            {
                this.fourthPlayerBtn.Visibility = Visibility.Visible;
            }
        }

        private void ResetPlayerButtonVisibility()
        {
            firstPlayerBtn.Visibility = Visibility.Hidden;
            secondPlayerBtn.Visibility = Visibility.Hidden;
            thirdPlayerBtn.Visibility = Visibility.Hidden;
            fourthPlayerBtn.Visibility = Visibility.Hidden;
        }

        private void HandlePlayerSelected(string selectedPlayer, string gameCode)
        {
            if (selectedPlayer != SessionManager.CurrentSession.gamertag && cardOnBoardImg.Source is BitmapImage bitmapImage)
            {
                string cardOnBoardImg = bitmapImage.UriSource.ToString();
                var requestCardRoutes = new List<string>
                {
                    "pack://application:,,,/CardsPackages/NormalPackage/please.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeA.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeM.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeO.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeR.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeS.png"
                };
                if (requestCardRoutes.Contains(cardOnBoardImg))
                {
                    gameManager.RequestCard(gameCode, selectedPlayer, SessionManager.CurrentSession.gamertag);
                }
                else if (cardOnBoardImg == "pack://application:,,,/CardsPackages/NormalPackage/leftTeam.png")
                {
                    // Implementa el ataque
                }
            }
        }


        public void ShowCardRequested(string playerRequesting)
        {
            new AlertModal($"{playerRequesting} te solicito una carta", $"Se le ha otorgado una de tus cartas a {playerRequesting}").ShowDialog();
        }

        public void ShowCardObtained(string cardName)
        {
            new AlertModal("Carta obtenida", $"Has obtenido una carta: {cardName}").ShowDialog();
        }

        public void ClearSelectedCards()
        {
            var cardsToRemove = this.selectedCardsWrapPanel.Children.OfType<Image>().ToList();

            foreach (var card in cardsToRemove)
            {
                int cardId = (int)card.Tag;
                selectedCardsWrapPanel.Children.Remove(card);
                gameManager.RemoveCardFromPlayerHand(cardId);
            }
        }
        private void NavigateStartPage()
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
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
