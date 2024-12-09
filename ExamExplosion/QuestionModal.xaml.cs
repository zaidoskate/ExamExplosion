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
    /// Lógica de interacción para QuestionModal.xaml
    /// </summary>
    public partial class QuestionModal : Window
    {
        public QuestionModal(string title, string message)
        {
            InitializeComponent();
            titleLbl.Content = title;
            errorMessageTxtBox.Text = message;
        }

        private void AcceptMessage(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void CancelMessage(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
