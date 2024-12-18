using System;
using System.ServiceModel; // Para CommunicationException
using Xunit;
using ExamExplotionService;
using ExamExplosion.Models;

namespace XUnitTest.Exceptions
{
    public class PurchasedAccessoryTestException
    {
        [Fact]
        public void TestUseAccessoryThrowsCommunicationException()
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement
            {
                AccesoryId = 1,
                PlayerId = 33,
                InUse = true
            };
            Assert.Throws<CommunicationException>(() =>
            {
                using (var proxy = new AccessoryManagerClient())
                {
                    proxy.SetAccessoryInUse(purchasedAccessoryManagement);
                }
            });
        }

        [Fact]
        public void PurchaseAccessoryThrowsCommunicationException()
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement
            {
                AccesoryId = 1,
                PlayerId = 33,
                InUse = false
            };

            Assert.Throws<CommunicationException>(() =>
            {
                using (var proxy = new AccessoryManagerClient())
                {
                    proxy.PurchaseAccessory(purchasedAccessoryManagement);
                }
            });
        }

        [Fact]
        public void GetAccessoryInUseThrowsCommunicationException()
        {
            Assert.Throws<CommunicationException>(() =>
            {
                using (var proxy = new AccessoryManagerClient())
                {
                    proxy.GetAccessoryInUse(33);
                }
            });
        }
    }
}
