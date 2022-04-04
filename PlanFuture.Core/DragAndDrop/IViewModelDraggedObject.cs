using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Core.DragAndDrop
{
    public interface IViewModelDraggedObject
    {
        public IDraggedObject DraggedObject { get; set; }
    }
}
