//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Account
{
    using CountriesWrapper;
    using Infrastructure;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;

    public class AccountModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public AccountModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICountryManager, CountryManager>();
        }

        public async void OnInitialized(IContainerProvider containerProvider)
        {
            await containerProvider.Resolve<ICountryManager>().InitialiseAsync();
            _regionManager.RegisterViewWithRegion(RegionNames.VehicleHeaderRegion, typeof(ProfileHeader));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ProfilePage));
        }
    }
}   // ImprezGarage.Modules.Account namespace 