//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications
{
    using Infrastructure.Services;
    using Prism.Ioc;
    using Prism.Modularity;

    public class NotificationsModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<INotificationsService, NotificationsService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}   //ImprezGarage.Modules.Notification namespace 