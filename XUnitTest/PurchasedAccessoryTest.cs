using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest
{
    public class PurchasedAccessoryTest
    {
        [Fact]
        public void TestUseAccessory()
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement
            {
                AccesoryId = 1,
                PlayerId = 22,
                InUse = true
            };
            bool accessoryInUse = false;
            using (var proxy = new ExamExplotionService.AccessoryManagerClient())
            {
                accessoryInUse = proxy.SetAccessoryInUse(purchasedAccessoryManagement);
            }
            Assert.True(accessoryInUse);
        }
        [Fact]
        public void PurchaseAccessory()
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement
            {
                AccesoryId = 1,
                PlayerId = 16,
                InUse = false
            };
            bool accessoryPurchased = false;
            using (var proxy = new AccessoryManagerClient())
            {
                accessoryPurchased = proxy.PurchaseAccessory(purchasedAccessoryManagement);
            }
            Assert.True(accessoryPurchased);
        }
        [Fact]
        public void GetAccessoryInUse()
        {
            int currentAccessoryId = -1;
            using (var proxy = new AccessoryManagerClient())
            {
                AccessoryManagement purchasedAccessory = proxy.GetAccessoryInUse(17);
                currentAccessoryId = purchasedAccessory.AccessoryId;
            }
            int expectedAccessoryId = 1;
            Assert.Equal(expectedAccessoryId, currentAccessoryId);
        }
        public static List<int> GetPurchasedAccessoriesByPlayer(int playerId)
        {
            using (var proxy = new AccessoryManagerClient())
            {
                var purchasedAccessories = proxy.GetPurchasedAccessories(playerId);
                return purchasedAccessories.ToList();
            }
        }
    }
}
