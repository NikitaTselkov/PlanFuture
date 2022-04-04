using Prism.Mvvm;
using PlanFuture.Services;
using System;
using PlanFuture.Core.DragAndDrop;

namespace PlanFuture.Modules.Projects.ViewModels
{
    public class CardViewModel : BindableBase, IViewModelDraggedObject
    {
        private readonly IDragAndDropService _dragAndDropService;

        private IDraggedObject _draggedObject;
        public IDraggedObject DraggedObject
        {
            get { return _draggedObject; }
            set { SetProperty(ref _draggedObject, value); }
        }

        public string Title => DraggedObject.Title;

        public CardViewModel(IDragAndDropService dragAndDropService)
        {
            _dragAndDropService = dragAndDropService;

            DraggedObject = _dragAndDropService.InitCard();
        }
    }
}
