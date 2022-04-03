using PlanFuture.Core.Events;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlanFuture.Core.Behaviors
{

    public class DragAndDropBehavior : DependencyObject
    {
        public readonly TranslateTransform Transform = new TranslateTransform();
        private Point _elementStartPosition;
        private Point _mouseStartPosition;
        private Thickness? _startSenderMargin;
        private static Point _transformPosition;
        private static Positions _replaceableObjectPositionRelativeToCurrentElement;
        private static DependencyObject _oldReplaceableObject;
        private static Lookup<Type, IViewDraggedObject> _draggedItems = new Lookup<Type, IViewDraggedObject>();
        private static DragAndDropBehavior _instance = new DragAndDropBehavior();     
        public static DragAndDropBehavior Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
        private event EventHandler<ReplaceableObjectPositionChangedEventArgs> _replaceableObjectPositionChanged;

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
                new FrameworkPropertyMetadata(null));

        public static DependencyObject GetPreviewDependencyObject(DependencyObject obj)
        {
            return (DependencyObject)obj.GetValue(PreviewDependencyObjectProperty);
        }

        public static void SetPreviewDependencyObject(DependencyObject obj, DependencyObject value)
        {
            obj.SetValue(PreviewDependencyObjectProperty, value);
        }

        public static readonly DependencyProperty PreviewDependencyObjectProperty =
            DependencyProperty.RegisterAttached("PreviewDependencyObject",
                typeof(DependencyObject),
                typeof(DragAndDropBehavior),
                new PropertyMetadata(null));

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
            var element = (FrameworkElement)sender;
            var isDrag = (bool)e.NewValue;

            Instance = new DragAndDropBehavior();
            ((UIElement)sender).RenderTransform = Instance.Transform;

            if (isDrag)
            {
                element.MouseLeftButtonDown += Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp += Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove += Instance.ElementOnMouseMove;
                element.Loaded += Instance.ElementLoaded;
                element.Unloaded += Instance.ElementUnloaded;
            }
            else
            {
                element.MouseLeftButtonDown -= Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp -= Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove -= Instance.ElementOnMouseMove;
                element.Loaded += Instance.ElementLoaded;
                element.Unloaded -= Instance.ElementUnloaded;
            }
        }

        #endregion

        private void ElementOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            mouseButtonEventArgs.Handled = true;

            UIElement parent = VisualTreeHelper.GetParent((FrameworkElement)sender) as UIElement;
            _mouseStartPosition = mouseButtonEventArgs.GetPosition(parent);
            _startSenderMargin = ((FrameworkElement)sender).Margin;
            ((UIElement)sender).CaptureMouse();
        }

        private void ElementOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ((UIElement)sender).ReleaseMouseCapture();

            if (GetIntersectingElement(sender, mouseButtonEventArgs, _transformPosition) is IViewDraggedObject replaceableObject)
                SetReplaceableObject((DependencyObject)sender, replaceableObject);

            if (_startSenderMargin is not null)
                ((FrameworkElement)sender).Margin = _startSenderMargin.Value;

            RemovePreviewDependencyObject(_oldReplaceableObject);

            _oldReplaceableObject = null;
            _startSenderMargin = null;
            _transformPosition = default;

            Transform.X = 0;
            Transform.Y = 0;
        }

        private void ElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            UIElement parent = VisualTreeHelper.GetParent((FrameworkElement)sender) as UIElement;
            var mousePos = mouseEventArgs.GetPosition(parent);
            var diff = mousePos - _mouseStartPosition;
            
            if (((UIElement)sender).IsMouseCaptured)
            {
                Positions newPosition;
                Positions oldPosition;

                Transform.X = _elementStartPosition.X + diff.X;
                Transform.Y = _elementStartPosition.Y + diff.Y;

                if (GetIntersectingElement(sender, mouseEventArgs) is IViewDraggedObject replaceableObject)
                {
                    newPosition = GetRelativePosition(sender as IViewDraggedObject, replaceableObject);
                    oldPosition = _replaceableObjectPositionRelativeToCurrentElement;

                    Instance._replaceableObjectPositionChanged?.Invoke(null,
                            new ReplaceableObjectPositionChangedEventArgs(oldPosition, newPosition, sender, replaceableObject));
                }
            }
        }

        private void ElementLoaded(object sender, RoutedEventArgs e)
        {
            Instance._replaceableObjectPositionChanged += OnReplaceableObjectPositionChanged;
            _draggedItems.Add(sender.GetType(), (IViewDraggedObject)sender);
        }

        private void ElementUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;

            element.MouseLeftButtonDown -= Instance.ElementOnMouseLeftButtonDown;
            element.MouseLeftButtonUp -= Instance.ElementOnMouseLeftButtonUp;
            element.MouseMove -= Instance.ElementOnMouseMove;
            element.Loaded -= Instance.ElementLoaded;
            element.Unloaded -= Instance.ElementUnloaded;
            Instance._replaceableObjectPositionChanged -= OnReplaceableObjectPositionChanged;

            _draggedItems.Remove(element.GetType(), (IViewDraggedObject)sender);
        }

        private void OnReplaceableObjectPositionChanged(object sender, ReplaceableObjectPositionChangedEventArgs e)
        {
            if (e.SelectedObject is null)
            {
                throw new ArgumentNullException(nameof(e.SelectedObject));
            }
            if (e.ReplaceableObject is null)
            {
                throw new ArgumentNullException(nameof(e.ReplaceableObject));
            }

            var replaceableElement = e.ReplaceableObject as DependencyObject;
            
            // Old 
            RemovePreviewDependencyObject(_oldReplaceableObject);

            // New
            AddPreviewDependencyObject(replaceableElement, e);

            _replaceableObjectPositionRelativeToCurrentElement = e.NewPosition;
            _oldReplaceableObject = replaceableElement;
        }

        private void RemovePreviewDependencyObject(DependencyObject replaceableObject)
        {
            if (replaceableObject is not null)
            {
                var previewDependencyObject = GetPreviewDependencyObject(replaceableObject);
                var replaceableObjectParent = VisualTreeHelper.GetParent(replaceableObject) as UIElement;

                if (replaceableObjectParent is StackPanel stackPanel)
                {
                    if (((FrameworkElement)previewDependencyObject)?.Parent is not null)
                        stackPanel.Children.Remove((UIElement)previewDependencyObject);
                }
            }
        }

        private void AddPreviewDependencyObject(DependencyObject replaceableObject, ReplaceableObjectPositionChangedEventArgs e)
        {
            if (replaceableObject is not null)
            {
                var actualHeight = ((FrameworkElement)replaceableObject).ActualHeight;
                var previewDependencyObject = GetPreviewDependencyObject(replaceableObject);
                var replaceableObjectParent = VisualTreeHelper.GetParent(replaceableObject) as UIElement;

                if (replaceableObjectParent is StackPanel stackPanel)
                {
                    if (((FrameworkElement)previewDependencyObject)?.Parent is null)
                    {
                        if (e.NewPosition == Positions.Below || e.NewPosition == Positions.Same)
                        {
                            _transformPosition.Y = actualHeight;
                            ((FrameworkElement)e.SelectedObject).Margin = new Thickness(0, -actualHeight, 0, 0);
                            stackPanel.Children.Insert(0, (UIElement)previewDependencyObject);
                        }
                        else if (e.NewPosition == Positions.Above)
                        {
                            _transformPosition.Y = -actualHeight;
                            ((FrameworkElement)e.SelectedObject).Margin = new Thickness(0, -actualHeight, 0, 0);
                            stackPanel.Children.Insert(1, (UIElement)previewDependencyObject);
                        }
                    }
                }
            }
        }

        private IViewDraggedObject GetIntersectingElement(object sender, MouseEventArgs mouseEventArgs, Point transformPosition = new Point())
        {
            var relativeMousePosition = mouseEventArgs.GetPosition((UIElement)sender);
            Point currentMousePosition = ((UIElement)sender).PointToScreen(relativeMousePosition);
            Point draggedItemPosition;

            currentMousePosition.X += transformPosition.X;
            currentMousePosition.Y += transformPosition.Y;

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
                        _elementStartPosition.Y = ((FrameworkElement)sender).ActualHeight;

                        return draggedItem;
                    }
                }
            }

            return null;
        }

        private static Positions GetRelativePosition(IViewDraggedObject selectedObject, IViewDraggedObject replaceableObject)
        {
            var replaceableItemIndexInCollection = (replaceableObject.DataContext as IViewModelDraggedObject)?.DraggedObject?.IndexInCollection;
            var selectedItemIndexInCollection = (selectedObject.DataContext as IViewModelDraggedObject)?.DraggedObject?.IndexInCollection;

            if (selectedItemIndexInCollection > replaceableItemIndexInCollection)
                return Positions.Below;
            else if (selectedItemIndexInCollection < replaceableItemIndexInCollection)
                return Positions.Above;
            else
                return Positions.Same;
        }
    }
}
