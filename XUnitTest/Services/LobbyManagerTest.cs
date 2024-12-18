using ExamExplosion.ExamExplotionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using XUnitTest.CallbackImplementation;

namespace XUnitTest.CRUD_Tests
{
    public class LobbyManagerTest
    {
        [Fact]
        public void TestSendMessageSuccess()
        {
            var lobbyCallbacks = new LobbyCallbacks();
            string message = "Test";
            lobbyCallbacks.SendMessage(message);
            Thread.Sleep(3000);
            Assert.Equal(message, lobbyCallbacks.MessageReceived);
        }

        [Fact]
        public void TestRepaintSuccess()
        {
            var lobbyCallbacks = new LobbyCallbacks();
            Thread.Sleep(3000);
            Assert.False(lobbyCallbacks.PlayerStatus);
        }

        [Fact]
        public void TestStartGameSuccess()
        {
            var lobbyCallbacks = new LobbyCallbacks();
            lobbyCallbacks.PlayGame();
            Thread.Sleep(3000);
            Assert.True(lobbyCallbacks.PlayerStatus);

        }
    }
}
