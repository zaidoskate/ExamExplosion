using ExamExplosion.ExamExplotionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest
{
    public class Fixtures
    {
        public string gameCode {  get; set; }
        public async void CreateLobby(ExamExplotionService.LobbyManagerClient proxy)
        {
            ExamExplotionService.GameM gameToCreate = new ExamExplotionService.GameM();
            gameToCreate.NumberPlayers = 1;
            gameToCreate.TimePerTurn = 15;
            gameToCreate.Lives = 1;
            gameToCreate.HostPlayerId = 7;
            string code = await proxy.CreateLobbyAsync(gameToCreate);
            this.gameCode = code;
        }
        public async void ConnectLobby(ExamExplotionService.LobbyManagerClient proxy)
        {
            string gamertag = "tlapa11";
            await proxy.ConnectAsync(gamertag, gameCode);
        }
    }
    public class LobbyManagerTest:IClassFixture<Fixtures>
    {
        private static ExamExplotionService.LobbyManagerClient proxy = new ExamExplotionService.LobbyManagerClient();
        private readonly Fixtures fixture; 

        public LobbyManagerTest(Fixtures _fixture)
        {
            this.fixture = _fixture;
            this.fixture.CreateLobby(proxy);
        }

        [Fact]
        public async void TestJoinLobbySuccess()
        {
            bool result = await proxy.JoinLobbyAsync(fixture.gameCode, "cristy11");
            Assert.True(result);
        }

    }
}
