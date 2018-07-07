//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Settings.DataModel
{
    using System.Xml.Serialization;

    [XmlRoot("Configuration")]
    public class Configuration
    {
        [XmlElement]
        public bool LaunchOnStartUp { get; set; }

        [XmlElement]
        public bool MinimizeOnLoad { get; set; }

        [XmlElement]
        public bool MinimizeToTry { get; set; }

        [XmlElement]
        public bool NotifyWhenVehicleTaxRenewalIsClose { get; set; }
    }
}   //ImprezGarage.Modules.Settings.DataModel namespace 