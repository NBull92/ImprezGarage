//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure
{
    using ImprezGarage.Infrastructure.ViewModels;
    using Prism.Events;

    public class Events
    {
        public class SelectVehicleEvent : PubSubEvent<VehicleViewModel> { }
        public class StatusUpdateEvent : PubSubEvent<string> { }
        public class EditVehicleEvent : PubSubEvent<VehicleViewModel> { }
        public class RefreshDataEvent : PubSubEvent { }
    }
}   //ImprezGarage.Infrastructure namespace 