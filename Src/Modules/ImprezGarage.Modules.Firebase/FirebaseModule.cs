namespace ImprezGarage.Modules.Firebase
{
    using Infrastructure.Services;
    using Prism.Modularity;
    using Prism.Ioc;

    public class FirebaseModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDataService, FirebaseDataService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}