
namespace ImprezGarage.Modules.Account
{
    using Prism.Modularity;
    using Prism.Regions;

    public class AccountModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public AccountModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            
        }
    }
}