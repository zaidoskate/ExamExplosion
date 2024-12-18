using ExamExplosion.ExamExplotionService;
using System;
using System.ServiceModel;
using Xunit;

namespace XUnitTest.Exceptions
{
    public class PlayerManagerTestException
    {
        private static ExamExplotionService.PlayerManagerClient proxy = new ExamExplotionService.PlayerManagerClient();
        private static ExamExplotionService.FriendAndBlockListClient friendProxy = new ExamExplotionService.FriendAndBlockListClient();

        [Fact]
        public void TestAddFriendThrowsCommunicationException()
        {
            ExamExplotionService.FriendManagement friend = new ExamExplotionService.FriendManagement
            {
                Player1Id = 33,
                Player2Id = 34
            };

            Assert.Throws<CommunicationException>(() => friendProxy.AddFriend(friend));
        }

        [Fact]
        public void TestGetWinsThrowsCommunicationException()
        {
            int playerId = 33;

            Assert.Throws<CommunicationException>(() => proxy.GetWins(playerId));
        }

        [Fact]
        public void TestRegisterPlayerThrowsCommunicationException()
        {
            ExamExplotionService.PlayerManagement playerToRegister = new ExamExplotionService.PlayerManagement
            {
                UserId = 1,
                AccountId = 1
            };

            Assert.Throws<CommunicationException>(() => proxy.RegisterPlayer(playerToRegister));
        }

        [Fact]
        public void TestRegisterPlayerFailThrowsCommunicationException()
        {
            ExamExplotionService.PlayerManagement playerToRegister = new ExamExplotionService.PlayerManagement();

            Assert.Throws<CommunicationException>(() => proxy.RegisterPlayer(playerToRegister));
        }

        [Fact]
        public void TestUpdateScoreThrowsCommunicationException()
        {
            int userId = 1;
            int newScore = 10;

            Assert.Throws<CommunicationException>(() => proxy.UpdateScore(userId, newScore));
        }

        [Fact]
        public void TestUpdateScoreFailThrowsCommunicationException()
        {
            int userId = -1;
            int newScore = -10;

            Assert.Throws<CommunicationException>(() => proxy.UpdateScore(userId, newScore));
        }
    }
}
