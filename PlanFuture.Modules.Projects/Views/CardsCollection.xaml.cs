using PlanFuture.Core;
using PlanFuture.Modules.Projects.UserControls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlanFuture.Modules.Projects.Views
{
    /// <summary>
    /// Interaction logic for CardsCollection
    /// </summary>
    public partial class CardsCollection : UserControl, IViewDraggedObject
    {
        private FrameworkElement _selectedCard { get; set; }
        private FrameworkElement _replaceableCard { get; set; }
        private FrameworkElement _preview { get; set; }
        private StackPanel _stackPanel { get; set; }
        private int _selectedCardIndex { get; set; }
        private int _replaceableCardIndex { get; set; }
        private Thickness _margin { get; set; }

        public CardsCollection()
        {
            InitializeComponent();

            _selectedCard = null;
            _replaceableCard = null;
            _preview = null;
            _selectedCardIndex = 0;
            _replaceableCardIndex = 0;
            _margin = default;
        }

        //private void ListView_MouseMove(object sender, MouseEventArgs e)
        //{
        //    //if (e.LeftButton == MouseButtonState.Pressed)
        //    //{
        //    //    var relativePosition = e.GetPosition(this);
        //    //    var mousePoint = PointToScreen(relativePosition);

        //    //    FrameworkElement container = null;
        //    //    DependencyObject card = null;
        //    //    DependencyObject currentChild = null;
        //    //    Point startPoint;

        //    //    var obj = (DependencyObject)sender;
        //    //    obj = VisualTreeHelper.GetChild(obj, 0);
        //    //    obj = VisualTreeHelper.GetChild(obj, 0);

        //    //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    //    {
        //    //        currentChild = VisualTreeHelper.GetChild(obj, i);
        //    //        card = FindDependencyObjectByType<Card>(currentChild);

        //    //        if (card != null)
        //    //        {
        //    //            container = (FrameworkElement)card;
        //    //            startPoint = container.PointToScreen(new Point(0, 0));

        //    //            if (startPoint.X <= mousePoint.X &&
        //    //                startPoint.X + container.ActualWidth >= mousePoint.X &&
        //    //                startPoint.Y + container.ActualHeight >= mousePoint.Y &&
        //    //                startPoint.Y <= mousePoint.Y)
        //    //            {
        //    //                if (_selectedCard == null)
        //    //                {
        //    //                    _selectedCard = container;

        //    //                    if (sender is ListView listView)
        //    //                        _selectedCardIndex = listView.Items.IndexOf(((ListViewItem)currentChild).Content);
        //    //                }
        //    //                else if (_selectedCard != container)
        //    //                {
        //    //                    if (_replaceableCard != null && _replaceableCard != container)
        //    //                    {
        //    //                        DeleteBorder();
        //    //                    }

        //    //                    _replaceableCard = container;

        //    //                    _preview = new BorderPreview();
        //    //                    _preview.Width = container.ActualWidth;
        //    //                    _preview.Height = container.ActualHeight;

        //    //                    _stackPanel = VisualTreeHelper.GetParent(_replaceableCard) as StackPanel;

        //    //                    if (_stackPanel != null && _stackPanel.Children.Count <= 1)
        //    //                    {
        //    //                        if (sender is ListView listView)
        //    //                            _replaceableCardIndex = listView.Items.IndexOf(((ListViewItem)currentChild).Content);

        //    //                        _margin = new Thickness(0, -_selectedCard.ActualHeight, 0, 0);
        //    //                        //_selectedCard.Margin = _margin;

        //    //                        if (_selectedCardIndex > _replaceableCardIndex)
        //    //                        {
        //    //                            _stackPanel.Children.Insert(0, _preview);

        //    //                            //TODO: Сдклать список Views
        //    //                            //TODO: Взять item сверху и поменять ему margin.
        //    //                        }
        //    //                        else
        //    //                        {
        //    //                            _stackPanel.Children.Insert(1, _preview);
        //    //                        }
        //    //                    }
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //}

        //private void ListView_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    //DeleteBorder();
        //    //_selectedCard = null;
        //}

        private void DeleteBorder()
        {
            if (_preview != null)
                _stackPanel.Children.Remove(_preview);

            if (_selectedCard != null)
                _selectedCard.Margin = new Thickness(0, 5, 0, 5);

            _margin = default;
            _preview = null;
            _replaceableCard = null;
        }

        private DependencyObject FindDependencyObjectByType<T>(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is not null and T)
                {
                    return child;
                }
                else
                {
                    var result = FindDependencyObjectByType<T>(child);

                    if (result != null)
                        return result;
                }
            }

            return null;
        }
    }
}
