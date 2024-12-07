﻿using ExamExplosion.ExamExplotionService;
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

namespace ExamExplosion
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : Page
    {
        private GameManager gameManager = null;
        private DispatcherTimer turnTimer;
        private List<Label> playersInGame = new List<Label>();
        private List<Image> playerImages = new List<Image>();
        private List<string> orderedGamertags;
        private int hitPoints;
        private int timePerTurn;
        private int remainingTime;
        private string gameCode;
        private string hostGamertag;
        private ILog log;
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
            cardsNames.Add("examBomb", "Repite");
            cardsNames.Add("exempt", "Excentar");
            cardsNames.Add("leftTeam", "Deja el equipo");
            cardsNames.Add("please", "Paro");
            cardsNames.Add("profeA", "Profe A");
            cardsNames.Add("profeO", "Profe O");
            cardsNames.Add("profeS", "Profe S");
            cardsNames.Add("profeR", "Profe R");
            cardsNames.Add("profeM", "Profe M");
            cardsNames.Add("reRegistration", "Re inscripcion");
            cardsNames.Add("shuffle", "Revolver");
            cardsNames.Add("takeFromBelow", "Tomar de abajo");
            cardsNames.Add("viewTheFuture", "Ver el futuro");
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
                        this.heart1Image.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        this.heart1Image.Visibility = Visibility.Visible;
                        this.heart2Image.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        this.heart1Image.Visibility = Visibility.Visible;
                        this.heart2Image.Visibility = Visibility.Visible;
                        this.heart3Image.Visibility = Visibility.Visible;
                        break;
                }
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

        private void InitializeListComponents()
        {
            playersInGame.Add(this.player1Lbl);
            playersInGame.Add(this.player2Lbl);
            playersInGame.Add(this.player3Lbl);
            playersInGame.Add(this.player4Lbl);

            playerImages.Add(this.player1Image);
            playerImages.Add(this.player2Image);
            playerImages.Add(this.player3Image);
            playerImages.Add(this.player4Image);
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
            PlayerDeck.Children.Clear();
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
                image.MouseLeftButtonDown += CardClicked;
                image.MouseEnter += CardOver;
                if (card.IsSelected)
                {
                    image.Opacity = 0.5;
                }
                else
                {
                    image.Opacity = 1;
                }
                PlayerDeck.Children.Add(image);
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
                    txtBlockCardName.Text = cardsNames[path];
                }
            }
        }


        private void CardClicked(object sender, MouseButtonEventArgs e)
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
                SelectedCards.Children.Add(newCard);
                clickedCard.Opacity = 0.5;
            }
            else
            {
                RemoveSelectedCards(clickedCard);
            }
        }

        private void RemoveSelectedCards(Image clickedCard)
        {
            var cardToRemove = SelectedCards.Children
                .OfType<Image>()
                .FirstOrDefault(img => (int)img.Tag == (int)clickedCard.Tag);

            if (cardToRemove != null)
            {
                gameManager.DeselectCard((int)clickedCard.Tag);
                SelectedCards.Children.Remove(cardToRemove);
                clickedCard.Opacity = 1.0;
            }
        }

        private void ShowLeftCards(object sender, RoutedEventArgs e)
        {
            gameManager.UpdatePlayerDeck(true);
        }

        private void ShowRightCards(object sender, RoutedEventArgs e)
        {
            gameManager.UpdatePlayerDeck(false);
        }
        private void PlayCards(object sender, RoutedEventArgs e)
        {
            //aqui solo se valida si se pudo o no jugar la carta, si se pudo se muestra un mensaje predeterminado de que no se puede
            if(gameManager.PlayCards(gameCode))
            {
                
            }
        }
        private void DrawCard(object sender, RoutedEventArgs e)
        {
            gameManager.DrawCard(gameCode, SessionManager.CurrentSession.gamertag);
            DrawCardAnimation();
        }

        public void DrawCardAnimation()
        {
            animatedCard.RenderTransform = new TranslateTransform(0, 0);
            animatedCard.Opacity = 1;
            animatedCard.Visibility = Visibility.Visible;

            var storyboard = (Storyboard)FindResource("CardDrawAnimation");
            storyboard.Begin();
        }
        internal void UpdateButtonsVisibility(string gamertagInTurn)
        {
            string gamertag = SessionManager.CurrentSession.gamertag;
            if (gamertag == gamertagInTurn)
            {
                btnPlayCards.IsEnabled = true;
                btnStack.IsEnabled = true;
            }
            else
            {
                btnPlayCards.IsEnabled = false;
                btnStack.IsEnabled = false;
            }
        }

        public void UpdateGameDeckCount(int count)
        {
            remainingCardsNumber.Content = count;
        }
        public void PrintCardOnBoard(string path)
        {
            CardOnBoard.Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{path}.png", UriKind.Absolute));
        }

        public void ShowTopCards(List<Card> topCards)
        {
            if (topCards.Count >= 1)
            {
                string cardPath = topCards[0].Path;
                FirstCard.Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{cardPath}.png", UriKind.Absolute));
            }

            if (topCards.Count >= 2)
            {
                string cardPath = topCards[1].Path;
                SecondCard.Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{cardPath}.png", UriKind.Absolute));
            }

            if (topCards.Count == 3)
            {
                string cardPath = topCards[2].Path;
                ThirdCard.Source = new BitmapImage(new Uri($"pack://application:,,,/CardsPackages/{this.defaultPackage}/{cardPath}.png", UriKind.Absolute));
            }
        }

        public void ResetTopCardsPath()
        {
            this.FirstCard.Source = null;
            this.SecondCard.Source = null;
            this.ThirdCard.Source = null;
        }

        internal void StartPlayerSelection(string gameCode)
        {
            MakePlayerButtonVisible();
            FirstPlayerBtn.Click += (sender, e) => HandlePlayerSelected(this.player1Lbl.Content.ToString(), gameCode);
            SecondPlayerBtn.Click += (sender, e) => HandlePlayerSelected(this.player2Lbl.Content.ToString(), gameCode);
            ThirdPlayerBtn.Click += (sender, e) => HandlePlayerSelected(this.player3Lbl.Content.ToString(), gameCode);
            FourthPlayerBtn.Click += (sender, e) => HandlePlayerSelected(this.player4Lbl.Content.ToString(), gameCode);
        }

        private void MakePlayerButtonVisible()
        {
            if(this.player1Lbl.Visibility == Visibility.Visible)
            {
                this.FirstPlayerBtn.Visibility = Visibility.Visible;
            }
            if (this.player2Lbl.Visibility == Visibility.Visible)
            {
                this.SecondPlayerBtn.Visibility = Visibility.Visible;
            }
            if (this.player3Lbl.Visibility == Visibility.Visible)
            {
                this.ThirdPlayerBtn.Visibility = Visibility.Visible;
            }
            if (this.player4Lbl.Visibility == Visibility.Visible)
            {
                this.FourthPlayerBtn.Visibility = Visibility.Visible;
            }
        }

        private void ResetPlayerButtonVisibility()
        {
            FirstPlayerBtn.Visibility = Visibility.Hidden;
            SecondPlayerBtn.Visibility = Visibility.Hidden;
            ThirdPlayerBtn.Visibility = Visibility.Hidden;
            FourthPlayerBtn.Visibility = Visibility.Hidden;
        }

        private void HandlePlayerSelected(string selectedPlayer, string gameCode)
        {
            if (selectedPlayer != SessionManager.CurrentSession.gamertag && CardOnBoard.Source is BitmapImage bitmapImage)
            {
                string cardOnBoard = bitmapImage.UriSource.ToString();
                var requestCardRoutes = new List<string>
                {
                    "pack://application:,,,/CardsPackages/NormalPackage/please.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeA.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeM.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeO.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeR.png",
                    "pack://application:,,,/CardsPackages/NormalPackage/profeS.png"
                };
                if (requestCardRoutes.Contains(cardOnBoard))
                {
                    gameManager.RequestCard(gameCode, selectedPlayer, SessionManager.CurrentSession.gamertag);
                }
                else if (cardOnBoard == "pack://application:,,,/CardsPackages/NormalPackage/leftTeam.png")
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
            var cardsToRemove = SelectedCards.Children.OfType<Image>().ToList();

            foreach (var card in cardsToRemove)
            {
                int cardId = (int)card.Tag;
                SelectedCards.Children.Remove(card);
                gameManager.RemoveCardFromPlayerHand(cardId);
            }
        }
    }
}
