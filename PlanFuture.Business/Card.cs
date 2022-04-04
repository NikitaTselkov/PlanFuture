using PlanFuture.Core;
using System;
using PlanFuture.Core.DragAndDrop;

namespace PlanFuture.Business
{
    public class Card : ICard
    {
        public string Title { get; set; }
        public int Index { get; set; }
        public int IndexInCollection { get; set; }

        public Card(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"\"{nameof(title)}\" не может быть неопределенным или пустым.", nameof(title));
            }

            Title = title;
        }
    }
}
