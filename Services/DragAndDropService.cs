using PlanFuture.Core;
using PlanFuture.Core.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Services
{
    public class DragAndDropService : IDragAndDropService
    {
        private static List<ICardCollection> _collections = new();
        private static int _lastCardCollectionIndex = 0;
        private static int _lastCardIndex = 0;

        public IEnumerable<ICardCollection> GetAllCollections()
        {
            return _collections;
        }

        public ICardCollection SetCollection(ICardCollection cardCollection)
        {
            if (cardCollection is null)
            {
                throw new ArgumentNullException(nameof(cardCollection));
            }

            cardCollection.Index = _lastCardCollectionIndex + 1;
            _collections.Add(cardCollection);

            return cardCollection;
        }

        public void SetItemToCollection(ICard card, int cardCollectionIndex)
        {
            var cardCollection = GetCollectionById(cardCollectionIndex);

            if (card is null)
            {
                throw new ArgumentNullException(nameof(card));
            }
            if (cardCollection is null)
            {
                throw new ArgumentNullException(nameof(cardCollection));
            }

            if(cardCollection.Cards is null)
                cardCollection.Cards = new ObservableCollection<ICard>();

            card.Index = _lastCardIndex + 1;
            card.IndexInCollection = cardCollection.Cards.Count;
            cardCollection.Cards.Add(card);
        }

        public void MoveItemToAnotherCollection<T>(T item1, T item2) where T : IDraggedObject
        {
            if (item1 is null)
            {
                throw new ArgumentNullException(nameof(item1));
            }
            if (item2 is null)
            {
                throw new ArgumentNullException(nameof(item2));
            }

            if (item1 is ICard card1 && item2 is ICard card2)
            {
                ICardCollection collection1 = FindCollectionByCard(card1);
                ICardCollection collection2 = FindCollectionByCard(card2);

                var card2IndexInCollection = card2.IndexInCollection;

                card1.IndexInCollection = card2IndexInCollection;
                card1.Index = _lastCardIndex + 1;
                collection1.Cards.Remove(card1);
                collection2.Cards.Insert(card2IndexInCollection, card1);

                for (int i = card2IndexInCollection + 1; i < collection2.Cards.Count; i++)
                {
                    collection2.Cards[i].IndexInCollection += 1;
                }

                for (int i = 0; i < collection1.Cards.Count; i++)
                {
                    collection1.Cards[i].IndexInCollection = i;
                }
            }
        }

        public bool IsItemsInDifferentCollections<T>(T item1, T item2) where T : IDraggedObject
        {
            if (item1 is ICard card1 && item2 is ICard card2)
            {
                var collection1 = FindCollectionByCard(card1);
                var collection2 = FindCollectionByCard(card2);

                return collection1.Index != collection2.Index;
            }

            throw new ArgumentNullException($"{nameof(item1)}, {nameof(item2)}");
        }

        public ICardCollection FindCollectionByCard(ICard card)
        {
            if (card is null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return _collections?.SingleOrDefault(s => s.Cards.Any(a => a.Index == card.Index));
        }

        public ICardCollection GetCollectionById(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            return _collections.SingleOrDefault(s => s.Index == index);
        }

        public ICard GetCardById(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            ICard card;

            foreach (var item in _collections)
            {
                card = item.Cards.SingleOrDefault(f => f.Index == index);

                if (card != null)
                    return card;
            }

            return null;
        }

        public void SwitchItems<T>(T item1, T item2) where T : IDraggedObject
        {
            if (item1 is null)
            {
                throw new ArgumentNullException(nameof(item1));
            }
            if (item2 is null)
            {
                throw new ArgumentNullException(nameof(item2));
            }

            int index = 0;

            if (item1 is ICard card1 && item2 is ICard card2)
            {
                ICardCollection collection1 = FindCollectionByCard(card1);
                ICardCollection collection2 = FindCollectionByCard(card2);

                var card1IndexInCollection = card1.IndexInCollection;
                var card2IndexInCollection = card2.IndexInCollection;

                ICard card = card1;

                #region Черная магия

                if (collection1 != collection2)
                {
                    card2.Index = _lastCardIndex + 2;
                    card.Index = _lastCardIndex + 1;
                }
                else
                {
                    if (card1.IndexInCollection > card2.IndexInCollection)
                    {
                        card2.Index = _lastCardIndex + 1;
                        card.Index = _lastCardIndex + 2;
                    }
                    else
                    {
                        card2.Index = _lastCardIndex + 2;
                        card.Index = _lastCardIndex + 1;
                    }
                }

                #endregion

                card2.IndexInCollection = card2IndexInCollection;
                collection2.Cards.RemoveAt(card2IndexInCollection);
                collection2.Cards.Insert(card2IndexInCollection, card2);

                card.IndexInCollection = card1IndexInCollection;
                collection1.Cards.RemoveAt(card1IndexInCollection);
                collection1.Cards.Insert(card1IndexInCollection, card);
            }
            else if (item1 is ICardCollection collection1 && item2 is ICardCollection collection2)
            {
                index = _collections.FirstOrDefault(f => f == collection1).Index;
                _collections.FirstOrDefault(f => f == collection1).Index = collection2.Index;
                _collections.FirstOrDefault(f => f == collection2).Index = index;
                
                //TODO: Добавить черной магии.
            }
        }

        public ICard InitCard()
        {
            _lastCardIndex++;
            return GetCardById(_lastCardIndex);
        }

        public ICardCollection InitCollection()
        {
            _lastCardCollectionIndex++;
            return GetCollectionById(_lastCardCollectionIndex);
        }
    }
}
