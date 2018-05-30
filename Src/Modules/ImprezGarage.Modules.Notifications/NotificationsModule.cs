﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications
{
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Regions;

    public class NotificationsModule : IModule
    {
        private IUnityContainer _container;

        public NotificationsModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<INotificationsService, NotificationsService>(new ContainerControlledLifetimeManager());
        }
    }
}   //ImprezGarage.Modules.Notification snamespace 