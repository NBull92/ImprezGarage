//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.StatusBar
{
    using Prism.Modularity;
    using Prism.Regions;
    using Microsoft.Practices.Unity;
    using ImprezGarage.Infrastructure;

    public class StatusBarModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public StatusBarModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {            
            _container.RegisterType<object, Views.StatusBar>(typeof(Views.StatusBar).FullName);
            _regionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(Views.StatusBar));
        }
    }
}   //ImprezGarage.Modules.StatusBar namespace 