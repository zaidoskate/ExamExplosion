using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ExamExplosion.Helpers
{
    public static class FriendsAndBloquedPlayersManager
    {
        public static Dictionary<int, string> GetBlockedPlayers(int playerId)
        {
            try
            {
                using (var proxy = new ExamExplotionService.FriendAndBlockListClient())
                {
                    return proxy.GetBlockedGamertags(playerId);
                }
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
        }
        public static Dictionary<int, string> GetFriendsList(int playerId)
        {
            try
            {
                using (var proxy = new ExamExplotionService.FriendAndBlockListClient())
                {
                    return proxy.GetFriendsGamertags(playerId);
                }
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
        }
        public static void RemoveBlockedPlayer(int playerId, int bloquedPlayerId)
        {
            ExamExplotionService.BlockListManagement blockToRemove = new ExamExplotionService.BlockListManagement();
            blockToRemove.PlayerId = playerId;
            blockToRemove.BlockedPlayerId = bloquedPlayerId;
            try
            {
                using (var proxy = new ExamExplotionService.FriendAndBlockListClient())
                {
                    proxy.RemoveBlock(blockToRemove);
                }
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
        }
        public static bool RemoveFriends(int playerId1, int playerId2)
        {
            ExamExplotionService.FriendManagement friendToRemove = new ExamExplotionService.FriendManagement();
            friendToRemove.Player1Id = playerId1;
            friendToRemove.Player2Id = playerId2;
            bool friendRemoved = false;
            try
            {
                using (var proxy = new ExamExplotionService.FriendAndBlockListClient())
                {
                    friendRemoved = proxy.RemoveFriend(friendToRemove);
                }
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
            return friendRemoved;
        }
        public static int AddFriend(int playerId1, int playerId2)
        {
            ExamExplotionService.FriendManagement friendToAdd = new ExamExplotionService.FriendManagement();
            friendToAdd.Player1Id = playerId1;
            friendToAdd.Player2Id = playerId2;
            int friendAdded = -1;
            try
            {
                using (var proxy = new ExamExplotionService.FriendAndBlockListClient())
                {
                    friendAdded = proxy.AddFriend(friendToAdd);
                }
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
            return friendAdded;
        }
        public static int AddBlockedPlayer(int playerId, int bloquedPlayerId)
        {
            ExamExplotionService.BlockListManagement blockToAdd = new ExamExplotionService.BlockListManagement();
            blockToAdd.PlayerId = playerId;
            blockToAdd.BlockedPlayerId = bloquedPlayerId;
            int blockAdded = -1;
            try
            {
                using (var proxy = new ExamExplotionService.FriendAndBlockListClient())
                {
                    proxy.AddBlock(blockToAdd);
                }
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
            return blockAdded;
        }
    }
}
