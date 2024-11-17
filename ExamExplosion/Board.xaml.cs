using ExamExplosion.ExamExplotionService;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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
        public int remainingTime;
        private string gameCode;
        private string hostGamertag;
        private ILog log;
        public Board(List<Label> playerGamertags, string gameCode, string hostGamertag)
        {
            InitializeComponent();
            gameManager = new GameManager(this);
            this.remainingTime = timePerTurn;
            this.hostGamertag = hostGamertag;
            InitializeBoard(playerGamertags, gameCode);
            orderedGamertags = playerGamertags.Select(label => label.Content.ToString()).ToList();
            log = LogManager.GetLogger(typeof(App));
            try
            {
                gameManager.InitializeGame(gameCode, orderedGamertags);
                gameManager.InitializeDeck(gameCode, orderedGamertags.Count);
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                //throw faultException;
                log.Error("Error del servidor (FaultException)", faultException);
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                //throw communicationException;
                log.Warn("Problema de comunicación con el servidor", communicationException);
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                //throw timeoutException;
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
            }
            StartTurnTimer();
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
                if (SessionManager.CurrentSession.gamertag == hostGamertag)
                {
                    try
                    {
                        GameManager.NotifyEndTurn(gameCode, currentTurnLbl.Content.ToString());
                    }
                    catch (FaultException faultException)
                    {
                        new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                        //throw faultException;
                        log.Error("Error del servidor (FaultException)", faultException);
                    }
                    catch (CommunicationException communicationException)
                    {
                        new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                        //throw communicationException;
                        log.Warn("Problema de comunicación con el servidor", communicationException);
                    }
                    catch (TimeoutException timeoutException)
                    {
                        new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                        //throw timeoutException;
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
                //throw faultException;
                log.Error("Error del servidor (FaultException)", faultException);
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                //throw communicationException;
                log.Warn("Problema de comunicación con el servidor", communicationException);
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                //throw timeoutException;
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
            }
        }
        private void InitializeGameDetails(string gameCode)
        {
            try
            {
                GameManagement game = GameManager.GetCurrentGameDetails(gameCode);
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
                //throw faultException;
                log.Error("Error del servidor (FaultException)", faultException);
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                //throw communicationException;
                log.Warn("Problema de comunicación con el servidor", communicationException);
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                //throw timeoutException;
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
    }
}
