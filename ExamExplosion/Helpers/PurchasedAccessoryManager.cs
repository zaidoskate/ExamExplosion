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
        private static ExamExplotionService.AccessoryManagerClient proxy;

        public static List<int> GetPurchasedAccessoriesByPlayer(int playerId)
        {
            proxy = new ExamExplotionService.AccessoryManagerClient();
            var purchasedAccessories = proxy.GetPurchasedAccessories(playerId);
            proxy.Close();
            return purchasedAccessories.ToList();
        }
        public static Accessory GetAccessoryInUse(int playerId)
        {
            proxy = new ExamExplotionService.AccessoryManagerClient();
            AccessoryManagement purchasedAccessory = proxy.GetAccessoryInUse(playerId);
            proxy.Close();
            Accessory accessory = new Accessory();
            accessory.accessoryId = purchasedAccessory.AccessoryId;
            accessory.name = purchasedAccessory.AccessoryName;
            accessory.path = purchasedAccessory.Path;
            return accessory;
        }
        public static bool PurchaseAccessory(PurchasedAccessory purchasedAccessory)
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement();
            purchasedAccessoryManagement.AccesoryId = purchasedAccessory.accessoryId;
            purchasedAccessoryManagement.PlayerId = purchasedAccessory.playerId;
            purchasedAccessoryManagement.InUse = purchasedAccessoryManagement.InUse;

            proxy = new ExamExplotionService.AccessoryManagerClient();
            bool purchasedAccessoryAdded = proxy.PurchaseAccessory(purchasedAccessoryManagement);
            proxy.Close();
            return purchasedAccessoryAdded;
        }
        public static bool UseAccessory(PurchasedAccessory purchasedAccessory)
        {
            PurchasedAccessoryManagement purchasedAccessoryManagement = new PurchasedAccessoryManagement();
            purchasedAccessoryManagement.AccesoryId = purchasedAccessory.accessoryId;
            purchasedAccessoryManagement.PlayerId = purchasedAccessory.playerId;
            purchasedAccessoryManagement.InUse = purchasedAccessoryManagement.InUse;

            proxy = new ExamExplotionService.AccessoryManagerClient();
            bool purchasedAccessoryAdded = proxy.SetAccessoryInUse(purchasedAccessoryManagement);
            proxy.Close();
            return purchasedAccessoryAdded;
        }
    }
}
