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
        private readonly ISettingsService _settingsService;
        #endregion

        #region Properties
        public bool LaunchOnStartUp
        {
            get => _settingsService.GetLaunchOnStartUp();
            set
            {
                _settingsService.SetLaunchOnStartUp(value);
                RegisterInStartup(value);
            }
        }

        public bool MinimizeOnLoad
        {
            get => _settingsService.GetMinimizeOnLoad();
            set => _settingsService.SetMinimizeOnLoad(value);
        }

        public bool MinimizeToTry
        {
            get => _settingsService.GetMinimizeToTry();
            set => _settingsService.SetMinimizeToTry(value);
        }

        public bool NotifyWhenVehicleTaxRenewalIsClose
        {
            get => _settingsService.GetNotifyWhenVehicleTaxRenewalIsClose();
            set => _settingsService.SetNotifyWhenVehicleTaxRenewalIsClose(value);
        }

        public bool NotifyWhenInsuranceRenewalIsClose
        {
            get => _settingsService.GetNotifyWhenInsuranceRenewalIsClose();
            set => _settingsService.SetNotifyWhenInsuranceRenewalIsClose(value);
        }

        public bool AllowNotifications
        {
            get => _settingsService.GetAllowNotifications();
            set => _settingsService.SetAllowNotifications(value);
        }
        #endregion

        #region Methods
        public MainViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

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