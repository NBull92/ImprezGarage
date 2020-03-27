
namespace ImprezGarage.Modules.Account
{
    using Infrastructure;
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

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.VehicleHeaderRegion, typeof(ProfileHeader));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ProfilePage));
        }
    }
}