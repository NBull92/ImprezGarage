﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Settings
{
    using Infrastructure.Model.SettingsDataModel;
    using Infrastructure.Services;
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class SettingsService : ISettingsService
    {
        #region Attribute
        /// <summary>
        /// Store the location of the settings file.
        /// </summary>
        private readonly string _configurationLocation;

        /// <summary>
        /// Store the current settings.
        /// </summary>
        private Configuration _configuration;
        #endregion

        #region Methods
        /// <summary>
        /// Construct the settings service and store the location of the settings file.
        /// </summary>
        public SettingsService()
        {
            _configurationLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ImprezGarage\\Config.xml";           
        }
        
        /// <summary>
        /// Check to see if the settings file already exists, then load and store the data.
        /// If not create a new configuration of settings.
        /// </summary>
        public void LoadConfigurationFile()
        {
            if(File.Exists(_configurationLocation))
            {
                using (var writer = XmlReader.Create(_configurationLocation))
                {
                    var serializer = new XmlSerializer(typeof(Configuration));
                    _configuration = serializer.Deserialize(writer) as Configuration;
                }
            }
            else
            {
                _configuration = new Configuration
                {
                    LaunchOnStartUp = false,
                    MinimizeOnLoad = false,
                    MinimizeToTry = false,
                    NotifyWhenVehicleTaxRenewalIsClose = true,
                    NotifyWhenInsuranceRenewalIsClose = true,
                    AllowNotifications = true,
                };
            }
        }

        /// <summary>
        /// Take the current settings configuration file and save it to the app data file location.
        /// </summary>
        public void PrintConfigurationFile()
        {
            File.Create(_configurationLocation).Dispose();
            var xmlWriterSettings = new XmlWriterSettings { Indent = true };
            using (var writer = XmlWriter.Create(_configurationLocation, xmlWriterSettings))
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                serializer.Serialize(writer, _configuration);
            }
        }

        /// <summary>
        /// Return whether or not the user wants the app to load on startup.
        /// </summary>
        public bool GetLaunchOnStartUp()
        {
            return _configuration.LaunchOnStartUp;
        }
        
        /// <summary>
        /// Return whether or not the user wants the app minimize on load.
        /// </summary>
        public bool GetMinimizeOnLoad()
        {
            return _configuration.MinimizeOnLoad;
        }

        /// <summary>
        /// Return whether or not the user wants the app minimize to tray and not close instead.
        /// </summary>
        public bool GetMinimizeToTry()
        {
            return _configuration.MinimizeToTry;
        }

        /// <summary>
        /// Return whether or not the user wants be notified of the tax renewal date if a vehicle is getting close to it.
        /// </summary>
        public bool GetNotifyWhenVehicleTaxRenewalIsClose()
        {
            return _configuration.NotifyWhenVehicleTaxRenewalIsClose;
        }

        /// <summary>
        /// Return whether or not the user wants be notified of the insurance date if a vehicle is getting close to it.
        /// </summary>
        public bool GetNotifyWhenInsuranceRenewalIsClose()
        {
            return _configuration.NotifyWhenInsuranceRenewalIsClose;
        }

        /// <summary>
        /// Return whether or not the user wants be notified of anything.
        /// </summary>
        public bool GetAllowNotifications()
        {
            return _configuration.AllowNotifications;
        }

        /// <summary>
        /// Set the users choice of launch on start up.
        /// </summary>
        public void SetLaunchOnStartUp(bool value)
        {
            _configuration.LaunchOnStartUp = value;
        }

        /// <summary>
        /// Set the users choice of allowing notifications.
        /// </summary>
        public void SetAllowNotifications(bool value)
        {
            _configuration.AllowNotifications = value;
        }

        /// <summary>
        /// Set the users choice of minimizing the app on load.
        /// </summary>
        public void SetMinimizeOnLoad(bool value)
        {
            _configuration.MinimizeOnLoad = value;
        }

        /// <summary>
        /// Set the users choice of notifying about insurance.
        /// </summary>
        public void SetNotifyWhenInsuranceRenewalIsClose(bool value)
        {
            _configuration.NotifyWhenInsuranceRenewalIsClose = value;
        }

        /// <summary>
        /// Set the users choice of notifying about tax.
        /// </summary>
        public void SetNotifyWhenVehicleTaxRenewalIsClose(bool value)
        {
            _configuration.NotifyWhenVehicleTaxRenewalIsClose = value;
        }

        /// <summary>
        /// Set the users choice of minimizing the app to the tray.
        /// </summary>
        public void SetMinimizeToTry(bool value)
        {
            _configuration.MinimizeToTry = value;
        }
        #endregion
    }
}   //ImprezGarage.Modules.Settings namespace 