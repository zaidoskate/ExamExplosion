﻿using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public class LobbyManager : ILobbyManagerCallback
    {
        private InstanceContext context = null;
        private LobbyManagerClient proxy = null;
        private Lobby lobbyPage = null;
        public LobbyManager(Lobby _lobbyPage)
        {
            context = new InstanceContext(this);
            proxy = new LobbyManagerClient(context);
            lobbyPage = _lobbyPage;
        }

        public LobbyManager()
        {
            context = new InstanceContext(this);
            proxy = new LobbyManagerClient(context);
        }
        public string CreateLobby(Game game)
        {
            GameM gameM = new GameM();
            gameM.NumberPlayers = game.NumberPlayers;
            gameM.TimePerTurn = game.TimePerTurn;
            gameM.HostPlayerId = game.HostPlayerId;
            gameM.InvitationCode = game.InvitationCode;
            gameM.Lives = game.Lives;

            string newCode = proxy.CreateLobby(gameM);
            
            return newCode;
        }

        public bool JoinLobby(string code, string gamertag)
        {
            return proxy.JoinLobby(code, gamertag);
        }

        public bool ConnectLobby(string code, string gamertag)
        {
            return proxy.Connect(gamertag, code);
        }

        public void SendMessage(string code, string message, string gamertag)
        {
            proxy.SendMessage(code, gamertag, message);
        }
        public void ReceiveMessage(string gamertag, string message)
        {
            lobbyPage.PrintNewMessage(gamertag, message);
        }
    }
}
