//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage
{
    using Infrastructure;
    using Infrastructure.Services;
    using Prism.Ioc;
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

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.VehicleListRegion, typeof(MainView));
            _regionManager.RegisterViewWithRegion(RegionNames.VehicleHeaderRegion, typeof(VehicleHeader));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ManageVehicle));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}   //ImprezGarage.Modules.MyGarage namespace 