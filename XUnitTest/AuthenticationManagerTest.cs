using ExamExplosion;
using ExamExplosion.Helpers;
using ExamExplosion.Models;

namespace XUnitTest
{
    public class AuthenticationManagerTest
    {
        private static ExamExplotionService.AuthenticationManagerClient proxy = new ExamExplotionService.AuthenticationManagerClient();
        [Fact]
        public async void TestAddAccountSuccess()
        {
            ExamExplotionService.AccountM account = new ExamExplotionService.AccountM();
            account.Name = "Dana";
            account.Email = "zaidskate@hotmail.com";
            account.Lastname = "Paz";
            account.Gamertag = "didarkrai";
            account.Password = "password123";

            bool result = await proxy.AddAccountAsync(account);
            Assert.True(result);
        }

        [Fact]
        public async void TestAddAccountFail()
        {
            ExamExplotionService.AccountM account = new ExamExplotionService.AccountM();
            account.Name = "Dana";
            account.Email = "zaidskate@hotmail.com";
            account.Lastname = "Paz";
            account.Password = "password123";

            bool result = await proxy.AddAccountAsync(account);
            Assert.False(result);
        }

        [Fact]
        public async void TestUpdatePasswordSuccess()
        {
            ExamExplotionService.AccountM account = new ExamExplotionService.AccountM();
            account.Name = "ZaidVazquez";
            account.Email = "zaidoskate@hotmail.com";
            account.Gamertag = "zaidoskate";
            account.Password = "pos";

            bool result = await proxy.UpdatePasswordAsync(account);
            Assert.True(result);
        }

        [Fact]
        public async void TestUpdatePasswordFail()
        {
            ExamExplotionService.AccountM account = new ExamExplotionService.AccountM();

            bool result = await proxy.UpdatePasswordAsync(account);
            Assert.False(result);
        }

        [Fact]
        public async void TestVerifyExistingEmailSuccess()
        {
            string emailExisting = "zaidoskate@hotmail.com";
            bool result = await proxy.VerifyExistingEmailAsync(emailExisting);

            Assert.True(result);
        }

        [Fact]
        public async void TestVerifyExistingEmailFail()
        {
            string emailExisting = "notAnEmail";
            bool result = await proxy.VerifyExistingEmailAsync(emailExisting);

            Assert.False(result);
        }

        [Fact]
        public async void TestVerifyExistingGamertagSuccess()
        {
            string gamertagExisting = "zaidoskate";
            bool result = await proxy.VerifyExistingEmailAsync(gamertagExisting);

            Assert.True(result);
        }

        [Fact]
        public async void TestVerifyExistingGamertagFail()
        {
            string gamertagExisting = "gamertagNotExistent";
            bool result = await proxy.VerifyExistingEmailAsync(gamertagExisting);

            Assert.False(result);
        }
    }
}