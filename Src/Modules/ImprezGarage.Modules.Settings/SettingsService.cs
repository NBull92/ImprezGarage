//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Settings
{
    using ImprezGarage.Modules.Settings.DataModel;
    using Infrastructure.Services;
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class SettingsService : ISettingsService
    {
        #region Attribute
        private string _configurationLocation;
        private Configuration _configuration;
        #endregion

        #region Methods
        public SettingsService()
        {
            _configurationLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ImprezGarage\\Config.xml";           
        }
        
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

        public void PrintConfigurationFile()
        {
            File.Create(_configurationLocation).Dispose();
            var xmlWriterSettings = new XmlWriterSettings() { Indent = true };
            using (var writer = XmlWriter.Create(_configurationLocation, xmlWriterSettings))
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                serializer.Serialize(writer, _configuration);
            }
        }

        public bool GetLaunchOnStartUp()
        {
            return _configuration.LaunchOnStartUp;
        }

        public bool GetMinimizeOnLoad()
        {
            return _configuration.MinimizeOnLoad;
        }

        public bool GetMinimizeToTry()
        {
            return _configuration.MinimizeToTry;
        }

        public bool GetNotifyWhenVehicleTaxRenewalIsClose()
        {
            return _configuration.NotifyWhenVehicleTaxRenewalIsClose;
        }

        public bool GetNotifyWhenInsuranceRenewalIsClose()
        {
            return _configuration.NotifyWhenInsuranceRenewalIsClose;
        }

        public bool GetAllowNotifications()
        {
            return _configuration.AllowNotifications;
        }

        public void SetLaunchOnStartUp(bool value)
        {
            _configuration.LaunchOnStartUp = value;
        }

        public void SetAllowNotifications(bool value)
        {
            _configuration.AllowNotifications = value;
        }

        public void SetMinimizeOnLoad(bool value)
        {
            _configuration.MinimizeOnLoad = value;
        }

        public void SetNotifyWhenInsuranceRenewalIsClose(bool value)
        {
            _configuration.NotifyWhenInsuranceRenewalIsClose = value;
        }

        public void SetNotifyWhenVehicleTaxRenewalIsClose(bool value)
        {
            _configuration.NotifyWhenVehicleTaxRenewalIsClose = value;
        }

        public void SetMinimizeToTry(bool value)
        {
            _configuration.MinimizeToTry = value;
        }
        #endregion
    }
}   //ImprezGarage.Modules.Settings namespace 