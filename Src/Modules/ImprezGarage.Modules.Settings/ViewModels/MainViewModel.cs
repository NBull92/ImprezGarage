//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Settings.ViewModels
{
    using Infrastructure.Services;
    using Microsoft.Win32;
    using Prism.Mvvm;
    using System.Reflection;

    public class MainViewModel : BindableBase
    {
        #region Attributes
        /// <summary>
        /// Store the injected setting service.
        /// </summary>
        private readonly ISettingsService _settingsService;
        #endregion

        #region Properties
        /// <summary>
        /// Store whether the user wants the app to launch on startup.
        /// Then register the app in the startup menu.
        /// </summary>
        public bool LaunchOnStartUp
        {
            get => _settingsService.GetLaunchOnStartUp();
            set
            {
                _settingsService.SetLaunchOnStartUp(value);
                RegisterInStartup(value);
            }
        }

        /// <summary>
        /// Store whether the user wants minimize the app when it loads.
        /// </summary>
        public bool MinimizeOnLoad
        {
            get => _settingsService.GetMinimizeOnLoad();
            set => _settingsService.SetMinimizeOnLoad(value);
        }

        /// <summary>
        /// Store whether the user wants minimize the app to the tray.
        /// </summary>
        public bool MinimizeToTry
        {
            get => _settingsService.GetMinimizeToTry();
            set => _settingsService.SetMinimizeToTry(value);
        }

        /// <summary>
        /// Store whether the user wants to be notified of a tax renewal date.
        /// </summary>
        public bool NotifyWhenVehicleTaxRenewalIsClose
        {
            get => _settingsService.GetNotifyWhenVehicleTaxRenewalIsClose();
            set => _settingsService.SetNotifyWhenVehicleTaxRenewalIsClose(value);
        }

        /// <summary>
        /// Store whether the user wants to be notified of a insurance renewal date.
        /// </summary>
        public bool NotifyWhenInsuranceRenewalIsClose
        {
            get => _settingsService.GetNotifyWhenInsuranceRenewalIsClose();
            set => _settingsService.SetNotifyWhenInsuranceRenewalIsClose(value);
        }

        /// <summary>
        /// Store whether the user wants to be notified at all.
        /// </summary>
        public bool AllowNotifications
        {
            get => _settingsService.GetAllowNotifications();
            set => _settingsService.SetAllowNotifications(value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Comstruct the view model and inject the settings service.
        /// </summary>
        /// <param name="settingsService"></param>
        public MainViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Register the app in the startup menu or remove it.
        /// </summary>
        private void RegisterInStartup(bool isChecked)
        {
            var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if(registryKey == null)
                return;

            if (isChecked)
            {
                registryKey.SetValue("ImprezGarage", Assembly.GetEntryAssembly().Location);
            }
            else
            {
                registryKey.DeleteValue("ImprezGarage");
            }
        }
        #endregion
    }
}// ImprezGarage.Modules.Settings.ViewModels namespace 