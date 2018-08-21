//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Settings.DataModel
{
    using Prism.Mvvm;
    using System.Xml.Serialization;

    [XmlRoot("Configuration")]
    public class Configuration : BindableBase
    {
        #region Attributes
        [XmlIgnore]
        private bool _launchOnStartUp;
        [XmlIgnore]
        private bool _minimizeOnLoad;
        [XmlIgnore]
        private bool _minimizeToTry;
        [XmlIgnore]
        private bool _notifyWhenVehicleTaxRenewalIsClose;
        [XmlIgnore]
        private bool _notifyWhenInsuranceRenewalIsClose;
        [XmlIgnore]
        private bool _allowNotifications;
        #endregion

        #region Properties
        [XmlElement]
        public bool LaunchOnStartUp
        {
            get => _launchOnStartUp;
            set => SetProperty(ref _launchOnStartUp, value);
        }

        [XmlElement]
        public bool MinimizeOnLoad
        {
            get => _minimizeOnLoad;
            set => SetProperty(ref _minimizeOnLoad, value);
        }

        [XmlElement]
        public bool MinimizeToTry
        {
            get => _minimizeToTry;
            set => SetProperty(ref _minimizeToTry, value);
        }

        [XmlElement]
        public bool NotifyWhenVehicleTaxRenewalIsClose
        {
            get => _notifyWhenVehicleTaxRenewalIsClose;
            set => SetProperty(ref _notifyWhenVehicleTaxRenewalIsClose, value);
        }

        [XmlElement]
        public bool NotifyWhenInsuranceRenewalIsClose
        {
            get => _notifyWhenInsuranceRenewalIsClose;
            set => SetProperty(ref _notifyWhenInsuranceRenewalIsClose, value);
        }

        [XmlElement]
        public bool AllowNotifications
        {
            get => _allowNotifications;
            set => SetProperty(ref _allowNotifications, value);
        }
        #endregion
    }
}   //ImprezGarage.Modules.Settings.DataModel namespace 