//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger
{
    using ImprezGarage.Infrastructure.Services;
    using ImprezGarage.Modules.Logger.DataModels;
    using System;
    using System.Collections.ObjectModel;

    internal class LoggerService : ILoggerService
    {
        private ObservableCollection<LogEntry> _logEntries = new ObservableCollection<LogEntry>();

        public void Log(string message)
        {
            _logEntries.Add(new LogEntry { Time = DateTime.Now.TimeOfDay, Message = message });
        }
    }
}   //ImprezGarage.Modules.Logger namespace 