using PlanFuture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Business
{
    public class Card : ICard
    {
        public string Title { get; private set; }

        public int Index { get; set; }

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
