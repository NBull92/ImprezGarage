//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using ImprezGarage.Modules.Settings.DataModel;

    public interface ISettingsService
    {
        void LoadConfigurationFile();
        void PrintConfigurationFile();
        Configuration GetConfiguration();
    }
}   //ImprezGarage.Infrastructure.Services namespace 