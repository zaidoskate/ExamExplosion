using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ExamExplosion
{
    public partial class Catalogue : Page
    {
        private string[] imageSources = new string[]
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
            "Paro",
            "Repite",
            "Excentar",
            "Deja el equipo",
            "Profe A",
            "Profe M",
            "Profe O",
            "Profe S",
            "Profe R",
            "Reinscripcion",
            "Revolver",
            "Tomar de abajo",
            "Ver el futuro",
        };

        private string[] descriptions = new string[]
        {
            "Te da una carta el jugador de tu eleccion.",
            "Has perdido una vida a menos de que puedas reinscribirte.",
            "Termina tu turno sin tomar una carta.",
            "Se le pasa tu turno a la siguiente persona.",
            "Junta dos cartas del mismo profe y te debe dar una carta un jugador",
            "Junta dos cartas del mismo profe y te debe dar una carta un jugador",
            "Junta dos cartas del mismo profe y te debe dar una carta un jugador",
            "Junta dos cartas del mismo profe y te debe dar una carta un jugador",
            "Junta dos cartas del mismo profe y te debe dar una carta un jugador",
            "Te recuperas de un Repite, no pierdes tu vida",
            "Revuelves las 3 cartas del tope de las cartas",
            "Tomas una carta de abajo del stack",
            "Puedes ver las 3 proximas cartas del Stack sin revolver."
        };

        private int currentIndex = 0;

        public Catalogue()
        {
            InitializeComponent();
            UpdateCard();
            this.KeyDown += OnKeyDown;
        }

        private void ShowLeftCard(object sender, RoutedEventArgs e)
        {
            currentIndex--;
            UpdateCard();
        }

        private void ShowRightCard(object sender, RoutedEventArgs e)
        {
            currentIndex++;
            UpdateCard();
        }

        private void UpdateCard()
        {
            imgCard.Source = new BitmapImage(new Uri(imageSources[currentIndex], UriKind.Absolute));
            lblTitleCard.Content = titles[currentIndex];
            txtBlockDescription.Text = descriptions[currentIndex];

            btnShowLeftCard.IsEnabled = currentIndex > 0;
            btnShowRightCard.IsEnabled = currentIndex < imageSources.Length - 1;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && btnShowLeftCard.IsEnabled)
            {
                ShowLeftCard(sender, e);
            }
            else if (e.Key == Key.Right && btnShowRightCard.IsEnabled)
            {
                ShowRightCard(sender, e);
            }
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
            }
        }
    }
}
