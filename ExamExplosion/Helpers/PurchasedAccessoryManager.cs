using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamExplosion.Helpers
{
    /// <summary>
    /// Administra las funcionalidades relacionadas con los accesorios comprados por los jugadores, 
    /// incluyendo su obtención, compra y asignación como en uso.
    /// </summary>
    public class PurchasedAccessoryManager
    {
        /// <summary>
        /// Obtiene la lista de identificadores de accesorios comprados por un jugador específico.
        /// </summary>
        /// <param name="playerId">El identificador único del jugador.</param>
        /// <returns>Una lista de identificadores de los accesorios comprados por el jugador.</returns>
        public static List<int> GetPurchasedAccessoriesByPlayer(int playerId)
        {
            using (var proxy = new AccessoryManagerClient())
            {
                var purchasedAccessories = proxy.GetPurchasedAccessories(playerId);
                return purchasedAccessories.ToList();
            }
        }

        /// <summary>
        /// Obtiene la información del accesorio actualmente en uso por un jugador específico.
        /// </summary>
        /// <param name="playerId">El identificador único del jugador.</param>
        /// <returns>Un objeto <see cref="Accessory"/> que contiene los detalles del accesorio en uso.</returns>
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

        /// <summary>
        /// Registra la compra de un accesorio por parte de un jugador.
        /// </summary>
        /// <param name="purchasedAccessory">Un objeto <see cref="PurchasedAccessory"/> con los detalles de la compra.</param>
        /// <returns>Un valor booleano que indica si la compra fue exitosa.</returns>
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

        /// <summary>
        /// Asigna un accesorio como el accesorio en uso por parte de un jugador.
        /// </summary>
        /// <param name="purchasedAccessory">Un objeto <see cref="PurchasedAccessory"/> con los detalles del accesorio a usar.</param>
        /// <returns>Un valor booleano que indica si la operación fue exitosa.</returns>
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
