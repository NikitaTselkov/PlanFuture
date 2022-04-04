using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PlanFuture.Core.DragAndDrop;

namespace PlanFuture.Services
{
    public class DragAndDropService : IDragAndDropService
    {
        private static ObservableCollection<ICardCollection> _collections = new();
        private static int _lastCardCollectionIndex = 0;
        private static int _lastCardIndex = 0;

        public ObservableCollection<ICardCollection> GetAllCollections()
        {
            return _collections;
        }

        public void SetCollection(ICardCollection cardCollection)
        {
            if (cardCollection is null)
            {
                throw new ArgumentNullException(nameof(cardCollection));
            }

            cardCollection.Index = _lastCardCollectionIndex + 1;
            cardCollection.IndexInCollection = _collections.Count;
            _collections.Add(cardCollection);
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

            return _collections?.SingleOrDefault(s => s.Index == index);
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

            if (item1 is ICard card1 && item2 is ICard card2)
            {
                ICardCollection collection1 = FindCollectionByCard(card1);
                ICardCollection collection2 = FindCollectionByCard(card2);

                var card1IndexInCollection = card1.IndexInCollection;
                var card2IndexInCollection = card2.IndexInCollection;

                ICard card = card1;

                //TODO: Починить.

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
                #region Черная магия

                if (collection1.IndexInCollection > collection2.IndexInCollection)
                {
                    collection2.Index = _lastCardCollectionIndex + 2;
                    collection1.Index = _lastCardCollectionIndex + 1;

                    SwapCardCollections(ref collection1, ref collection2, 1);
                    UpdateCardsIndex(ref collection2, ref collection1);
                }
                else
                {
                    collection2.Index = _lastCardCollectionIndex + 1;
                    collection1.Index = _lastCardCollectionIndex + 2;

                    SwapCardCollections(ref collection1, ref collection2, -1);
                    UpdateCardsIndex(ref collection1, ref collection2);
                }

                #endregion
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

        private static void SwapCardCollections(ref ICardCollection collection1, ref ICardCollection collection2, int IndexOffset)
        {
            var collection1IndexInCollection = collection1.IndexInCollection;
            var collection2IndexInCollection = collection2.IndexInCollection;

            collection1.IndexInCollection = collection2IndexInCollection;
            _collections.RemoveAt(collection1IndexInCollection);
            _collections.Insert(collection2IndexInCollection, collection1);

            collection2.IndexInCollection = collection1IndexInCollection;
            _collections.RemoveAt(collection2IndexInCollection + IndexOffset);
            _collections.Insert(collection1IndexInCollection, collection2);
        }

        private static void UpdateCardsIndex(ref ICardCollection collection1, ref ICardCollection collection2)
        {
            var lastCardIndex = 0;

            for (int i = 0; i < collection2.Cards.Count; i++)
            {
                collection2.Cards[i].Index = _lastCardIndex + i + 1;
                lastCardIndex = _lastCardIndex + i + 1;
            }

            for (int i = 0; i < collection1.Cards.Count; i++)
            {
                collection1.Cards[i].Index = lastCardIndex + i + 1;
            }
        }
    }
}
