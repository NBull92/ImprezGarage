//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model.LogDataModel
{
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// This class if for creating a 'log' and storing it's content in ImprezGarage, before being saved to a file upon closure of ImprezGarage.
    /// </summary>
    public class Log : BindableBase
    {
        // Private member attributes
        #region Attributes
        /// <summary>
        /// Store whether this is the currently used log file or not.
        /// </summary>
        private bool _isCurrent;

        /// <summary>
        /// Store the label to be used in the UI for this log file.
        /// </summary>
        private string _label;

        /// <summary>
        /// Store the size of the log file.
        /// </summary>
        private string _fileSize;

        /// <summary>
        /// Store all of the content of the log file.
        /// </summary>
        private ObservableCollection<LogEntry> _logMessages;

        /// <summary>
        /// Store the full path of the log file.
        /// </summary>
        public string FilePath;
        #endregion

        // Public properties
        #region Properties
        /// <summary>
        /// Store whether this is the currently used log file or not.
        /// </summary>
        public bool IsCurrent
        {
            get => _isCurrent;
            set => SetProperty(ref _isCurrent, value);
        }

        /// <summary>
        /// Store the label to be used in the UI for this log file.
        /// </summary>
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        /// <summary>
        /// Store the date in which this log file was created.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Store the size of the log file.
        /// </summary>
        public string FileSize
        {
            get => _fileSize;
            set => SetProperty(ref _fileSize, value);
        }

        /// <summary>
        /// Store all of the content of the log file.
        /// </summary>
        public ObservableCollection<LogEntry> LogMessages
        {
            get => _logMessages;
            set => SetProperty(ref _logMessages, value);
        }
        #endregion

        // Public and private methods
        #region Methods

        #region Initialisation
        /// <summary>
        /// Log constructor. Instantiate the Command and store the dependencies.
        /// This can take in the file info that this log Vm will represent and it will populate the properties accordingly.
        /// </summary>
        public Log(FileInfo fileInfo = null)
        {
            // Check if a file was not passed through.
            if (fileInfo == null)
            {
                // Set up a blank log as the current log.
                FilePath = string.Empty;
                Date = DateTime.Today.ToShortDateString();
                IsCurrent = true;
                Label = "Current Log";
                LogMessages = new ObservableCollection<LogEntry>();
            }
            // If one was passed through then setup up with the information of said file.
            else
            {
                FilePath = fileInfo.FullName;
                string fileLength;

                if (fileInfo.Length >= (1 << 30))
                {
                    fileLength = $"{fileInfo.Length >> 30} GB";
                }
                else if (fileInfo.Length >= (1 << 20))
                {
                    fileLength = $"{fileInfo.Length >> 20} MB";
                }
                else if (fileInfo.Length >= (1 << 10))
                {
                    fileLength = $"{fileInfo.Length >> 10} KB";
                }
                else
                {
                    fileLength = "1Kb";
                }

                FileSize = "(" + fileLength + ")";

                Date = File.GetCreationTime(FilePath).ToShortDateString();

                if (Date == DateTime.Today.ToShortDateString())
                {
                    IsCurrent = true;
                    Label = "Current Log";
                }
                else
                {
                    Label = "[" + Date + "]" + " Log " + FileSize;
                }

                PopulateLogMessages();
            }
        }
        #endregion

        /// <summary>
        /// This will read in the log entries from the file attached to this log and then populate the collection of log entries.
        /// If this is the current log file, then it will also add the currently stored log entries, not yet printed to the file, to display in the UI.
        /// </summary>
        public void PopulateLogMessages()
        {
            LogMessages = new ObservableCollection<LogEntry>();

            if (string.IsNullOrEmpty(FilePath))
                return;

            // Check if the log being read in, is a csv file.
            if (FilePath.Contains(".Csv"))
            {
                StreamReader sr = null;
                try
                {
                    using (sr = new StreamReader(FilePath))
                    {
                        sr.ReadLine();
                        while (sr.Peek() != -1)
                        {
                            var line = sr.ReadLine();

                            if (line == null)
                                return;

                            var values = line.Split(',');

                            if (DateTime.TryParse(values[0], out DateTime dateTime))
                            {
                                var message = new LogEntry();
                                message.DateTimeStamp = dateTime.ToShortTimeString();
                                message.Message = values[1];

                                if (message.Message.StartsWith(" : "))
                                {
                                    message.Message = message.Message.Remove(0, 3);
                                }

                                if (values.Length > 2 && !string.IsNullOrEmpty(values[2]))
                                {
                                    message.ExceptionType = values[2];
                                }

                                if (values.Length > 3 && !string.IsNullOrEmpty(values[3]))
                                {
                                    message.Source = values[3];
                                }

                                if (values.Length > 4 && !string.IsNullOrEmpty(values[4]))
                                {
                                    message.StackTrace = values[4];
                                }

                                LogMessages.Add(message);
                            }
                            else
                            {
                                if (!LogMessages.Any())
                                    continue;

                                var previousLog = LogMessages.LastOrDefault();
                                if (previousLog != null)
                                {
                                    previousLog.StackTrace += "\n" + line;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessages.Add(new LogEntry("An error occured when attempting to load the current log entries.", ex));
                }
                finally
                {
                    sr?.Close();
                }
            }
            // Else it is a txt file.
            else
            {
                var sr = new StreamReader(FilePath);
                var line = sr.ReadLine();
                var pattern = @"\d+:\d+";

                while (line != null)
                {
                    var message = new LogEntry();
                    var input = line;

                    message.Message = line;

                    foreach (Match m in Regex.Matches(input, pattern))
                    {
                        message.DateTimeStamp = m.Value;

                        if (!string.IsNullOrEmpty(m.Value) && message.Message.Contains(m.Value))
                        {
                            message.Message = message.Message.Replace(m.Value, "");
                        }
                    }

                    if (!string.IsNullOrEmpty(Date) && message.Message.Contains(Date))
                    {
                        message.Message = message.Message.Replace(Date, "");
                    }

                    if (message.Message.StartsWith(" : "))
                    {
                        message.Message = message.Message.Remove(0, 3);
                    }

                    if (!string.IsNullOrEmpty(message.DateTimeStamp))
                    {
                        LogMessages.Add(message);
                    }
                    else
                    {
                        var last = LogMessages.LastOrDefault();

                        if (last == null)
                            return;

                        if (last.ExceptionType == null)
                        {
                            last.ExceptionType = message.Message;
                        }
                        else if (string.IsNullOrEmpty(last.Source))
                        {
                            last.Source = message.Message;
                        }
                        else if (string.IsNullOrEmpty(last.StackTrace))
                        {
                            last.StackTrace = message.Message;
                        }
                    }

                    line = sr.ReadLine();
                }

                sr.Close();
            }
        }
        #endregion
    }
}   //ImprezGarage.Infrastructure.Model.LogDataModel namespace 