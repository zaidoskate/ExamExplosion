using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest.CRUD_Tests
{
    public class ReportManagerTest
    {
        private static ExamExplotionService.ReportManagerClient proxy = new ExamExplotionService.ReportManagerClient();

        [Fact]
        public void TestGetReportCountSuccess()
        {
            int playerId = 33;
            int reportAmountObtained = proxy.GetReportCount(playerId);

            Assert.Equal(0, reportAmountObtained);
        }


        [Fact]
        public void TestReportPlayerSuccess()
        {
            int reportedPlayerId = 34;
            bool reported = proxy.ReportPlayer(reportedPlayerId);

            Assert.True(reported);
        }
    }
}
