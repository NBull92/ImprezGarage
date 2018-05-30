//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Dashboard
{
    using Prism.Modularity;
    using Prism.Regions;
    using Microsoft.Practices.Unity;
    using Prism.Unity;

    public class DashboardModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public DashboardModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<object, Views.Dashboard>(typeof(Views.Dashboard).FullName);
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Views.Dashboard));
        }
    }
}   //ImprezGarage.Modules.Dashboard namespace 