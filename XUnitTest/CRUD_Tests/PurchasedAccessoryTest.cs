using ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest.CRUD_Tests
{
    public class PurchasedAccessoryTest
    {
        [Fact]
        public void TestUseAccessory()
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement
            {
                AccesoryId = 1,
                PlayerId = 33,
                InUse = true
            };
            bool accessoryInUse = false;
            using (var proxy = new AccessoryManagerClient())
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
                PlayerId = 33,
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
                AccessoryManagement purchasedAccessory = proxy.GetAccessoryInUse(33);
                currentAccessoryId = purchasedAccessory.AccessoryId;
            }
            int expectedAccessoryId = 1;
            Assert.Equal(expectedAccessoryId, currentAccessoryId);
        }
    }
}
