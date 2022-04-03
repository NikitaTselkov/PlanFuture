using PlanFuture.Core;
using PlanFuture.Core.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PlanFuture.Services
{
    public interface IDragAndDropService
    {
        public void SetItemToCollection(ICard card, int cardCollectionIndex);
        public void MoveItemToAnotherCollection<T>(T item1, T item2) where T : IDraggedObject;
        public ICardCollection SetCollection(ICardCollection cardCollection);
        public bool IsItemsInDifferentCollections<T>(T item1, T item2) where T : IDraggedObject;
        public void SwitchItems<T>(T item1, T item2) where T : IDraggedObject;
        public IEnumerable<ICardCollection> GetAllCollections();
        public ICardCollection GetCollectionById(int index);
        public ICard GetCardById(int index);
        public ICardCollection InitCollection();
        public ICard InitCard();
        public ICardCollection FindCollectionByCard(ICard card);
    }
}
