//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger.Configuration
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// This class is for holding the settings for the logger module.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "LogsConfiguration")]
    public class LogsConfiguration
    {
        #region Properties
        /// <summary>
        /// Store the users selected level of log details.
        /// </summary>
        [XmlElement("LogDetail")]
        public string LogDetail { get; set; }

        /// <summary>
        /// Store the file type the user has chosen to store the logs as.
        /// </summary>
        [XmlElement("LogFileType")]
        public string LogFileType { get; set; }

        /// <summary>
        /// Store how long the user would like to keep the logs stroed for.
        /// </summary>
        [XmlElement("LogLife")]
        public int LogLife { get; set; }
        #endregion
    }
}   //ImprezGarage.Modules.Logger.Configuration namespace 