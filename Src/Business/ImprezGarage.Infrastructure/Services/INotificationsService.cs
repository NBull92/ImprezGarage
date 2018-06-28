//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    public interface INotificationsService
    {
        void Alert(string message, string header = "Alert");
        bool Confirm(string message, string header = "Confirm");
    }
}   //ImprezGarage.Modules.Notifications namespace 