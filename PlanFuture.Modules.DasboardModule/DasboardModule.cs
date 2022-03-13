using PlanFuture.Core;
using PlanFuture.Modules.DasboardModule.ViewModels;
using PlanFuture.Modules.DasboardModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace PlanFuture.Modules.DasboardModule
{
    public class DasboardModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public DasboardModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(MainDasboard));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<MainDasboard, MainDasboardViewModel>();

            containerRegistry.RegisterForNavigation<MainDasboard, MainDasboardViewModel>(NavigationPaths.MainDasboard);
        }
    }
}