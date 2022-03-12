using PlanFuture.Core;
using PlanFuture.Modules.NavigationModule.ViewModels;
using PlanFuture.Modules.NavigationModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace PlanFuture.NavigationModule
{
    public class NavigationModuleModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public NavigationModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.NavigateBarRegion, typeof(NavigationPanel));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<NavigationPanel, NavigationPanelViewModel>();
        }
    }
}