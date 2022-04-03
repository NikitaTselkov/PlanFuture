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
        public CardsCollection()
        {
            InitializeComponent();
        }
    }
}
