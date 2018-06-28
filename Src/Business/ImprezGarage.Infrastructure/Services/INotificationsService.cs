//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    public enum WindowButton
    {
        Ok,
        OkCancel,
        YesNo
    }

    public interface INotificationsService
    {
        void Alert(WindowButton windowButton, string message, string header = "Alert");
        bool Confirm(WindowButton windowButton, string message, string header = "Confirm");
    }
}   //ImprezGarage.Modules.Notifications namespace 