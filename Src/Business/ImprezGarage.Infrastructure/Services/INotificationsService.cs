//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using System;

    public interface INotificationsService
    {
        /// <summary>
        /// Create a simple alert to inform or warn a user.
        /// </summary>
        void Alert(string message, string header = "Alert");

        /// <summary>
        /// Create an alert that will allow the user to confirm something.
        /// Then return result of the users action and called any 'action'.
        /// </summary>
        bool Confirm(string message, string header = "Confirm", Action action = null);

        /// <summary>
        /// Create a desktop notification to inform the user of important information.
        /// The passed through action will allow them to proceed with whatever you want them too.
        /// </summary>
        void Toast(string message, string header = "ImprezGarage", string buttonContent = null, Action action = null);
    }
}   //ImprezGarage.Modules.Notifications namespace 