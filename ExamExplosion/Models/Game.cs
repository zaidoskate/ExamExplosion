using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Models
{
    public class Game
    {
        public int TimePerTurn { get; set; }
        public string InvitationCode { get; set; }
        public int Lives { get; set; }
        public int NumberPlayers { get; set; }
        public int HostPlayerId { get; set; }
    }
}