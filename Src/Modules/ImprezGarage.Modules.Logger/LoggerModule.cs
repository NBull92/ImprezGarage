//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger
{
    using DataModels;
    using Infrastructure;
    using Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Regions;

    public class LoggerModule : IModule
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
        /// <summary>
        /// Log module constructor. Store the dependencies.
        /// </summary>
        public LoggerModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<ILoggerService, LoggerService>(new ContainerControlledLifetimeManager());

            var loggerDataModel = new LogModel();
            _regionManager.Regions[RegionNames.StatusBarRegion].Context = loggerDataModel;

            var loggerService = (LoggerService)_container.Resolve<ILoggerService>();
            loggerService.SetDataModel(loggerDataModel);
        }
        #endregion
    }
}   //ImprezGarage.Modules.Logger namespace 