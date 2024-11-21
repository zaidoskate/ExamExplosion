using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public class PurchasedAccessoryManager
    {
        public static List<int> GetPurchasedAccessoriesByPlayer(int playerId)
        {
            using (var proxy = new AccessoryManagerClient())
            {
                var purchasedAccessories = proxy.GetPurchasedAccessories(playerId);
                return purchasedAccessories.ToList();
            }
        }

        public static Accessory GetAccessoryInUse(int playerId)
        {
            using (var proxy = new AccessoryManagerClient())
            {
                AccessoryManagement purchasedAccessory = proxy.GetAccessoryInUse(playerId);
                Accessory accessory = new Accessory
                {
                    accessoryId = purchasedAccessory.AccessoryId,
                    name = purchasedAccessory.AccessoryName,
                    path = purchasedAccessory.Path
                };
                return accessory;
            }
        }

        public static bool PurchaseAccessory(PurchasedAccessory purchasedAccessory)
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement
            {
                AccesoryId = purchasedAccessory.accessoryId,
                PlayerId = purchasedAccessory.playerId,
                InUse = purchasedAccessory.inUse
            };

            using (var proxy = new AccessoryManagerClient())
            {
                return proxy.PurchaseAccessory(purchasedAccessoryManagement);
            }
        }

        public static bool UseAccessory(PurchasedAccessory purchasedAccessory)
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement
            {
                AccesoryId = purchasedAccessory.accessoryId,
                PlayerId = purchasedAccessory.playerId,
                InUse = purchasedAccessory.inUse
            };

            using (var proxy = new AccessoryManagerClient())
            {
                return proxy.SetAccessoryInUse(purchasedAccessoryManagement);
            }
        }
    }
}
