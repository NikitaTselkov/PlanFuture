using PlanFuture.Core;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using PlanFuture.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanFuture.Modules.Projects.ViewModels
{
    public class CardViewModel : BindableBase
    {
        private readonly IDragAndDropService _dragAndDropService;

        private ICard _card;
        public ICard Card
        {
            get { return _card; }
            set { SetProperty(ref _card, value); }
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

            Card = _dragAndDropService.InitCard();
            
            Title = Card.Title;
        }
    }
}
