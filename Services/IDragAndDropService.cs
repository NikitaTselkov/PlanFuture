using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlanFuture.Core.DragAndDrop;

namespace PlanFuture.Services
{
    public interface IDragAndDropService
    {
        public void SetItemToCollection(ICard card, int cardCollectionIndex);
        public void MoveItemToAnotherCollection<T>(T item1, T item2) where T : IDraggedObject;
        public void SetCollection(ICardCollection cardCollection);
        public bool IsItemsInDifferentCollections<T>(T item1, T item2) where T : IDraggedObject;
        public void SwitchItems<T>(T item1, T item2) where T : IDraggedObject;
        public ObservableCollection<ICardCollection> GetAllCollections();
        public ICardCollection GetCollectionById(int index);
        public ICard GetCardById(int index);
        public ICardCollection InitCollection();
        public ICard InitCard();
        public ICardCollection FindCollectionByCard(ICard card);
    }
}
