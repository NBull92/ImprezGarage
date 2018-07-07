//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using System;

    public interface INotificationsService
    {
        void Alert(string message, string header = "Alert");
        bool Confirm(string message, string header = "Confirm", Action action = null);
        void Toast(string message, string header = "ImprezGarage", string buttonContent = null, Action action = null);
    }
}   //ImprezGarage.Modules.Notifications namespace 