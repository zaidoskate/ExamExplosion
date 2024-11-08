using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamExplosion;
using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Models;

namespace XUnitTest
{
    public class GameManagerTest
    {
        private static ExamExplotionService.GameManagerClient proxy = new ExamExplotionService.GameManagerClient();

        [Fact]
        public void TestGetGameSuccess()
        {
            string gameCode = "FBRM";
            ExamExplotionService.GameManagement gameObtained = proxy.GetGame(gameCode);
            Assert.NotNull(gameObtained);
        }

        [Fact]
        public void TestGetGameFail()
        {
            string gameCodeWrong = "????";
            ExamExplotionService.GameManagement gameObtained = proxy.GetGame(gameCodeWrong);
            Assert.Null(gameObtained);
        }
    }
}
