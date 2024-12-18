using System;
using System.ServiceModel;
using Xunit;

namespace XUnitTest.Exceptions
{
    public class ReportManagerTestException
    {
        private static ExamExplotionService.ReportManagerClient proxy = new ExamExplotionService.ReportManagerClient();

        [Fact]
        public void TestGetReportCountCommunicationException()
        {
            int playerId = 33;

            // Act & Assert
            Assert.Throws<CommunicationException>(() => proxy.GetReportCount(playerId));
        }

        [Fact]
        public void TestReportPlayerCommunicationException()
        {
            int reportedPlayerId = 34;

            // Act & Assert
            Assert.Throws<CommunicationException>(() => proxy.ReportPlayer(reportedPlayerId));
        }
    }
}
