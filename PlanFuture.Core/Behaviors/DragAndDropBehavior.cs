using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlanFuture.Core.Behaviors
{
    public class DragAndDropBehavior
    {
        public readonly TranslateTransform Transform = new TranslateTransform();
        private Point _elementStartPosition2;
        private Point _mouseStartPosition2;
        private static DragAndDropBehavior _instance = new DragAndDropBehavior();
        public static DragAndDropBehavior Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

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
            }
            else
            {
                element.MouseLeftButtonDown -= Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp -= Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove -= Instance.ElementOnMouseMove;
            }
        }

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

            var previewBorder = new DependencyObject();
            var grid = VisualTreeHelper.GetParent((FrameworkElement)sender);
            var t = VisualTreeHelper.GetParent(grid);

            if (previewBorder is IDropPlace)
            {
                _elementStartPosition2.X = Transform.X;
                _elementStartPosition2.Y = Transform.Y;
            }
            else
            {
                Transform.X = _elementStartPosition2.X;
                Transform.Y = _elementStartPosition2.Y;
            }
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
    }
}
