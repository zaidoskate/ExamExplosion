using ExamExplosion;
using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
using Xunit;

namespace XUnitTest.CRUD_Tests
{
    public class AuthenticationManagerTest
    {
        private static ExamExplotionService.AuthenticationManagerClient proxy = new ExamExplotionService.AuthenticationManagerClient();
        [Fact]
        public void TestAddAccountSuccess()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement();
            account.Name = "Dana";
            account.Email = "zaidskate@hotmail.com";
            account.Lastname = "Paz";
            account.Gamertag = "didarkrai";
            account.Password = "password123";

            bool result = proxy.AddAccount(account);
            Assert.True(result);
        }


        [Fact]
        public void TestUpdatePasswordSuccess()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement();
            account.Name = "Jesus";
            account.Email = "jesustlapahernandez@hotmail.com";
            account.Gamertag = "Tlapa11";
            account.Password = "Password123";

            bool result = proxy.UpdatePassword(account);
            Assert.True(result);
        }

        [Fact]
        public void TestVerifyExistingEmailSuccess()
        {
            string emailExisting = "jesustlapahernandez@hotmail.com";
            int result = proxy.VerifyExistingEmail(emailExisting);
            int expectedResult = 1;
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestVerifyExistingEmailFail()
        {
            string emailExisting = "jesus@hotmail.com";
            int result = proxy.VerifyExistingEmail(emailExisting);
            int expectedResult = 0;
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestVerifyExistingGamertagSuccess()
        {
            string gamertagExisting = "Tlapa11";
            int result = proxy.VerifyExistingGamertag(gamertagExisting);
            int expectedResult = 1;
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestVerifyExistingGamertagFail()
        {
            string gamertagExisting = "Tlapa";
            int result = proxy.VerifyExistingGamertag(gamertagExisting);
            int expectedResult = 0;
            Assert.Equal(expectedResult, result);
        }
    }
}