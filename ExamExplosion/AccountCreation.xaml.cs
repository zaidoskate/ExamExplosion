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
    /// <summary>
    /// Lógica de interacción para AccountCreation.xaml
    /// </summary>
    public partial class AccountCreation : Page
    {
        public AccountCreation()
        {
            InitializeComponent();
        }

        private void ValidateEmail(object sender, RoutedEventArgs e)
        {

        }

        private void CancelAccountCreation(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new StartPage());
            }
        }
    }
}
