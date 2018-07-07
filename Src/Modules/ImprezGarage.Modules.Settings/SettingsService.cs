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
        #endregion

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
                };
            }
        }

        public void PrintConfigurationFile()
        {
            File.Create(_configurationLocation).Dispose();
            
            using (var writer = XmlWriter.Create(_configurationLocation))
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                serializer.Serialize(writer, _configuration);
            }
        }
    }
}   //ImprezGarage.Modules.Settings namespace 