
namespace ImprezGarage.Modules.FirebaseAuth
{
    using Infrastructure;
    using Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;

    public class AuthModule : IModule
    {
        #region Attributes
        /// <summary>
        /// Store the container manager.
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Store the region manager.
        /// </summary>
        private readonly IRegionManager _regionManager;
        #endregion

        #region Methods
        public AuthModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<IAuthenticationService, FirebaseAuthenticationService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<object, SignIn>(typeof(SignIn).FullName);
            _container.RegisterType<object, CreateAccount>(typeof(CreateAccount).FullName);

            _regionManager.RegisterViewWithRegion(RegionNames.AuthenticateRegion, typeof(SignIn));
        }
        #endregion
    }
}