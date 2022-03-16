using PlanFuture.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Business
{
    public class CardCollection : ICardCollection
    {
        public string Title { get; }
        public int Index { get; set; }
        public ObservableCollection<ICard> Cards { get; set; }

        public CardCollection(string title)
        {
            Title = title;
            Cards = new ObservableCollection<ICard>();
        }
    }
}
