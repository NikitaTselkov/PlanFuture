using System.Collections.ObjectModel;
using PlanFuture.Core.DragAndDrop;

namespace PlanFuture.Business
{
    public class CardCollection : ICardCollection
    {
        public string Title { get; set; }
        public int Index { get; set; }
        public int IndexInCollection { get; set; }
        public ObservableCollection<ICard> Cards { get; set; }      

        public CardCollection(string title)
        {
            Title = title;
            Cards = new ObservableCollection<ICard>();
        }
    }
}
