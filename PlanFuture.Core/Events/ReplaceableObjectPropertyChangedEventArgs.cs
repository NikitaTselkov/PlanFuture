using PlanFuture.Core.DragAndDrop;
using System.Windows;

namespace PlanFuture.Core.Events
{
    public class ReplaceableObjectPropertyChangedEventArgs : RoutedEventArgs
    {
        public IViewDraggedObject SelectedObject { get; private set; }
        public IViewDraggedObject ReplaceableObject { get; private set; }

        public ReplaceableObjectPropertyChangedEventArgs() { }

        public ReplaceableObjectPropertyChangedEventArgs(IViewDraggedObject selectedObject, IViewDraggedObject replaceableObject)
        {
            SelectedObject = selectedObject;
            ReplaceableObject = replaceableObject;
        }

        public ReplaceableObjectPropertyChangedEventArgs(IViewDraggedObject selectedObject, IViewDraggedObject replaceableObject, RoutedEvent routedEvent) : base(routedEvent)
        {
            SelectedObject = selectedObject;
            ReplaceableObject = replaceableObject;
        }
    }
}
