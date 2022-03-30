using PlanFuture.Core;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using PlanFuture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using PlanFuture.Core.Events;

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

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public CardViewModel(IDragAndDropService dragAndDropService)
        {
            _dragAndDropService = dragAndDropService;

            DraggedObject = _dragAndDropService.InitCard();
            Title = new Random().Next(0, 3904).ToString();
        }
    }
}
