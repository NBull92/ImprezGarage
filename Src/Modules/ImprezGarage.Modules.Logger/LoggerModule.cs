//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger
{
    using ImprezGarage.Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;

    public class LoggerModule : IModule
    {
        private IUnityContainer _container;

        public LoggerModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ILoggerService, LoggerService>(new ContainerControlledLifetimeManager());
        }        
    }
}   //ImprezGarage.Modules.Logger namespace 