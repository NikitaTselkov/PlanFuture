using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Core.Events
{
    public class ReplaceableObjectPositionChangedEventArgs : EventArgs
    {
        public Positions OldPosition { get; private set; }
        public Positions NewPosition { get; private set; }
        public object SelectedObject { get; private set; }
        public object ReplaceableObject { get; private set; }

        public ReplaceableObjectPositionChangedEventArgs() { }

        public ReplaceableObjectPositionChangedEventArgs(Positions oldPosition, Positions newPosition, object selectedObject, object replaceableObject)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
            SelectedObject = selectedObject;
            ReplaceableObject = replaceableObject;
        }
    }
}
