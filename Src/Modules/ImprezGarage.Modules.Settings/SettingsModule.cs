using ImprezGarage.Modules.Settings.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace ImprezGarage.Modules.Settings
{
    public class SettingsModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public SettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<ViewA>();
        }
    }
}