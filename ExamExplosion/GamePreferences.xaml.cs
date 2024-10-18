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
using System.Windows.Shapes;

namespace ExamExplosion
{
    /// <summary>
    /// Lógica de interacción para GamePreferences.xaml
    /// </summary>
    public partial class GamePreferences : Page
    {
        public GamePreferences()
        {
            InitializeComponent();
            InitializeSliderValues();
        }

        private void InitializeSliderValues()
        {
            MaxPlayersValue.Text = ((int)MaxPlayersSlider.Minimum).ToString();
            TimePerTurnValue.Text = ((int)TimePerTurnSlider.Minimum).ToString();
            MaxHPValue.Text = ((int)MaxHPSlider.Minimum).ToString();
        }
        private void MaxPlayersSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MaxPlayersValue != null)
            {
                MaxPlayersValue.Text = ((int)e.NewValue).ToString();
            }
        }

        private void TimePerTurnSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TimePerTurnValue != null)
            {
                TimePerTurnValue.Text = ((int)e.NewValue).ToString();
            }
        }

        private void MaxHPSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MaxHPValue != null)
            {
                MaxHPValue.Text = ((int)e.NewValue).ToString();
            }
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 450;
                    window.Width = 800;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }
        }

        private void CreateLobby(object sender, RoutedEventArgs e)
        {
            int maxPlayers = (int)MaxPlayersSlider.Value;
            int timePerTurn = (int)TimePerTurnSlider.Value;
            int maxHP = (int)MaxHPSlider.Value;

            Lobby lobby = new Lobby(maxPlayers, timePerTurn, maxHP, SessionManager.CurrentSession.gamertag);

            NavigationService.Navigate(lobby);
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 450;
                window.Width = 800;
                window.SizeToContent = SizeToContent.Manual;
            }
        }
    }
}
