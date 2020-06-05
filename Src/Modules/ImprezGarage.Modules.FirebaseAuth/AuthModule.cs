//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.FirebaseAuth
{
    using Infrastructure;
    using Infrastructure.Services;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;

    public class AuthModule : IModule
    {
        #region Attributes
        /// <summary>
        /// Store the region manager.
        /// </summary>
        private readonly IRegionManager _regionManager;
        #endregion

        #region Methods
        public AuthModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAuthenticationService, FirebaseAuthenticationService>();
            containerRegistry.Register<object, SignIn>(typeof(SignIn).FullName);
            containerRegistry.Register<object, CreateAccount>(typeof(CreateAccount).FullName);

            _regionManager.RegisterViewWithRegion(RegionNames.AuthenticateRegion, typeof(SignIn));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
        #endregion
    }
}   // ImprezGarage.Modules.FirebaseAuth namespace 