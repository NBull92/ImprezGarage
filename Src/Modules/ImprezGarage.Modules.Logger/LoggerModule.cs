//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger
{
    using DataModels;
    using Infrastructure;
    using Infrastructure.Services;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    public class LoggerModule : IModule
    {
        /// <summary>
        /// Store the region manager.
        /// </summary>
        private readonly IRegionManager _regionManager;

        #region Methods
        /// <summary>
        /// Log module constructor. Store the dependencies.
        /// </summary>
        public LoggerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILoggerService, LoggerService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var loggerDataModel = new LogModel();
            _regionManager.Regions[RegionNames.ContentRegion].Context = loggerDataModel;
            var loggerService = (LoggerService)containerProvider.Resolve<ILoggerService>();
            loggerService.SetDataModel(loggerDataModel);
        }
        #endregion
    }
}   //ImprezGarage.Modules.Logger namespace 