using System;
using PlanFuture.Core.DragAndDrop;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlanFuture.Modules.Projects.Views
{
    /// <summary>
    /// Interaction logic for Card
    /// </summary>
    public partial class Card : UserControl, IViewDraggedObject
    {
        public Card()
        {
            InitializeComponent();

            Random r = new Random();
            Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),
                              (byte)r.Next(1, 255), (byte)r.Next(1, 233)));

            Background = brush;
        }
    }
}
