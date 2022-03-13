using PlanFuture.Modules.Projects.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlanFuture.Modules.Projects.ViewModels
{
    public class CardsCollectionViewModel : BindableBase
    {
        private string _title = "Test";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<Card> _cards;
        public ObservableCollection<Card> Cards
        {
            get { return _cards; }
            set { SetProperty(ref _cards, value); }
        }

        public CardsCollectionViewModel()
        {
            _cards = new ObservableCollection<Card>();

            _cards.Add(new Card());
            _cards.Add(new Card());
            _cards.Add(new Card());
        }
    }
}
