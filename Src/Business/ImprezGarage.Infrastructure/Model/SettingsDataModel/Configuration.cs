//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model.SettingsDataModel
{
    using System.Xml.Serialization;
    using Prism.Mvvm;

    [XmlRoot("Configuration")]
    public class Configuration : BindableBase
    {
        #region Attributes
        /// <summary>
        /// Store whether the user wants the app to launch on startup.
        /// </summary>
        [XmlIgnore]
        private bool _launchOnStartUp;

        /// <summary>
        /// Store whether the user wants minimize the app when it loads.
        /// </summary>
        [XmlIgnore]
        private bool _minimizeOnLoad;

        /// <summary>
        /// Store whether the user wants minimize the app to the tray.
        /// </summary>
        [XmlIgnore]
        private bool _minimizeToTry;

        /// <summary>
        /// Store whether the user wants to be notified of a tax renewal date.
        /// </summary>
        [XmlIgnore]
        private bool _notifyWhenVehicleTaxRenewalIsClose;

        /// <summary>
        /// Store whether the user wants to be notified of a insurance renewal date.
        /// </summary>
        [XmlIgnore]
        private bool _notifyWhenInsuranceRenewalIsClose;

        /// <summary>
        /// Store whether the user wants to be notified at all.
        /// </summary>
        [XmlIgnore]
        private bool _allowNotifications;
        #endregion

        #region Properties
        /// <summary>
        /// Store whether the user wants the app to launch on startup.
        /// </summary>
        [XmlElement]
        public bool LaunchOnStartUp
        {
            get => _launchOnStartUp;
            set => SetProperty(ref _launchOnStartUp, value);
        }

        /// <summary>
        /// Store whether the user wants minimize the app when it loads.
        /// </summary>
        [XmlElement]
        public bool MinimizeOnLoad
        {
            get => _minimizeOnLoad;
            set => SetProperty(ref _minimizeOnLoad, value);
        }

        /// <summary>
        /// Store whether the user wants minimize the app to the tray.
        /// </summary>
        [XmlElement]
        public bool MinimizeToTry
        {
            get => _minimizeToTry;
            set => SetProperty(ref _minimizeToTry, value);
        }

        /// <summary>
        /// Store whether the user wants to be notified of a tax renewal date.
        /// </summary>
        [XmlElement]
        public bool NotifyWhenVehicleTaxRenewalIsClose
        {
            get => _notifyWhenVehicleTaxRenewalIsClose;
            set => SetProperty(ref _notifyWhenVehicleTaxRenewalIsClose, value);
        }

        /// <summary>
        /// Store whether the user wants to be notified of a insurance renewal date.
        /// </summary>
        [XmlElement]
        public bool NotifyWhenInsuranceRenewalIsClose
        {
            get => _notifyWhenInsuranceRenewalIsClose;
            set => SetProperty(ref _notifyWhenInsuranceRenewalIsClose, value);
        }

        /// <summary>
        /// Store whether the user wants to be notified at all.
        /// </summary>
        [XmlElement]
        public bool AllowNotifications
        {
            get => _allowNotifications;
            set => SetProperty(ref _allowNotifications, value);
        }
        #endregion
    }
}   //ImprezGarage.Modules.Settings.DataModel namespace 