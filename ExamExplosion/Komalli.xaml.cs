using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using ExamExplosion.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamExplosion
{
    /// <summary>
    /// Lógica de interacción para Komalli.xaml
    /// </summary>
    public partial class Komalli : Page
    {
        Dictionary<string, bool> purchasedAccessories = new Dictionary<string, bool>();
        Dictionary<int, int> accessoriesPrice = new Dictionary<int, int>();
        int accessoryIdSelected = 0;
        int points = 0;
        private ILog log;

        public Komalli()
        {
            InitializeComponent();
            InitializeKomalli();
            log = LogManager.GetLogger(typeof(App));
        }

        private void InitializeKomalli()
        {
            int playerId = SessionManager.CurrentSession.userId;
            UpdatePoints();

            accessoriesPrice.Add(1, 0);
            accessoriesPrice.Add(4, 100);
            accessoriesPrice.Add(2, 300);
            accessoriesPrice.Add(3, 500);
            purchasedAccessories.Add("radioBtnNormalPackage", false);
            purchasedAccessories.Add("radioBtnHatPackage", false);
            purchasedAccessories.Add("radioBtnGraduatedPackage", false);
            purchasedAccessories.Add("radioBtnCapPackage", false);

            var idsPurchasedAccessories = PurchasedAccessoryManager.GetPurchasedAccessoriesByPlayer(playerId);
            foreach (var idPurchasedAccessory in idsPurchasedAccessories)
            {
                switch (idPurchasedAccessory)
                {
                    case 1:
                        purchasedAccessories["radioBtnNormalPackage"] = true;
                        break;
                    case 2:
                        purchasedAccessories["radioBtnHatPackage"] = true;
                        break;
                    case 3:
                        purchasedAccessories["radioBtnGraduatedPackage"] = true;
                        break;
                    case 4:
                        purchasedAccessories["radioBtnCapPackage"] = true;
                        break;
                }
            }

            Accessory accessory = null;
            try
            {
                accessory = PurchasedAccessoryManager.GetAccessoryInUse(playerId);
                accessoryIdSelected = accessory.accessoryId;
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                log.Error("Error del servidor (FaultException)", faultException);
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                log.Warn("Problema de comunicación con el servidor", communicationException);
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
            }
            switch (accessoryIdSelected)
            {
                case 1:
                    radioBtnNormalPackage.IsChecked = true;
                    break;
                case 2:
                    radioBtnHatPackage.IsChecked = true;
                    break;
                case 3:
                    radioBtnGraduatedPackage.IsChecked = true;
                    break;
                case 4:
                    radioBtnCapPackage.IsChecked = true;
                    break;
            }
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
            }
        }
        private void BuyPackage(object sender, RoutedEventArgs e)
        {
            PurchasedAccessory purchasedAccessory = new PurchasedAccessory();
            purchasedAccessory.playerId = SessionManager.CurrentSession.accountId;
            purchasedAccessory.accessoryId = accessoryIdSelected;
            purchasedAccessory.inUse = false;
            if (points < accessoriesPrice[accessoryIdSelected])
            {
                new AlertModal("Puntos insuficientes", "Necesitas ganar mas partidas para poder comprarlo.").ShowDialog();
            }
            else
            {
                try
                {
                    PurchasedAccessoryManager.PurchaseAccessory(purchasedAccessory);
                    new AlertModal("Accesorio comprado", "Ahora puedes utilizar este paquete de cartas para visualizarlas en tu juego.").ShowDialog();
                    UpdatePurchasedAccessories(accessoryIdSelected);
                    UpdatePoints();
                }
                catch (FaultException faultException)
                {
                    new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                    log.Error("Error del servidor (FaultException)", faultException);
                }
                catch (CommunicationException communicationException)
                {
                    new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                    log.Warn("Problema de comunicación con el servidor", communicationException);
                }
                catch (TimeoutException timeoutException)
                {
                    new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                    log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                }
                
            }
        }
        private void UpdatePoints()
        {
            int playerId = SessionManager.CurrentSession.userId;
            try
            {
                lblPoints.Content = points;
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                log.Error("Error del servidor (FaultException)", faultException);
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                log.Warn("Problema de comunicación con el servidor", communicationException);
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
            }
            points = PlayerManager.GetPointsByPlayerId(playerId);
        }

        private void UpdatePurchasedAccessories(int accessoryIdSelected)
        {
            btnBuyAccessory.IsEnabled = false;
            btnUseAccessory.IsEnabled = true;
            switch (accessoryIdSelected)
            {
                case 1:
                    purchasedAccessories["radioBtnNormalPackage"] = true;
                    break;
                case 2:
                    purchasedAccessories["radioBtnHatPackage"] = true;
                    break;
                case 3:
                    purchasedAccessories["radioBtnGraduatedPackage"] = true;
                    break;
                case 4:
                    purchasedAccessories["radioBtnCapPackage"] = true;
                    break;
            }
        }

        private void UsePackage(object sender, RoutedEventArgs e)
        {
            PurchasedAccessory purchasedAccessory = new PurchasedAccessory();
            purchasedAccessory.playerId = SessionManager.CurrentSession.accountId;
            purchasedAccessory.accessoryId = accessoryIdSelected;
            purchasedAccessory.inUse = true;
            try
            {
                PurchasedAccessoryManager.UseAccessory(purchasedAccessory);
                new AlertModal("Accesorio en uso", "Ahora puedes visualizar las cartas con el estilo adquirido.").ShowDialog();
            }
            catch (FaultException faultException)
            {
                new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                log.Error("Error del servidor (FaultException)", faultException);
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                log.Warn("Problema de comunicación con el servidor", communicationException);
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
            }
        }
        private void PackageSelected(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                string nombre = radioButton.Name;
                if (purchasedAccessories[nombre] == true)
                {
                    btnBuyAccessory.IsEnabled = false;
                    btnUseAccessory.IsEnabled = true;  
                }
                else
                {
                    btnBuyAccessory.IsEnabled = true;
                    btnUseAccessory.IsEnabled = false;
                }
                switch(nombre)
                {
                    case "radioBtnNormalPackage":
                        accessoryIdSelected = 1;
                        break;

                    case "radioBtnHatPackage":
                        accessoryIdSelected = 2;
                        break;

                    case "radioBtnGraduatedPackage":
                        accessoryIdSelected = 3;
                        break;

                    case "radioBtnCapPackage":
                        accessoryIdSelected = 4;
                        break;
                }
            }
        }

    }
}
