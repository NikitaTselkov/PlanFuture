using PlanFuture.Core;
using PlanFuture.Modules.DasboardModule;
using PlanFuture.Modules.NavigationModule;
using PlanFuture.Modules.Projects;
using PlanFuture.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace PlanFuture
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<NavigationModule>();
            moduleCatalog.AddModule<DasboardModule>();
            moduleCatalog.AddModule<ProjectsModule>();
        }
    }
}
