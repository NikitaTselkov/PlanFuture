using PlanFuture.Core;
using PlanFuture.Business;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlanFuture.Modules.Projects.ViewModels
{
    public class CardsCollectionViewModel : BindableBase
    {
        private readonly IDragAndDropService _dragAndDropService;

        private ICardCollection _cardCollection;
        public ICardCollection CardCollection
        {
            get { return _cardCollection; }
            set { SetProperty(ref _cardCollection, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<ICard> _cards;
        public ObservableCollection<ICard> Cards
        {
            get { return _cards; }
            set { SetProperty(ref _cards, value); }
        }

        private DelegateCommand _addCard;
        public DelegateCommand AddCard =>
            _addCard ?? (_addCard = new DelegateCommand(ExecuteAddCard));

        public CardsCollectionViewModel(IDragAndDropService dragAndDropService)
        {
            _dragAndDropService = dragAndDropService;

            CardCollection = _dragAndDropService.InitCollection();

            Title = CardCollection.Title;
            Cards = CardCollection.Cards;
        }

        private void ExecuteAddCard()
        {
            _dragAndDropService.SetItemToCollection(new Card("Test1"), CardCollection);
        }
    }
}
