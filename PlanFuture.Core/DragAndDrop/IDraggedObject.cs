using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Core.DragAndDrop
{
    public interface IDraggedObject
    {
        public string Title { get; set; }
        public int Index { get; set; }
        public int IndexInCollection { get; set; }
    }
}
