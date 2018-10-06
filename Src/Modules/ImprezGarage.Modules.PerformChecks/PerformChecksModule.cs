//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks
{
    using Microsoft.Practices.Unity;
    using Prism.Regions;
    using Prism.Modularity;
    using Views;
    using Infrastructure;

    public class PerformChecksModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public PerformChecksModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {   
            _container.RegisterType<object, Main>(typeof(Main).FullName);
            _container.RegisterType<object, PerformNewCheck>(typeof(PerformNewCheck).FullName);

            _regionManager.RegisterViewWithRegion(RegionNames.ChecksRegion, typeof(Main));
            _regionManager.RegisterViewWithRegion(RegionNames.ChecksPerformedRegion, typeof(ChecksPerformed));            
        }
    }
}   //ImprezGarage.Modules.PerformChecks namespace 