//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications
{
    using ImprezGarage.Infrastructure.Services;

    internal class NotificationsService : INotificationsService
    {
        public void Alert(WindowButton windowButton, string message, string header = "Alert")
        {
            throw new System.NotImplementedException();
        }

        public bool Confirm(WindowButton windowButton, string message, string header = "Confirm")
        {
            throw new System.NotImplementedException();
        }
    }
}   //ImprezGarage.Modules.Notifications namespace