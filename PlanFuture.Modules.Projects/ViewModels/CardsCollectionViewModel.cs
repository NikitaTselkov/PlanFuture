using PlanFuture.Business;
using Prism.Commands;
using Prism.Mvvm;
using PlanFuture.Services;
using System.Collections.ObjectModel;
using PlanFuture.Core.Events;
using PlanFuture.Core.DragAndDrop;


namespace PlanFuture.Modules.Projects.ViewModels
{
    public class CardsCollectionViewModel : BindableBase, IViewModelDraggedObject
    {
        private readonly IDragAndDropService _dragAndDropService;

        private IDraggedObject _draggedObject;
        public IDraggedObject DraggedObject
        {
            get { return _draggedObject; }
            set{ SetProperty(ref _draggedObject, value); }
        }

        public string Title => DraggedObject.Title;

        public ObservableCollection<ICard> Cards => _dragAndDropService.GetCollectionById(DraggedObject.Index).Cards;

        #region Commands

        private DelegateCommand _addCard;
        public DelegateCommand AddCard =>
            _addCard ?? (_addCard = new DelegateCommand(ExecuteAddCard));

        private DelegateCommand<ReplaceableObjectPropertyChangedEventArgs> _switchCards;
        public DelegateCommand<ReplaceableObjectPropertyChangedEventArgs> SwitchCards =>
            _switchCards ?? (_switchCards = new DelegateCommand<ReplaceableObjectPropertyChangedEventArgs>(ExecuteSwitchCards));

        #endregion

        public CardsCollectionViewModel(IDragAndDropService dragAndDropService)
        {
            _dragAndDropService = dragAndDropService;

            DraggedObject = _dragAndDropService.InitCollection();
        }

        private void ExecuteAddCard()
        {
            //TODO: Изменить.
            _dragAndDropService.SetItemToCollection(new Card("Test1"), DraggedObject.Index);
        }

        private void ExecuteSwitchCards(object sender)
        {
            if (sender is ReplaceableObjectPropertyChangedEventArgs args)
            {
                if (args.SelectedObject.DataContext is CardViewModel cardVM1 && args.ReplaceableObject.DataContext is CardViewModel cardVM2)
                {
                    if (_dragAndDropService.IsItemsInDifferentCollections(cardVM1.DraggedObject, cardVM2.DraggedObject))
                        _dragAndDropService.MoveItemToAnotherCollection(cardVM1.DraggedObject, cardVM2.DraggedObject);
                    else 
                        _dragAndDropService.SwitchItems(cardVM1.DraggedObject, cardVM2.DraggedObject);
                }
            }
        }
    }
}
