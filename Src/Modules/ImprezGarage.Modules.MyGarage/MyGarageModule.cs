//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage
{
    using ImprezGarage.Infrastructure;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;

    public class MyGarageModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MyGarageModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.VehicleListRegion, typeof(MainView));
        }
    }
}   //ImprezGarage.Modules.MyGarage namespace 