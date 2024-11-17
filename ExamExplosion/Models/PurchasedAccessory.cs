using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Models
{
    public class PurchasedAccessory
    {
        public int accessoryId { get; set; }
        public int playerId { get; set; }
        public bool inUse { get; set; }
    }
}
