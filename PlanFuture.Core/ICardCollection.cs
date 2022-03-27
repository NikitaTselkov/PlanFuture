using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Core
{
    public interface ICardCollection : IDraggedObject
    {
        public ObservableCollection<ICard> Cards { get; set; }

        public ICard this[ICard card]
        {
            get => Cards.FirstOrDefault(f => f == card);
        }
    }
}
