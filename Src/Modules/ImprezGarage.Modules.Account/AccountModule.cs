using CommonServiceLocator;

namespace ImprezGarage.Modules.Account
{
    using CountriesWrapper;
    using Infrastructure;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;
    using Microsoft.Practices.Unity;

    public class AccountModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public AccountModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public async void Initialize()
        {
            _container.RegisterType<ICountryManager, CountryManager>(new ContainerControlledLifetimeManager());
            _regionManager.RegisterViewWithRegion(RegionNames.VehicleHeaderRegion, typeof(ProfileHeader));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ProfilePage));
            await _container.Resolve<ICountryManager>().InitialiseAsync();
        }
    }
}