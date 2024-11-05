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
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : Page
    {
        private List<Label> playersInGame = new List<Label>();
        private List<Image> playerImages = new List<Image>();
        public Board(List<Label> playerGamertags)
        {
            InitializeComponent();
            InitializeBoard(playerGamertags);
        }

        private void InitializeBoard(List<Label> playerGamertags)
        {
            playersInGame.Add(this.player1Lbl);
            playersInGame.Add(this.player2Lbl);
            playersInGame.Add(this.player3Lbl);
            playersInGame.Add(this.player4Lbl);

            playerImages.Add(this.player1Image);
            playerImages.Add(this.player2Image);
            playerImages.Add(this.player3Image);
            playerImages.Add(this.player4Image);

            for (int i = 0; i < playerGamertags.Count; i++)
            {
                playersInGame[i].Content = playerGamertags[i].Content;
            }

            for (int i = 0; i < playersInGame.Count; i++)
            {
                Label label = playersInGame[i];
                Image image = playerImages[i];

                if (!string.IsNullOrEmpty(label.Content?.ToString()))
                {
                    label.Visibility = Visibility.Visible;
                    image.Visibility = Visibility.Visible;
                }
                else
                {
                    label.Visibility = Visibility.Collapsed;
                    image.Visibility = Visibility.Collapsed;
                }
            }
        }

    }
}
