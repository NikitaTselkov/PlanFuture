using PlanFuture.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Services
{
    public class DragAndDropService : IDragAndDropService
    {
        private static List<ICardCollection> _collections = new();
        private static int _lastCardCollectionIndex = -1;
        private static int _lastCardIndex = -1;

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

        public void SetItemToCollection(ICard card, ICardCollection cardCollection)
        {
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
            cardCollection.Cards.Add(card);

            if (_collections.Any(a => a == cardCollection))
                _collections.Remove(cardCollection);
            
            _collections.Add(cardCollection);
        }

        public ICardCollection FindCollectionByCard(ICard card)
        {
            if (card is null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return _collections?.SingleOrDefault(s => s.Cards.Any(a => a == card));
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

                index = collection1.Cards.FirstOrDefault(f => f == card1).Index;

                _collections.FirstOrDefault(f => f == collection1)[card1].Index = card2.Index;
                _collections.FirstOrDefault(f => f == collection1)[card2].Index = index;

                _collections.ForEach(f => f.Cards.Sort((s1, s2) => s1.Index.CompareTo(s2.Index)));
            }
            else if (item1 is ICardCollection collection1 && item2 is ICardCollection collection2)
            {
                index = _collections.FirstOrDefault(f => f == collection1).Index;
                _collections.FirstOrDefault(f => f == collection1).Index = collection2.Index;
                _collections.FirstOrDefault(f => f == collection2).Index = index;

                //TODO: Сортировать.
                // _collections.OrderBy(o => o.Index);
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
