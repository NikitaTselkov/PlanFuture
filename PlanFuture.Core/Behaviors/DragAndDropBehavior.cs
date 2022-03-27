using Microsoft.Xaml.Behaviors;
using PlanFuture.Core.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PlanFuture.Core.Behaviors
{
    public class DragAndDropBehavior : DependencyObject
    {
        public readonly TranslateTransform Transform = new TranslateTransform();
        private Point _elementStartPosition2;
        private Point _mouseStartPosition2;
        private static Lookup<Type, IViewDraggedObject> _draggedItems = new Lookup<Type, IViewDraggedObject>();
        private static DragAndDropBehavior _instance = new DragAndDropBehavior();     
        public static DragAndDropBehavior Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        #region Events

        public static readonly RoutedEvent ReplaceableObjectPropertyChangedEvent =
         EventManager.RegisterRoutedEvent("ReplaceableObjectPropertyChanged",
             RoutingStrategy.Bubble,
         typeof(RoutedPropertyChangedEventHandler<IViewDraggedObject>),
         typeof(DragAndDropBehavior));

        public static void AddReplaceableObjectPropertyChangedHandler(DependencyObject obj, RoutedPropertyChangedEventHandler<IViewDraggedObject> handler)
        {
            ((UIElement)obj).AddHandler(ReplaceableObjectPropertyChangedEvent, handler);
        }

        public static void RemoveReplaceableObjectPropertyChangedHandler(DependencyObject obj, RoutedPropertyChangedEventHandler<IViewDraggedObject> handler)
        {
            ((UIElement)obj).RemoveHandler(ReplaceableObjectPropertyChangedEvent, handler);
        }

        #endregion

        #region DependencyProperty

        public static IViewDraggedObject GetReplaceableObject(DependencyObject obj)
        {
            return (IViewDraggedObject)obj.GetValue(ReplaceableObjectProperty);
        }

        public static void SetReplaceableObject(DependencyObject obj, IViewDraggedObject value)
        {
            obj.SetValue(ReplaceableObjectProperty, value);

            ((UIElement)obj).RaiseEvent(new ReplaceableObjectPropertyChangedEventArgs((IViewDraggedObject)obj, value, ReplaceableObjectPropertyChangedEvent));
        }

        public static readonly DependencyProperty ReplaceableObjectProperty =
            DependencyProperty.Register("ReplaceableObject",
                typeof(IViewDraggedObject),
                typeof(DragAndDropBehavior),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static bool GetIsDrag(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragProperty);
        }

        public static void SetIsDrag(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragProperty, value);
        }

        public static readonly DependencyProperty IsDragProperty =
          DependencyProperty.RegisterAttached("IsDrag",
          typeof(bool), typeof(DragAndDropBehavior),
          new PropertyMetadata(false, OnDragChanged));

        private static void OnDragChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)sender;
            var isDrag = (bool)e.NewValue;

            Instance = new DragAndDropBehavior();
            ((UIElement)sender).RenderTransform = Instance.Transform;

            if (isDrag)
            {
                element.MouseLeftButtonDown += Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp += Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove += Instance.ElementOnMouseMove;

                _draggedItems.Add(element.GetType(), (IViewDraggedObject)sender);
            }
            else
            {
                element.MouseLeftButtonDown -= Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp -= Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove -= Instance.ElementOnMouseMove;

                _draggedItems.Remove(element.GetType(), (IViewDraggedObject)sender);
            }
        }

        #endregion

        private void ElementOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            mouseButtonEventArgs.Handled = true;

            UIElement parent = VisualTreeHelper.GetParent((FrameworkElement)sender) as UIElement;
            _mouseStartPosition2 = mouseButtonEventArgs.GetPosition(parent);
            ((UIElement)sender).CaptureMouse();
        }

        private void ElementOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ((UIElement)sender).ReleaseMouseCapture();

            if (GetIntersectingElement(sender) is IViewDraggedObject replaceableObject)
            {
                SetReplaceableObject((DependencyObject)sender, replaceableObject);
            }

            Transform.X = _elementStartPosition2.X;
            Transform.Y = _elementStartPosition2.Y;
        }

        private void ElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            UIElement parent = VisualTreeHelper.GetParent((FrameworkElement)sender) as UIElement;

            var mousePos = mouseEventArgs.GetPosition(parent);
            var diff = mousePos - _mouseStartPosition2;
            
            if (((UIElement)sender).IsMouseCaptured)
            {
                Transform.X = _elementStartPosition2.X + diff.X;
                Transform.Y = _elementStartPosition2.Y + diff.Y;
            }
        }

        private static IViewDraggedObject GetIntersectingElement(object sender)
        {
            Point currentMousePosition = ((UIElement)sender).PointToScreen(new Point(0, 0));
            Point draggedItemPosition;
            
            foreach (IViewDraggedObject draggedItem in _draggedItems[sender.GetType()])
            {
                draggedItemPosition = ((UIElement)draggedItem).PointToScreen(new Point(0, 0));

                if (sender != draggedItem)
                {
                    if (draggedItemPosition.X <= currentMousePosition.X &&
                        draggedItemPosition.Y <= currentMousePosition.Y &&
                        draggedItemPosition.X + (draggedItem as FrameworkElement).ActualWidth >= currentMousePosition.X &&
                        draggedItemPosition.Y + (draggedItem as FrameworkElement).ActualHeight >= currentMousePosition.Y)
                    {
                        return draggedItem;
                    }
                }
            }

            return null;
        }
    }
}
