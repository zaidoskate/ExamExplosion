using ExamExplosion.ExamExplotionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest
{
    public class PlayerManagerTest
    {
        private static ExamExplotionService.PlayerManagerClient proxy = new ExamExplotionService.PlayerManagerClient();

        [Fact]
        public void TestAddFriendSuccess()
        {
            //missing
        }

        [Fact]
        public void TestAddFriendFail()
        {
            //missing
        }

        [Fact]
        public void TestGetWinsSuccess()
        {
            //missing
        }

        [Fact]
        public void TestGetWinsFail()
        {
            //missing
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
