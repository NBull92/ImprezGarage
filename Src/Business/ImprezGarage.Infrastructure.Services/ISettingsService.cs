//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    public interface ISettingsService
    {
        #region Methods
        /// <summary>
        /// Check to see if the settings file already exists, then load and store the data.
        /// If not create a new configuration of settings.
        /// </summary>
        void LoadConfigurationFile();

        /// <summary>
        /// Take the current settings configuration file and save it to the app data file location.
        /// </summary>
        void SaveConfigurationFile();

        /// <summary>
        /// Return whether or not the user wants the app to load on startup.
        /// </summary>
        bool GetLaunchOnStartUp();

        /// <summary>
        /// Return whether or not the user wants the app minimize on load.
        /// </summary>
        bool GetMinimizeOnLoad();

        /// <summary>
        /// Return whether or not the user wants the app minimize to tray and not close instead.
        /// </summary>
        bool GetMinimizeToTry();

        /// <summary>
        /// Return whether or not the user wants be notified of the tax renewal date if a vehicle is getting close to it.
        /// </summary>
        bool GetNotifyWhenVehicleTaxRenewalIsClose();

        /// <summary>
        /// Return whether or not the user wants be notified of the insurance date if a vehicle is getting close to it.
        /// </summary>
        bool GetNotifyWhenInsuranceRenewalIsClose();

        /// <summary>
        /// Return whether or not the user wants be notified of anything.
        /// </summary>
        bool GetAllowNotifications();

        /// <summary>
        /// Set the users choice of launch on start up.
        /// </summary>
        void SetLaunchOnStartUp(bool value);

        /// <summary>
        /// Set the users choice of allowing notifications.
        /// </summary>
        void SetAllowNotifications(bool value);

        /// <summary>
        /// Set the users choice of minimizing the app on load.
        /// </summary>
        void SetMinimizeOnLoad(bool value);

        /// <summary>
        /// Set the users choice of notifying about insurance.
        /// </summary>
        void SetNotifyWhenInsuranceRenewalIsClose(bool value);

        /// <summary>
        /// Set the users choice of notifying about tax.
        /// </summary>
        void SetNotifyWhenVehicleTaxRenewalIsClose(bool value);

        /// <summary>
        /// Set the users choice of minimizing the app to the tray.
        /// </summary>
        void SetMinimizeToTry(bool value);
        #endregion
    }
}   //ImprezGarage.Infrastructure.Services namespace 