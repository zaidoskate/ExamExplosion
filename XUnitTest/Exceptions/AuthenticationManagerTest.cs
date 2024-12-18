using ExamExplosion;
using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
using System.ServiceModel;
using Xunit;

namespace XUnitTest.Exceptions
{
    public class AuthenticationManagerTestException
    {
        private static ExamExplotionService.AuthenticationManagerClient proxy = new ExamExplotionService.AuthenticationManagerClient();

        [Fact]
        public void TestAddAccountThrowsCommunicationObjectFaultedException()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement
            {
                Name = "Dana",
                Email = "zaidskate@hotmail.com",
                Lastname = "Paz",
                Gamertag = "didarkrai",
                Password = "password123"
            };

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.AddAccount(account));
        }

        [Fact]
        public void TestUpdatePasswordThrowsCommunicationObjectFaultedException()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement
            {
                Name = "Jesus",
                Email = "jesustlapahernandez@hotmail.com",
                Gamertag = "Tlapa11",
                Password = "Password123"
            };

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.UpdatePassword(account));
        }

        [Fact]
        public void TestVerifyExistingEmailThrowsCommunicationObjectFaultedException()
        {
            string emailExisting = "jesustlapahernandez@hotmail.com";

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.VerifyExistingEmail(emailExisting));
        }

        [Fact]
        public void TestVerifyNonExistingEmailThrowsCommunicationObjectFaultedException()
        {
            string emailExisting = "jesus@hotmail.com";

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.VerifyExistingEmail(emailExisting));
        }

        [Fact]
        public void TestVerifyExistingGamertagThrowsCommunicationObjectFaultedException()
        {
            string gamertagExisting = "Tlapa11";

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.VerifyExistingGamertag(gamertagExisting));
        }

        [Fact]
        public void TestVerifyNonExistingGamertagThrowsCommunicationObjectFaultedException()
        {
            string gamertagExisting = "Tlapa";

            Assert.Throws<CommunicationObjectFaultedException>(() => proxy.VerifyExistingGamertag(gamertagExisting));
        }
    }
}
