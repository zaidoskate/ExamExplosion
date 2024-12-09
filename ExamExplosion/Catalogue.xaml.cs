using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ExamExplosion.Properties;

namespace ExamExplosion
{
    public partial class Catalogue : Page
    {
        private readonly string[] imageSources = new string[]
        {
            "pack://application:,,,/CardsPackages/NormalPackage/Please.png",
            "pack://application:,,,/CardsPackages/NormalPackage/examBomb.png",
            "pack://application:,,,/CardsPackages/NormalPackage/exempt.png",
            "pack://application:,,,/CardsPackages/NormalPackage/leftTeam.png",
            "pack://application:,,,/CardsPackages/NormalPackage/profeA.png",
            "pack://application:,,,/CardsPackages/NormalPackage/profeM.png",
            "pack://application:,,,/CardsPackages/NormalPackage/profeO.png",
            "pack://application:,,,/CardsPackages/NormalPackage/profeS.png",
            "pack://application:,,,/CardsPackages/NormalPackage/profeR.png",
            "pack://application:,,,/CardsPackages/NormalPackage/reRegistration.png",
            "pack://application:,,,/CardsPackages/NormalPackage/shuffle.png",
            "pack://application:,,,/CardsPackages/NormalPackage/takeFromBelow.png",
            "pack://application:,,,/CardsPackages/NormalPackage/viewTheFuture.png"
        };

        private string[] titles = new string[]
        {
            ExamExplosion.Properties.Resources.globalLblFavorCard,
            ExamExplosion.Properties.Resources.globalLblExamBombCard,
            ExamExplosion.Properties.Resources.globalLblExemptCard,
            ExamExplosion.Properties.Resources.globalLblLeftTeamCard,
            ExamExplosion.Properties.Resources.globalLblTeacherACard,
            ExamExplosion.Properties.Resources.globalLblTeacherMCard,
            ExamExplosion.Properties.Resources.globalLblTeacherOCard,
            ExamExplosion.Properties.Resources.globalLblTeacherSCard,
            ExamExplosion.Properties.Resources.globalLblTeacherRCard,
            ExamExplosion.Properties.Resources.globalReRegistrationCard,
            ExamExplosion.Properties.Resources.globalLblShuffleCard,
            ExamExplosion.Properties.Resources.globalLblTakeBelowCard,
            ExamExplosion.Properties.Resources.globalLblViewFutureCard,
        };

        private string[] descriptions = new string[]
        {
            ExamExplosion.Properties.Resources.catalogueLblFavorDescription,
            ExamExplosion.Properties.Resources.catalogueLblExamBombDescription,
            ExamExplosion.Properties.Resources.catalogueLblExemptDescription,
            ExamExplosion.Properties.Resources.catalogueLblLeftTeamDescription,
            ExamExplosion.Properties.Resources.catalogueLblTeacherDescription,
            ExamExplosion.Properties.Resources.catalogueLblTeacherDescription,
            ExamExplosion.Properties.Resources.catalogueLblTeacherDescription,
            ExamExplosion.Properties.Resources.catalogueLblTeacherDescription,
            ExamExplosion.Properties.Resources.catalogueLblTeacherDescription,
            ExamExplosion.Properties.Resources.catalogueLblReRegistrationDescription,
            ExamExplosion.Properties.Resources.catalogueLblShuffleDescription,
            ExamExplosion.Properties.Resources.catalogueLblTakeBelowDescription,
            ExamExplosion.Properties.Resources.catalogueLblViewFutureDescription
        };

        private int currentIndex = 0;

        public Catalogue()
        {
            InitializeComponent();
            UpdateCard();
            this.KeyDown += OnKeyDown;
        }

        private void ShowLeftCardBtn_Click(object sender, RoutedEventArgs e)
        {
            currentIndex--;
            UpdateCard();
        }

        private void ShowRightCardBtn_Click(object sender, RoutedEventArgs e)
        {
            currentIndex++;
            UpdateCard();
        }

        private void UpdateCard()
        {
            cardImg.Source = new BitmapImage(new Uri(imageSources[currentIndex], UriKind.Absolute));
            titleCardLbl.Content = titles[currentIndex];
            descriptionTxtBlock.Text = descriptions[currentIndex];

            showLeftCardBtn.IsEnabled = currentIndex > 0;
            showRightCardBtn.IsEnabled = currentIndex < imageSources.Length - 1;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && showLeftCardBtn.IsEnabled)
            {
                ShowLeftCardBtn_Click(sender, e);
            }
            else if (e.Key == Key.Right && showRightCardBtn.IsEnabled)
            {
                ShowRightCardBtn_Click(sender, e);
            }
        }

        private void NavigateHomePage(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
            }
        }
    }
}
