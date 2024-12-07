using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest.CallbackImplementation
{
    class LobbyCallbacks : ExamExplotionService.ILobbyManagerCallback
    {
        private InstanceContext context;
        private ExamExplotionService.LobbyManagerClientBase proxy;
        public required string MessageReceived { get; set; }
        public bool PlayerStatus {  get; set; }
        private string gamertag;
        private string gameCode;

        public LobbyCallbacks()
        {
            context = new InstanceContext(this);
            proxy = new ExamExplotionService.LobbyManagerClientBase(context);

            gamertag = "zaidstake";
            gameCode = "AAAA";

            ExamExplotionService.GameManagement game = new ExamExplotionService.GameManagement();
            game.Lives = 3;
            game.TimePerTurn = 15;
            game.NumberPlayers = 2;
            game.InvitationCode = gameCode;
            game.HostPlayerId = 1;
            proxy.CreateLobby(game);
            proxy.Connect(gamertag, gameCode);
        }
        public void SendMessage(string message)
        {
            proxy.SendMessage(gameCode, gamertag, message);
        }
        public void ReceiveMessage(string gamertag, string message)
        {
            MessageReceived = message;
        }

        public void Repaint(Dictionary<string, bool> playerStatus)
        {
            PlayerStatus = playerStatus[gamertag];
        }

        public void PlayGame()
        {
            proxy.UpdatePlayerStatus(gameCode, gamertag, true);
            proxy.PlayGame(gameCode);
        }
        public void StartGame(Dictionary<string, bool> lobbyPlayers)
        {
            PlayerStatus = lobbyPlayers[gamertag];
        }
    }
}
