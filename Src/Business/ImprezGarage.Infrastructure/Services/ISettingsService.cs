//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    public interface ISettingsService
    {
        void LoadConfigurationFile();
        void PrintConfigurationFile();
        bool GetLaunchOnStartUp();
        bool GetMinimizeOnLoad();
        bool GetMinimizeToTry();
        bool GetNotifyWhenVehicleTaxRenewalIsClose();
        bool GetNotifyWhenInsuranceRenewalIsClose();
        bool GetAllowNotifications();

        void SetLaunchOnStartUp(bool value);
        void SetAllowNotifications(bool value);
        void SetMinimizeOnLoad(bool value);
        void SetNotifyWhenInsuranceRenewalIsClose(bool value);
        void SetNotifyWhenVehicleTaxRenewalIsClose(bool value);
        void SetMinimizeToTry(bool value);
    }
}   //ImprezGarage.Infrastructure.Services namespace 