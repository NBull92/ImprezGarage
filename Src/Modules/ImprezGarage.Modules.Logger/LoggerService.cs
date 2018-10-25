//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger
{
    using Configuration;
    using DataModels;
    using Infrastructure.Services;
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    /// <inheritdoc />
    /// <summary>
    /// A class that implements the concrete functions of the logger interface.
    /// This is where all of the functionality will be implemented.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        #region Attributes
        /// <summary>
        /// Store the current instance of the data model.
        /// </summary>
        private LogModel _loggerDataModel;
        #endregion

        #region Methods        
        /// <summary>
        /// Log a more in depth entry, with parameters as well as the basic message.
        /// </summary>
        public void Log(string message, params object[] parameters)
        {
            _loggerDataModel.AddLogEntry(message, parameters);
        }

        /// <summary>
        /// Make a more in depth log entry by accepting the exception as a parameter and print out it's contents.
        /// </summary>
        public void LogException(Exception exception, string message = null, params object[] parameters)
        {
            // Check the currently chosen level of log details and either print just the exception's message or store the whole exception in the log entry.
            if (_loggerDataModel.GetSelectedLogDetail() == LogDetail.Simple)
            {
                Log(message + $"\n" + exception.Message, parameters);
            }
            else
            {
                _loggerDataModel.AddLogEntry(exception, message, parameters);
            }
        }

        /// <summary>
        /// Get all of the currently saved log messages and print it out in a text file.
        /// </summary>
        public void PrintLogFile()
        {
            // Print the current log file.
            var current = _loggerDataModel.GetCurrentLog();

            if (current == null)
                return;

            // Check to see which type of log file the user has selected.
            if (_loggerDataModel.GetSelectedLogFileTypes() == LogFileType.Txt)
            {
                // Get the file path and see if the current file is a csv. If so, then delete it.
                var filePath = current.FilePath;
                if (filePath.Contains(".csv"))
                {
                    File.Delete(filePath);
                    filePath = current.FilePath.Replace("csv", "txt");
                }

                // Create/update the the txt file.
                File.Create(filePath).Dispose();

                using (var writer = new StreamWriter(filePath, true))
                {
                    foreach (var logMessage in current.LogMessages)
                    {
                        if (!string.IsNullOrEmpty(logMessage.DateTimeStamp))
                        {
                            writer.WriteLine(logMessage.DateTimeStamp + " : " + logMessage.Message);
                        }
                        else
                        {
                            writer.WriteLine(logMessage.Message);
                        }

                        if (logMessage.ExceptionType != null)
                            writer.WriteLine(logMessage.ExceptionType);

                        if (!string.IsNullOrEmpty(logMessage.Source))
                            writer.WriteLine(logMessage.Source);

                        if (!string.IsNullOrEmpty(logMessage.StackTrace))
                            writer.WriteLine(logMessage.StackTrace);
                    }
                }
            }
            else
            {
                // Get the file path and see if the current file is a txt. If so, then delete it.
                var filePath = current.FilePath;
                if (filePath.Contains(".txt"))
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    filePath = current.FilePath.Replace("txt", "csv");
                }

                // Create/update the the csv file.
                File.Create(filePath).Dispose();

                using (var writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Time,Message,Exception,Source,StackTrace");

                    foreach (var logMessage in current.LogMessages)
                    {
                        var sb = new StringBuilder();

                        sb.Append(!string.IsNullOrEmpty(logMessage.DateTimeStamp) ? logMessage.DateTimeStamp : "");

                        sb.Append(",");

                        sb.Append(logMessage.Message);

                        sb.Append(",");

                        if (logMessage.ExceptionType != null)
                        {
                            sb.Append(logMessage.ExceptionType);
                        }

                        sb.Append(",");

                        if (!string.IsNullOrEmpty(logMessage.Source))
                        {
                            sb.Append(logMessage.Source);
                        }

                        sb.Append(",");

                        if (!string.IsNullOrEmpty(logMessage.StackTrace))
                        {
                            sb.Append(logMessage.StackTrace);
                        }

                        writer.WriteLine(sb.ToString());
                    }
                }
            }

            var screenshots = _loggerDataModel.GetScreenshots();

            foreach (var screenshot in screenshots)
            {
                File.Create(screenshot).Dispose();
            }
        }

        /// <summary>
        /// Using an XML serializer, print out the current logger configuration settings to an XML.
        /// </summary>
        public void PrintConfigurationFile()
        {
            // Print the logger configuration file.
            var serializer = new XmlSerializer(typeof(LogsConfiguration));
            using (var writer = new StreamWriter(_loggerDataModel.GetConfigFileLocation()))
            {
                serializer.Serialize(writer, _loggerDataModel.GetConfigFile());
            }
        }

        /// <summary>
        /// Store the passed through data model and then manage sort out the logs folder.
        /// </summary>
        public void SetDataModel(LogModel loggerDataModel)
        {
            _loggerDataModel = loggerDataModel;
            _loggerDataModel?.ManageLogsFolder();
        }
        #endregion
    }
}   //ImprezGarage.Modules.Logger namespace 