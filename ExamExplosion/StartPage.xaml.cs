using ExamExplosion.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            buttonPlay.Focus();
        }

        private void DisplayLogin(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Login());
            }
        }
        private void DisplayJoinLobby(object sender, RoutedEventArgs e)
        {
            bool newGuestAdded = GuestManager.AddGuest();
            if (!newGuestAdded)
            {
                new AlertModal("Error","Ha ocurrido un error, crea una cuenta o intentalo mas tarde.").ShowDialog();
                return;
            }
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new GameCode());
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 420;
                    window.Width = 420;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }

        }
        private void DisplayCreateAccount(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new AccountCreation());
            }
        }
    }
}
