using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlanFuture.Core.DragAndDrop
{
    public interface ICardCollection : IDraggedObject
    {
        public ObservableCollection<ICard> Cards { get; set; }

        public ICard this[ICard card]
        {
            get => Cards.FirstOrDefault(f => f == card);
            set => Cards[Cards.IndexOf(card)] = value;
        }
    }
}
