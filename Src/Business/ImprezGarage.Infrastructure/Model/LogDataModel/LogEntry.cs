//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model.LogDataModel
{
    using System;

    public class LogEntry
    {
        public TimeSpan Time { get; set; }
        public string Message { get; set; }
    }
}   //ImprezGarage.Modules.Logger.DataModels namespace 