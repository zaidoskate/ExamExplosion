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
using System.Windows.Shapes;

namespace ExamExplosion
{
    /// <summary>
    /// Lógica de interacción para AlertModal.xaml
    /// </summary>
    public partial class AlertModal : Window
    {
        public AlertModal(string title, string message)
        {
            InitializeComponent();
            lblTitle.Content = title;
            lblErrorMessage.Content = message; 
        }

        private void AcceptMessage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
