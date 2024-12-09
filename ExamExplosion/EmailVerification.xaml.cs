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
using ExamExplosion.Properties;

namespace ExamExplosion
{
    /// <summary>
    /// Lógica de interacción para EmailVerification.xaml
    /// </summary>
    public partial class EmailVerification : Window
    {
        private string code;
        public bool EmailVerified {  get; set; }
        public EmailVerification(string code)
        {
            this.code = code;
            this.EmailVerified = false;
            InitializeComponent();
            txtBoxCode.Focus();
        }

        private void GoOut(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ValidateCode(object sender, RoutedEventArgs e)
        {
            string codeEntered = txtBoxCode.Text;
            if(codeEntered != this.code)
            {
                new AlertModal(ExamExplosion.Properties.Resources.globalLblError , ExamExplosion.Properties.Resources.emailVerificationLblIncorrectCode).ShowDialog();
            }
            else
            {
                EmailVerified = true;
                this.DialogResult = true;
            }
        }
    }
}
