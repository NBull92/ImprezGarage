//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure
{
    using Prism.Events;
    using System;

    /// <summary>
    /// Events that are published and subscribe to through the different modules.
    /// This allows for some small amount of data to be passed between them.
    /// </summary>
    public class Events
    {
        /// <summary>
        /// Event for when the status has been updated for the status bar.
        /// </summary>
        public class StatusUpdateEvent : PubSubEvent<string> { }

        /// <summary>
        /// Event for when the user requests to refresh all of the current data.
        /// </summary>
        public class RefreshDataEvent : PubSubEvent { }

        /// <summary>
        /// Event for when a user logs in and out
        /// <param name="Item1">bool for if the user is logged in or not.</param>
        /// <param name="Item2">string to store the user Id, for when a user logs in.</param>
        /// </summary>
        public class UserAccountChange : PubSubEvent<Tuple<bool,string>> { }
    }
}   //ImprezGarage.Infrastructure namespace 