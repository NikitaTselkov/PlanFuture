using PlanFuture.Business;
using Prism.Commands;
using Prism.Mvvm;
using PlanFuture.Services;
using System.Collections.ObjectModel;
using PlanFuture.Core.DragAndDrop;
using PlanFuture.Core.Events;

namespace PlanFuture.Modules.Projects.ViewModels
{
    public class MainProjectViewModel : BindableBase
    {
        private readonly IDragAndDropService _dragAndDropService;

        private string _cardTitle;
        public string CardTitle
        {
            get { return _cardTitle; }
            set { SetProperty(ref _cardTitle, value); }
        }

        public ObservableCollection<ICardCollection> CardCollections => _dragAndDropService.GetAllCollections();

        #region Commands

        private DelegateCommand _addCardsCollection;
        public DelegateCommand AddCardsCollection =>
            _addCardsCollection ?? (_addCardsCollection = new DelegateCommand(ExecuteAddCardsCollection));

        private DelegateCommand<ReplaceableObjectPropertyChangedEventArgs> _switchCardsCollection;
        public DelegateCommand<ReplaceableObjectPropertyChangedEventArgs> SwitchCardsCollection =>
            _switchCardsCollection ?? (_switchCardsCollection = new DelegateCommand<ReplaceableObjectPropertyChangedEventArgs>(ExecuteSwitchCardsCollection));

        #endregion

        public MainProjectViewModel(IDragAndDropService dragAndDropService)
        {
            _dragAndDropService = dragAndDropService;
        }

        private void ExecuteAddCardsCollection()
        {
            // TODO: Сделать Validation
            if(!string.IsNullOrWhiteSpace(CardTitle))
                _dragAndDropService.SetCollection(new CardCollection(CardTitle));
        }

        private void ExecuteSwitchCardsCollection(object sender)
        {
            if (sender is ReplaceableObjectPropertyChangedEventArgs args)
            {
                if (args.SelectedObject.DataContext is CardsCollectionViewModel cardVM1 && args.ReplaceableObject.DataContext is CardsCollectionViewModel cardVM2)
                {
                    _dragAndDropService.SwitchItems(cardVM1.DraggedObject, cardVM2.DraggedObject);
                }
            }
        }
    }
}
