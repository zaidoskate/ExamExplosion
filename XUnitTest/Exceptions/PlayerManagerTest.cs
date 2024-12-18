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
        public void TestAddFriendThrowsCommunicationObjectFaultedException()
        {
            ExamExplotionService.FriendManagement friend = new ExamExplotionService.FriendManagement
            {
                Player1Id = 33,
                Player2Id = 34
            };

            Assert.Throws<CommunicationObjectFaultedException>(() => friendProxy.AddFriend(friend));
        }

        [Fact]
        public void TestGetWinsThrowsCommunicationObjectFaultedException()
        {
            int playerId = 33;

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.GetWins(playerId));
        }

        [Fact]
        public void TestRegisterPlayerThrowsCommunicationObjectFaultedException()
        {
            ExamExplotionService.PlayerManagement playerToRegister = new ExamExplotionService.PlayerManagement
            {
                UserId = 1,
                AccountId = 1
            };

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.RegisterPlayer(playerToRegister));
        }

        [Fact]
        public void TestRegisterPlayerFailThrowsCommunicationObjectFaultedException()
        {
            ExamExplotionService.PlayerManagement playerToRegister = new ExamExplotionService.PlayerManagement();

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.RegisterPlayer(playerToRegister));
        }

        [Fact]
        public void TestUpdateScoreThrowsCommunicationObjectFaultedException()
        {
            int userId = 1;
            int newScore = 10;

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.UpdateScore(userId, newScore));
        }

        [Fact]
        public void TestUpdateScoreFailThrowsCommunicationObjectFaultedException()
        {
            int userId = -1;
            int newScore = -10;

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.UpdateScore(userId, newScore));
        }
    }
}
