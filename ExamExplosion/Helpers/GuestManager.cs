﻿using ExamExplosion.ExamExplotionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public class GuestManager
    {
        public static bool AddGuest()
        {
            bool newGuestAdded = false;
            ExamExplotionService.PlayerManagerClient proxy = new ExamExplotionService.PlayerManagerClient();
            GuestManagement newGuest = null;
            try
            {
                newGuest = proxy.AddGuest();
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
            if (newGuest != null )
            {
                SessionManager.CurrentSession.isGuest = true;
                SessionManager.CurrentSession.isLobbyOwner = false;
                SessionManager.CurrentSession.gamertag = $"GUEST_{newGuest.GuestNumber}";
                SessionManager.CurrentSession.userId = newGuest.UserId ;
                SessionManager.CurrentSession.accountId = -1;
                newGuestAdded = true;
            }
            return newGuestAdded;
        }
    }
}
