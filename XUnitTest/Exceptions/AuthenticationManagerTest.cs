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
        public void TestAddAccountThrowsCommunicationException()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement
            {
                Name = "Dana",
                Email = "zaidskate@hotmail.com",
                Lastname = "Paz",
                Gamertag = "didarkrai",
                Password = "password123"
            };

            Assert.Throws<CommunicationException>(() => proxy.AddAccount(account));
        }

        [Fact]
        public void TestUpdatePasswordThrowsCommunicationException()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement
            {
                Name = "Jesus",
                Email = "jesustlapahernandez@hotmail.com",
                Gamertag = "Tlapa11",
                Password = "Password123"
            };

            Assert.Throws<CommunicationException>(() => proxy.UpdatePassword(account));
        }

        [Fact]
        public void TestVerifyExistingEmailThrowsCommunicationException()
        {
            string emailExisting = "jesustlapahernandez@hotmail.com";

            Assert.Throws<CommunicationException>(() => proxy.VerifyExistingEmail(emailExisting));
        }

        [Fact]
        public void TestVerifyNonExistingEmailThrowsCommunicationException()
        {
            string emailExisting = "jesus@hotmail.com";

            Assert.Throws<CommunicationException>(() => proxy.VerifyExistingEmail(emailExisting));
        }

        [Fact]
        public void TestVerifyExistingGamertagThrowsCommunicationException()
        {
            string gamertagExisting = "Tlapa11";

            Assert.Throws<CommunicationException>(() => proxy.VerifyExistingGamertag(gamertagExisting));
        }

        [Fact]
        public void TestVerifyNonExistingGamertagThrowsCommunicationException()
        {
            string gamertagExisting = "Tlapa";

            Assert.Throws<CommunicationException>(() => proxy.VerifyExistingGamertag(gamertagExisting));
        }
    }
}
