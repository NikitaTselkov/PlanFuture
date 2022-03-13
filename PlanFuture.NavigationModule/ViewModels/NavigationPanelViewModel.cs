using PlanFuture.Business;
using PlanFuture.Core;
using PlanFuture.Modules.NavigationModule.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace PlanFuture.Modules.NavigationModule.ViewModels
{
    public class NavigationPanelViewModel : ViewModelBase
    {
        private NavigationItem _dasboardNavigationItem;
        public NavigationItem DasboardNavigationItem
        {
            get { return _dasboardNavigationItem; }
            set { SetProperty(ref _dasboardNavigationItem, value); }
        }

        private NavigationItem _projectsNavigationItem;
        public NavigationItem ProjectsNavigationItem
        {
            get { return _projectsNavigationItem; }
            set { SetProperty(ref _projectsNavigationItem, value); }
        }

        private NavigationItem _myTaskNavigationItem;
        public NavigationItem MyTaskNavigationItem
        {
            get { return _myTaskNavigationItem; }
            set { SetProperty(ref _myTaskNavigationItem, value); }
        }

        private NavigationItem _settingsNavigationItem;
        public NavigationItem SettingsNavigationItem
        {
            get { return _settingsNavigationItem; }
            set { SetProperty(ref _settingsNavigationItem, value); }
        }



        private DelegateCommand<NavigationItem> _selectedCommand;
        private readonly IApplicationCommands _applicationCommands;

        public DelegateCommand<NavigationItem> SelectedCommand =>
            _selectedCommand ?? (_selectedCommand = new DelegateCommand<NavigationItem>(ExecuteSelectedCommand));

        public NavigationPanelViewModel(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;
            ConfigNavigationItems();
        }

        private void ExecuteSelectedCommand(NavigationItem navigationItem)
        {
            if (navigationItem != null)
                _applicationCommands.NavigateCommand.Execute(navigationItem.NavigationPath);
        }

        private void ConfigNavigationItems()
        {
            DasboardNavigationItem = new NavigationItem(Resources.Dasboard, NavigationPaths.MainDasboard);
            ProjectsNavigationItem = new NavigationItem(Resources.Projects, NavigationPaths.MainProject);
            MyTaskNavigationItem = new NavigationItem(Resources.My_Task, NavigationPaths.MainTask);
            SettingsNavigationItem = new NavigationItem(Resources.Settings, NavigationPaths.MainSettings);
        }
    }
}
