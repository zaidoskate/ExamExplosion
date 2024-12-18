using ExamExplosion.ExamExplotionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest.CRUD_Tests
{
    public class PlayerManagerTest
    {
        private static ExamExplotionService.PlayerManagerClient proxy = new ExamExplotionService.PlayerManagerClient();
        private static ExamExplotionService.FriendAndBlockListClient friendProxy = new ExamExplotionService.FriendAndBlockListClient();

        [Fact]
        public void TestAddFriendSuccess()
        {
            ExamExplotionService.FriendManagement friend = new ExamExplotionService.FriendManagement();
            friend.Player1Id = 33;
            friend.Player2Id = 34;
            int result = friendProxy.AddFriend(friend);
            Assert.True(result > 0);
        }

        [Fact]
        public void TestGetWinsSuccess()
        {
            int playerId = 33;
            int playerWins = proxy.GetWins(playerId);
            Assert.Equal(1, playerWins);
        }

        [Fact]
        public void TestRegisterPlayerSuccess()
        {
            ExamExplotionService.PlayerManagement playerToRegister = new ExamExplotionService.PlayerManagement();
            playerToRegister.UserId = 1;
            playerToRegister.AccountId = 1;

            bool playerRegistered = proxy.RegisterPlayer(playerToRegister);
            Assert.True(playerRegistered);
        }

        [Fact]
        public void TestRegisterPlayerFail()
        {
            ExamExplotionService.PlayerManagement playerToRegister = new ExamExplotionService.PlayerManagement();

            bool playerRegistered = proxy.RegisterPlayer(playerToRegister);
            Assert.False(playerRegistered);
        }

        [Fact]
        public void TestUpdateScoreSuccess()
        {
            int userId = 1;
            int newScore = 10;

            bool scoreUpdated = proxy.UpdateScore(userId, newScore);
            Assert.True(scoreUpdated);
        }

        [Fact]
        public void TestUpdateScoreFail()
        {
            int userId = -1;
            int newScore = -10;

            bool scoreUpdated = proxy.UpdateScore(userId, newScore);
            Assert.False(scoreUpdated);
        }
    }
}
