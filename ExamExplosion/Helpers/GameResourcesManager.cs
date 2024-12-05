using ExamExplosion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamExplosion.Helpers
{
    public class GameResourcesManager
    {
        public Stack<Card> GameDeck {  get; set; }
        public List<Card> PlayerCards { get; set; }
        public int CurrentIndex {  get; set; }
        public int Hp {  get; set; }
        public GameResourcesManager() { 
        
        }

        public void DrawBottomCard()
        {
            Card bottomCard = GameDeck.ToArray().Last();
            PlayerCards.Add(bottomCard);
            RemoveBottomCard();
        }
        public void DrawTopCard()
        {
            Card card = this.GameDeck.Pop();
            PlayerCards.Add(card);
        }
        public void RemoveBottomCard()
        {
            Stack<Card> temporaryStack = new Stack<Card>();
            while (GameDeck.Count > 1)
            {
                temporaryStack.Push(GameDeck.Pop());
            }
            if (GameDeck.Count == 1)
            {
                GameDeck.Pop();
            }
            while (temporaryStack.Count > 0)
            {
                GameDeck.Push(temporaryStack.Pop());
            }
        }
        public void RemoveTopCard()
        {
            this.GameDeck.Pop();
        }
        public void ShuffleGameDeck()
        {
            Card[] cardsToShuffle = {null, null, null};
            cardsToShuffle[0] = GameDeck.Pop();
            cardsToShuffle[1] = GameDeck.Pop();
            cardsToShuffle[2] = GameDeck.Pop();

            GameDeck.Push(cardsToShuffle[0]);
            GameDeck.Push(cardsToShuffle[1]);
            GameDeck.Push(cardsToShuffle[2]);
        }
        public void AddReRegistrationCard(int index)
        {
            Card newCard = new Card();
            newCard.Path = "reRegistration";
            newCard.Name = "Re registration";

            List<Card> gameDeckList = new List<Card>(GameDeck);
            gameDeckList.Insert(index, newCard);
            GameDeck = new Stack<Card>(gameDeckList);
        }
        public bool HasReRegistration()
        {
            bool hasReRegistration = false;
            foreach (Card card in PlayerCards)
            {
                if(card != null && card.Path == "reRegistration")
                {
                    hasReRegistration = true;
                }
            }
            return hasReRegistration;
        }
        public void DropCardByIndex(int index)
        {
            PlayerCards.RemoveAt(index);
        }

        public void AddCard(Card card)
        {
            this.PlayerCards.Add(card);
        }
        public void ReduceHp()
        {
            this.Hp --;
        }

        public List<string> GetSelectedCardsPaths()
        {
            List<string> paths = new List<string>();
            foreach (Card card in PlayerCards)
            {
                if (card.IsSelected)
                {
                    paths.Add(card.Path);
                }
            }
            return paths;
        }
        
        public List<Card> SeeTheFuture()
        {
            List<Card> topThreeCards = new List<Card>();
            List<Card> deckSnapshot = GameDeck.ToList();

            for (int i = 0; i < Math.Min(3, deckSnapshot.Count); i++)
            {
                topThreeCards.Add(deckSnapshot[i]);
            }

            return topThreeCards;
        }
    }
}
