//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model.LogDataModel
{
    using System;

    /// <summary>
    /// This class is for creating a new piece of data to add to a log.
    /// </summary>
    public class LogEntry
    {
        // Public properties
        #region Properties
        /// <summary>
        /// Store what time the issue was logged.
        /// </summary>
        public string DateTimeStamp { get; set; }

        /// <summary>
        /// Store the message of the issue.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Store the type of exception that occured.
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// STore the modules the error came from.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Store the stacktrace of the exception.
        /// </summary>
        public string StackTrace { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// When constructed, if an exception is passed through, pass it's parameters to the log entry parameters.
        /// </summary>
        public LogEntry(string message = "", Exception exception = null)
        {
            DateTimeStamp = DateTime.Now.ToShortTimeString();

            if (exception != null)
            {
                Message = exception.Message;
                ExceptionType = exception.GetType().ToString();
                Source = exception.Source;
                StackTrace = exception.StackTrace;
            }
            else
            {
                Message = message;
            }
        }
        #endregion
    }
}   //ImprezGarage.Modules.Logger.DataModels namespace 