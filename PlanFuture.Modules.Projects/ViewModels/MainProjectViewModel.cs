using PlanFuture.Business;
using PlanFuture.Core;
using Prism.Commands;
using Prism.Mvvm;
using PlanFuture.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        private ObservableCollection<ICardCollection> _cardCollections;
        public ObservableCollection<ICardCollection> CardCollections
        {
            get { return _cardCollections; }
            set { SetProperty(ref _cardCollections, value); }
        }


        private DelegateCommand _addCardCollection;
        public DelegateCommand AddCardCollection =>
            _addCardCollection ?? (_addCardCollection = new DelegateCommand(ExecuteAddCardCollection));

        public MainProjectViewModel(IDragAndDropService dragAndDropService)
        {
            _dragAndDropService = dragAndDropService;

            CardCollections = new ObservableCollection<ICardCollection>(_dragAndDropService.GetAllCollections());
        }

        private void ExecuteAddCardCollection()
        {
            // TODO: Сделать Validation
            if(!string.IsNullOrWhiteSpace(CardTitle))
                CardCollections.Add(_dragAndDropService.SetCollection(new CardCollection(CardTitle)));
        }
    }
}
