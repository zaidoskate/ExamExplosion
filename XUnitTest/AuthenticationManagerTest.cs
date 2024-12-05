using ExamExplosion;
using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
using Xunit;

namespace XUnitTest
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
        public void TestAddAccountFail()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement();
            account.Name = "Dana";
            account.Email = "zaidskate@hotmail.com";
            account.Lastname = "Paz";
            account.Password = "password123";

            bool result = proxy.AddAccount(account);
            Assert.False(result);
        }

        [Fact]
        public void TestUpdatePasswordSuccess()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement();
            account.Name = "ZaidVazquez";
            account.Email = "zaidoskate@hotmail.com";
            account.Gamertag = "zaidoskate";
            account.Password = "pos";

            bool result = proxy.AddAccount(account);
            Assert.True(result);
        }

        [Fact]
        public void TestUpdatePasswordFail()
        {
            ExamExplotionService.AccountManagement account = new ExamExplotionService.AccountManagement();

            bool result = proxy.AddAccount(account);
            Assert.False(result);
        }

        [Fact]
        public void TestVerifyExistingEmailSuccess()
        {
            string emailExisting = "zaidoskate@hotmail.com";
            bool result = proxy.VerifyExistingEmail(emailExisting);

            Assert.True(result);
        }

        [Fact]
        public void TestVerifyExistingEmailFail()
        {
            string emailExisting = "notAnEmail";
            bool result = proxy.VerifyExistingEmail(emailExisting);

            Assert.False(result);
        }

        [Fact]
        public void TestVerifyExistingGamertagSuccess()
        {
            string gamertagExisting = "zaidoskate";
            bool result = proxy.VerifyExistingEmail(gamertagExisting);

            Assert.True(result);
        }

        [Fact]
        public void TestVerifyExistingGamertagFail()
        {
            string gamertagExisting = "gamertagNotExistent";
            bool result = proxy.VerifyExistingEmail(gamertagExisting);

            Assert.False(result);
        }
    }
}