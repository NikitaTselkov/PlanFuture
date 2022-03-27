using PlanFuture.Core;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PlanFuture.Services
{
    public interface IDragAndDropService
    {      
        public void SetItemToCollection(ICard card, ICardCollection cardCollection);
        public ICardCollection SetCollection(ICardCollection cardCollection);
        public void SwitchItems<T>(T item1, T item2) where T : IDraggedObject;
        public IEnumerable<ICardCollection> GetAllCollections();
        public ICardCollection GetCollectionById(int index);
        public ICard GetCardById(int index);
        public ICardCollection InitCollection();
        public ICard InitCard();
        public ICardCollection FindCollectionByCard(ICard card);
    }
}
