using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public static class SessionManager
    {
        public static SessionInfo CurrentSession
        {
            get
            {
                if (_currentSession == null)
                {
                    _currentSession = new SessionInfo();
                }
                return _currentSession;
            }
        }
        private static SessionInfo _currentSession;
    }

    public class SessionInfo
    {
        public string gamertag { get; set; }
        public string currentLobbyCode { get; set; }
        public int userId { get; set; }
        public int accountId { get; set; }
        public bool isLobbyOwner { get; set; }
        public bool isGuest { get; set; }

    }
}
