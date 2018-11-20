//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger.DataModels
{
    using Configuration;
    using Infrastructure.Model.LogDataModel;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the level of detail stored in the logs.
    /// </summary>
    public enum LogDetail
    {
        Simple,
        Verbose
    }

    /// <summary>
    /// Defines the file type the logs are stored as.
    /// </summary>
    public enum LogFileType
    {
        Txt,
        Csv
    }

    public class LogModel
    {
        // Private member attributes
        #region Attributes
        /// <summary>
        /// Store all of the log files.
        /// </summary>
        private readonly ObservableCollection<Log> _logsArchive = new ObservableCollection<Log>();

        /// <summary>
        /// Store all of the saved screenshots.
        /// </summary>
        private readonly ObservableCollection<string> _screenshots = new ObservableCollection<string>();

        /// <summary>
        /// Store a copy of the current configuration.
        /// </summary>
        private LogsConfiguration _logsConfiguration;

        /// <summary>
        /// Store the location of the configuration file.
        /// </summary>
        private readonly string _configFileLocation;

        /// <summary>
        /// Store the folder that stores all of the logs.
        /// </summary>
        private readonly string _logFolderLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ImprezGarage\\Logs";
        #endregion

        // Private and public methods
        #region Methods
        /// <summary>
        /// Construct the log model and set the config file location.
        /// </summary>
        public LogModel()
        {
            _configFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ImprezGarage\\LogsConfiguration.xml";

            SetupFileDirectory();
        }

        /// <summary>
        /// Check to see if the directory for the config file exists already.
        /// If not then create it.
        /// </summary>
        private void SetupFileDirectory()
        {
            FileInfo test = new FileInfo(_configFileLocation);
            
            if(!test.Directory.Exists)
            {
                Directory.CreateDirectory(test.Directory.FullName);
            }
        }

        /// <summary>
        /// Get the current log file.
        /// </summary>
        public Log GetCurrentLog()
        {
            return _logsArchive.Any(o => o.IsCurrent) ? _logsArchive.FirstOrDefault(o => o.IsCurrent) : null;
        }
        /// <summary>
        /// Add a new log file to the collection.
        /// </summary>
        public void AddLogFile(Log log)
        {
            _logsArchive.Add(log);
        }

        /// <summary>
        /// Return the list of screenshots.
        /// </summary>
        public ObservableCollection<string> GetScreenshots()
        {
            return _screenshots;
        }

        /// <summary>
        /// Take the passed through message and parameters and create a new log entry. 
        /// Add it to the list of log messages for the current log file.
        /// </summary>
        public void AddLogEntry(string message = null, params object[] parameters)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                message = string.Format(message, parameters);

                GetCurrentLog()?.LogMessages.Add(new LogEntry(message));
            }));
        }

        /// <summary>
        /// Take the passed through exception and create a new log entry. 
        /// Add it to the list of log messages for the current log file.
        /// </summary>
        public void AddLogEntry(Exception exception, string message, params object[] parameters)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                message = string.Format(message, parameters);

                GetCurrentLog()?.LogMessages.Add(new LogEntry(message, exception));
            }));
        }

        /// <summary>
        /// Return the selected LogLevel associated with the passed through string, or return the first from the collection.
        /// </summary>
        public LogDetail GetSelectedLogDetail()
        {
            return !String.IsNullOrEmpty(_logsConfiguration.LogDetail) ? Enum.GetValues(typeof(LogDetail)).Cast<LogDetail>().ToList().FirstOrDefault(o => o.ToString() == _logsConfiguration.LogDetail)
            : Enum.GetValues(typeof(LogDetail)).Cast<LogDetail>().ToList().FirstOrDefault();
        }

        /// <summary>
        /// Return the selected LogFileType associated with the passed through string, or return the first from the collection.
        /// </summary>
        public LogFileType GetSelectedLogFileTypes()
        {
            return !String.IsNullOrEmpty(_logsConfiguration.LogFileType) ? Enum.GetValues(typeof(LogFileType)).Cast<LogFileType>().FirstOrDefault(o => o.ToString() == _logsConfiguration.LogFileType)
            : Enum.GetValues(typeof(LogFileType)).Cast<LogFileType>().ToList().FirstOrDefault();
        }

        /// <summary>
        /// Return the selected number of days for a logs life.
        /// </summary>
        public int GetSelectedLogLife()
        {
            if (_logsConfiguration == null)
                return 0;

            return _logsConfiguration.LogLife == 0 ? 7 : _logsConfiguration.LogLife;
        }

        /// <summary>
        /// This function will go through the logs folder and see if any of the logs are older than the selected log level (in months).
        /// If so then it will delete them.
        /// </summary>
        public void ManageLogs(DirectoryInfo logFolder)
        {
            var logFiles = logFolder.GetFiles();

            foreach (var file in logFiles)
            {
                var shortDateString = File.GetCreationTime(file.FullName).ToShortDateString();

                if (Convert.ToDateTime(shortDateString) <= DateTime.Today.AddDays(-GetSelectedLogLife()))
                {
                    file.Delete();
                }
            }

            foreach (var fileInfo in logFolder.GetFiles())
            {
                var log = new Log(fileInfo);
                AddLogFile(log);
            }

            if (GetCurrentLog() != null)
                return;

            var newLog = new Log();

            var dt = Convert.ToDateTime(newLog.Date);

            newLog.FilePath = logFolder.FullName + "\\" + dt.Date.Year + "_" + dt.Date.Month + "_" + dt.Date.Day + "_Logs." + _logsConfiguration.LogFileType;
            AddLogFile(newLog);
        }

        /// <summary>
        /// Update the level of log detail the user would prefer to see.
        /// </summary>
        public void SetLogLevel(string logLevel)
        {
            _logsConfiguration.LogDetail = logLevel;
        }

        /// <summary>
        /// Update the type of log file the user would like to save the logs to. 
        /// </summary>
        public void SetLogFileType(string fileType)
        {
            _logsConfiguration.LogFileType = fileType;
        }

        /// <summary>
        /// Update the selected Log Life to the configuration model.
        /// </summary>
        public void SetLogLife(int logLIfe)
        {
            _logsConfiguration.LogLife = logLIfe;
        }

        /// <summary>
        /// Store the passed through logger config file to the current parameter and then update the settings.
        /// </summary>
        public void SetLogsConfiguration(LogsConfiguration logsConfiguration)
        {
            _logsConfiguration = logsConfiguration;

            SetLogLevel(_logsConfiguration.LogDetail);
            SetLogFileType(_logsConfiguration.LogFileType);
            SetLogLife(_logsConfiguration.LogLife);
        }

        /// <summary>
        /// Return the current logger config file.
        /// </summary>
        public LogsConfiguration GetConfigFile()
        {
            return _logsConfiguration;
        }

        /// <summary>
        /// This function will go through the logs folder and see if any of the logs are more than a months old. 
        /// If so then it will delete them.
        /// </summary>
        public void ManageLogsFolder()
        {
            LoadConfigFile();

            CheckLogFoldersExist();

            ManageLogs(new DirectoryInfo(_logFolderLocation));
        }

        /// <summary>
        /// ImprezGarage makes use of a folder structure in the app data to store the log contents. 
        /// This function makes sure these folders exist.
        /// </summary>
        private void CheckLogFoldersExist()
        {
            if (!Directory.Exists(_logFolderLocation))
            {
                Directory.CreateDirectory(_logFolderLocation);
            }

            var archiveLogsFolder = _logFolderLocation + "\\Archive";

            if (!Directory.Exists(archiveLogsFolder))
            {
                Directory.CreateDirectory(archiveLogsFolder);
            }

            var screenshotFolder = _logFolderLocation + "\\Screenshot";

            if (!Directory.Exists(screenshotFolder))
            {
                Directory.CreateDirectory(screenshotFolder);
            }
        }

        /// <summary>
        /// Using the location of the configuration file, check to see if the file already exists.
        /// If it does exist then, deserialize the file with an XML serializer and then store the config file in this service.
        /// Otherwise just create a new default logs configuration.
        /// </summary>
        private void LoadConfigFile()
        {            
            LogsConfiguration logsConfiguration = null;

            if (!File.Exists(_configFileLocation))
            {
                File.Create(_configFileLocation).Dispose();
                logsConfiguration = CreateNewLogConfiguration();
            }
            else
            {
                FileStream readFileStream = null;

                try
                {
                    using (readFileStream = File.OpenRead(_configFileLocation))
                    {
                        var hierarchySerializer = new XmlSerializer(typeof(LogsConfiguration));

                        // Try to deserialize the config file as a Logger config. If this does not succeed then create a new logger config.
                        logsConfiguration = hierarchySerializer.Deserialize(readFileStream) as LogsConfiguration;
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    AddLogEntry("New log configuration has been created.", ioe);
                    logsConfiguration = CreateNewLogConfiguration();
                }
                catch (Exception ex)
                {
                    AddLogEntry("An error occured when loading the logs configuration file..", ex);
                }
                finally
                {
                    readFileStream?.Close();
                }
            }

            SetLogsConfiguration(logsConfiguration);
        }

        /// <summary>
        /// Create a new logs configuration.
        /// </summary>
        private static LogsConfiguration CreateNewLogConfiguration()
        {
            return new LogsConfiguration
            {
                LogDetail = LogDetail.Simple.ToString(),
                LogFileType = LogFileType.Txt.ToString(),
                LogLife = 28,
            };
        }

        /// <summary>
        /// Return the location of the configuration file.
        /// </summary>
        public string GetConfigFileLocation()
        {
            return _configFileLocation;
        }
        #endregion
    }
}   //ImprezGarage.Modules.Logger.DataModels namespace 