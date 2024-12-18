using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using XUnitTest.CallbackImplementation;
using ExamExplotionService;

namespace XUnitTest.Services
{
    public class GameManagerTest
    {
        [Fact]
        public void TestNotifyCardReceived()
        {
            var gameCallbacks = new GameCallbacks();
            var card = new CardManagement { CardName = "TestCard" };

            gameCallbacks.NotifyCardReceived(card);

            Assert.Equal("Card received: TestCard", gameCallbacks.NotificationMessage);
        }

        [Fact]
        public void TestReciveNotification()
        {
            var gameCallbacks = new GameCallbacks();
            string notification = "This is a test notification.";

            gameCallbacks.ReciveNotification(notification);

            Assert.Equal(notification, gameCallbacks.NotificationMessage);
        }

        [Fact]
        public void TestReciveAndAddExamBomb()
        {
            var gameCallbacks = new GameCallbacks();

            gameCallbacks.ReciveAndAddExamBomb(1);

            Assert.True(gameCallbacks.IsExamBombReceived);
        }

        [Fact]
        public void TestReceiveGameDeck()
        {
            var gameCallbacks = new GameCallbacks();
            var deck = new Stack<CardManagement>();
            deck.Push(new CardManagement { CardName = "Card1" });
            deck.Push(new CardManagement { CardName = "Card2" });

            gameCallbacks.ReceiveGameDeck(deck);

            Assert.Equal(2, gameCallbacks.GameDeck.Count);
        }

        [Fact]
        public void TestRemoveCardFromStackTop()
        {
            var gameCallbacks = new GameCallbacks();
            var deck = new Stack<CardManagement>();
            deck.Push(new CardManagement { CardName = "Card1" });
            deck.Push(new CardManagement { CardName = "Card2" });

            gameCallbacks.ReceiveGameDeck(deck);
            gameCallbacks.RemoveCardFromStack(true);

            Assert.Equal(1, gameCallbacks.GameDeck.Count);
            Assert.Equal("Card1", gameCallbacks.GameDeck.Peek().CardName);
        }

        [Fact]
        public void TestUpdateCurrentTurn()
        {
            var gameCallbacks = new GameCallbacks();
            string player = "Player1";

            gameCallbacks.UpdateCurrentTurn(player);

            Assert.Equal(player, gameCallbacks.CurrentTurnPlayer);
        }
    }
}
