using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public static class SessionManager
    {
        public static SessionInfo CurrentSession { get; set; }
    }

    public class SessionInfo
    {
        public string gamertag { get; set; }
        public int userId { get; set; }
        public int accountId {  get; set; }
    }
}
