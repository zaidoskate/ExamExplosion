using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest
{
    public class ReportManagerTest
    {
        private static ExamExplotionService.ReportManagerClient proxy = new ExamExplotionService.ReportManagerClient();

        [Fact]
        public void TestGetReportCountSuccess()
        {
            int playerId = 1;
            int reportAmountObtained = proxy.GetReportCount(playerId);

            Assert.Equal(0, reportAmountObtained);
        }

        [Fact]
        public void TestGetReportCountFail()
        {
            int inexistentPlayerId = 1000;
            int reportAmountObtained = proxy.GetReportCount(inexistentPlayerId);

            Assert.Equal(0, reportAmountObtained);
        }

        [Fact]
        public void TestReportPlayerSuccess()
        {
            int reportedPlayerId = 2;
            bool reported = proxy.ReportPlayer(reportedPlayerId);
            
            Assert.True(reported);
        }

        [Fact]
        public void TestReportPlayerFail()
        {
            int inexistentPlayerId = 1000;
            bool reported = proxy.ReportPlayer(inexistentPlayerId);

            Assert.False(reported);
        }
    }
}
