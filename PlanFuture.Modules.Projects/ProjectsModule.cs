using PlanFuture.Core;
using PlanFuture.Modules.Projects.ViewModels;
using PlanFuture.Modules.Projects.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using PlanFuture.Services;

namespace PlanFuture.Modules.Projects
{
    public class ProjectsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ProjectsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<MainProject, MainProjectViewModel>();

            containerRegistry.RegisterForNavigation<MainProject, MainProjectViewModel>(NavigationPaths.MainProject);
            containerRegistry.RegisterSingleton<IDragAndDropService, DragAndDropService>();
        }
    }
}