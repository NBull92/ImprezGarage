//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure
{
    using ViewModels;
    using Prism.Events;

    /// <summary>
    /// Events that are published and subscribe to through the different modules.
    /// This allows for some small amount of data to be passed between them.
    /// </summary>
    public class Events
    {
        /// <summary>
        /// Event for when a vehicle is selected by the user.
        /// </summary>
        public class SelectVehicleEvent : PubSubEvent<VehicleViewModel> { }

        /// <summary>
        /// Event for when the status has been updated for the status bar.
        /// </summary>
        public class StatusUpdateEvent : PubSubEvent<string> { }

        /// <summary>
        /// Event for when a vehicle is being edited.
        /// </summary>
        public class EditVehicleEvent : PubSubEvent<VehicleViewModel> { }

        /// <summary>
        /// Event for when the user requests to refresh all of the current data.
        /// </summary>
        public class RefreshDataEvent : PubSubEvent { }
    }
}   //ImprezGarage.Infrastructure namespace 