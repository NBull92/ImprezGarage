//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using System;

    /// <summary>
    /// Service that provides global access to logging functionality
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Log a more in depth entry, with parameters as well as the basic message.
        /// </summary>
        void Log(string message, params object[] parameters);

        /// <summary>
        /// Make a more in depth log entry by accepting the exception as a parameter and print out it's contents.
        /// </summary>
        void LogException(Exception exception);

        /// <summary>
        /// Get all of the currently saved log messages and print it out in a text file.
        /// </summary>
        void PrintLogFile();

        /// <summary>
        /// Using an XML serializer, print out the current logger configuration settings to an XML.
        /// </summary>
        void PrintConfigurationFile();
    }
}   //ImprezGarage.Modules.Logger namespace 