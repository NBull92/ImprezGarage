
namespace ImprezGarage.Modules.Firebase
{
    using Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;

    public class FirebaseModule : IModule
    {
        #region Attributes
        /// <summary>
        /// Store the container manager.
        /// </summary>
        private readonly IUnityContainer _container;
        #endregion

        #region Methods
        public FirebaseModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IDataService, FirebaseDataService>(new ContainerControlledLifetimeManager());
        }
        #endregion
    }
}