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
        readonly Dictionary<string, bool> purchasedAccessories = new Dictionary<string, bool>();
        readonly Dictionary<int, int> accessoriesPrice = new Dictionary<int, int>();
        int accessoryIdSelected = 0;
        int points = 0;
        private readonly ILog log;

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
            purchasedAccessories.Add("normalPackageRadioBtn", false);
            purchasedAccessories.Add("hatPackageRadioBtn", false);
            purchasedAccessories.Add("graduatedPackageRadioBtn", false);
            purchasedAccessories.Add("capPackageRadioBtn", false);

            var idsPurchasedAccessories = PurchasedAccessoryManager.GetPurchasedAccessoriesByPlayer(playerId);
            if (idsPurchasedAccessories.Any())
            {
                foreach (var idPurchasedAccessory in idsPurchasedAccessories)
                {
                    switch (idPurchasedAccessory)
                    {
                        case 1:
                            purchasedAccessories["normalPackageRadioBtn"] = true;
                            break;
                        case 2:
                            purchasedAccessories["hatPackageRadioBtn"] = true;
                            break;
                        case 3:
                            purchasedAccessories["graduatedPackageRadioBtn"] = true;
                            break;
                        case 4:
                            purchasedAccessories["capPackageRadioBtn"] = true;
                            break;
                        default:
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
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblFaultException).ShowDialog();
                    log.Error("Error del servidor (FaultException)", faultException);
                    NavigateStartPage();
                }
                catch (CommunicationException communicationException)
                {
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblCommunicationException).ShowDialog();
                    log.Warn("Problema de comunicación con el servidor", communicationException);
                    NavigateStartPage();
                }
                catch (TimeoutException timeoutException)
                {
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblTimeoutException).ShowDialog();
                    log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                    NavigateStartPage();
                }
                switch (accessoryIdSelected)
                {
                    case 1:
                        normalPackageRadioBtn.IsChecked = true;
                        break;
                    case 2:
                        hatPackageRadioBtn.IsChecked = true;
                        break;
                    case 3:
                        graduatedPackageRadioBtn.IsChecked = true;
                        break;
                    case 4:
                        capPackageRadioBtn.IsChecked = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.komalliLblLoadingPackagesError).ShowDialog();
            }
        }

        private void NavigateHomePage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
            }
        }
        private void BuyPackageBtn_Click(object sender, RoutedEventArgs e)
        {
            PurchasedAccessory purchasedAccessory = new PurchasedAccessory();
            purchasedAccessory.playerId = SessionManager.CurrentSession.accountId;
            purchasedAccessory.accessoryId = accessoryIdSelected;
            purchasedAccessory.inUse = false;
            if (points < accessoriesPrice[accessoryIdSelected])
            {
                new AlertModal(ExamExplosion.Properties.Resources.komalliLblInsuficientPointsTitle, ExamExplosion.Properties.Resources.komalliLblInsuficientPoints).ShowDialog();
            }
            else
            {
                try
                {
                    PurchasedAccessoryManager.PurchaseAccessory(purchasedAccessory);
                    new AlertModal(ExamExplosion.Properties.Resources.komalliLblPackPurchasedTitle, ExamExplosion.Properties.Resources.komalliLblPackPurchased).ShowDialog();
                    UpdatePurchasedAccessories(accessoryIdSelected);
                    UpdatePoints();
                }
                catch (FaultException faultException)
                {
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblFaultException).ShowDialog();
                    log.Error("Error del servidor (FaultException)", faultException);
                    NavigateStartPage();
                }
                catch (CommunicationException communicationException)
                {
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblCommunicationException).ShowDialog();
                    log.Warn("Problema de comunicación con el servidor", communicationException);
                    NavigateStartPage();
                }
                catch (TimeoutException timeoutException)
                {
                    new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblTimeoutException).ShowDialog();
                    log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                    NavigateStartPage();
                }
                
            }
        }
        private void UpdatePoints()
        {
            int playerId = SessionManager.CurrentSession.userId;
            try
            {
                points = PlayerManager.GetPointsByPlayerId(playerId);
                pointsLbl.Content = points;
            }
            catch (FaultException faultException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblFaultException).ShowDialog();
                log.Error("Error del servidor (FaultException)", faultException);
                NavigateStartPage();
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblCommunicationException).ShowDialog();
                log.Warn("Problema de comunicación con el servidor", communicationException);
                NavigateStartPage();
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblTimeoutException).ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                NavigateStartPage();
            }
        }

        private void UpdatePurchasedAccessories(int accessoryIdSelected)
        {
            buyAccessoryBtn.IsEnabled = false;
            useAccessoryBtn.IsEnabled = true;
            switch (accessoryIdSelected)
            {
                case 1:
                    purchasedAccessories["normalPackageRadioBtn"] = true;
                    break;
                case 2:
                    purchasedAccessories["hatPackageRadioBtn"] = true;
                    break;
                case 3:
                    purchasedAccessories["graduatedPackageRadioBtn"] = true;
                    break;
                case 4:
                    purchasedAccessories["capPackageRadioBtn"] = true;
                    break;
                default:
                    break;
            }
        }

        private void UsePackageBtn_Click(object sender, RoutedEventArgs e)
        {
            PurchasedAccessory purchasedAccessory = new PurchasedAccessory();
            purchasedAccessory.playerId = SessionManager.CurrentSession.accountId;
            purchasedAccessory.accessoryId = accessoryIdSelected;
            purchasedAccessory.inUse = true;
            try
            {
                PurchasedAccessoryManager.UseAccessory(purchasedAccessory);
                new AlertModal(ExamExplosion.Properties.Resources.komalliLblAccessoryInUseTitle, ExamExplosion.Properties.Resources.komalliLblAccessoryInUse).ShowDialog();
            }
            catch (FaultException faultException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblFaultException).ShowDialog();
                log.Error("Error del servidor (FaultException)", faultException);
                NavigateStartPage();
            }
            catch (CommunicationException communicationException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblCommunicationException).ShowDialog();
                log.Warn("Problema de comunicación con el servidor", communicationException);
                NavigateStartPage();
            }
            catch (TimeoutException timeoutException)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError, ExamExplosion.Properties.Resources.globalLblTimeoutException).ShowDialog();
                log.Warn("Timeout al intentar conectar con el servidor", timeoutException);
                NavigateStartPage();
            }
        }
        private void PackageSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                string nombre = radioButton.Name;
                if (purchasedAccessories[nombre] == true)
                {
                    buyAccessoryBtn.IsEnabled = false;
                    useAccessoryBtn.IsEnabled = true;  
                }
                else
                {
                    buyAccessoryBtn.IsEnabled = true;
                    useAccessoryBtn.IsEnabled = false;
                }
                switch(nombre)
                {
                    case "normalPackageRadioBtn":
                        accessoryIdSelected = 1;
                        break;

                    case "hatPackageRadioBtn":
                        accessoryIdSelected = 2;
                        break;

                    case "graduatedPackageRadioBtn":
                        accessoryIdSelected = 3;
                        break;

                    case "capPackageRadioBtn":
                        accessoryIdSelected = 4;
                        break;
                    default:
                        break;
                }
            }
        }
        private void NavigateStartPage()
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
    }
}
