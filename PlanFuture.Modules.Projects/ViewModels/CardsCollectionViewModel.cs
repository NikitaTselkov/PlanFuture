using PlanFuture.Core;
using PlanFuture.Business;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using PlanFuture.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PlanFuture.Core.Events;
using System.ComponentModel;

namespace PlanFuture.Modules.Projects.ViewModels
{
    public class CardsCollectionViewModel : BindableBase
    {
        private readonly IDragAndDropService _dragAndDropService;

        private ICardCollection _cardCollection;
        public ICardCollection CardCollection
        {
            get { return _cardCollection; }
            set{ SetProperty(ref _cardCollection, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<ICard> Cards => _dragAndDropService.GetCollectionById(CardCollection.Index).Cards;


        private DelegateCommand _addCard;
        public DelegateCommand AddCard =>
            _addCard ?? (_addCard = new DelegateCommand(ExecuteAddCard));

        private DelegateCommand<ReplaceableObjectPropertyChangedEventArgs> _switchCards;
        public DelegateCommand<ReplaceableObjectPropertyChangedEventArgs> SwitchCards =>
            _switchCards ?? (_switchCards = new DelegateCommand<ReplaceableObjectPropertyChangedEventArgs>(ExecuteSwitchCards));


        public CardsCollectionViewModel(IDragAndDropService dragAndDropService)
        {
            _dragAndDropService = dragAndDropService;

            CardCollection = _dragAndDropService.InitCollection();

            Title = CardCollection.Title;
        }

        private void ExecuteAddCard()
        {
            //TODO: Изменить.
            _dragAndDropService.SetItemToCollection(new Card("Test1"), CardCollection);
        }

        private void ExecuteSwitchCards(object sender)
        {
            if (sender is ReplaceableObjectPropertyChangedEventArgs args)
            {
                _dragAndDropService.SwitchItems(((CardViewModel)args.SelectedObject.DataContext).Card, ((CardViewModel)args.ReplaceableObject.DataContext).Card);
            }
        }
    }
}
