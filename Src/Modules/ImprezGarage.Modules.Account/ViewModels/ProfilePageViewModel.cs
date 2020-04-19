
using System.Configuration;

namespace ImprezGarage.Modules.Account.ViewModels
{
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using MyGarage.Views;
    using PetrolExpenditure.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;

    public class ProfilePageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDataService _dataService;
        private readonly ILoggerService _loggerService;

        private Account _currentUser;

        private bool _canSave;
        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                if (string.IsNullOrWhiteSpace(_name) || _name.Equals(_currentUser.Name))
                {
                    CanSave = false;
                }
                else
                {
                    CanSave = true;
                }
            }
        }

        private bool _isReadonly;
        public bool IsReadonly
        {
            get => _isReadonly;
            set => SetProperty(ref _isReadonly, value);
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }

        public ProfilePageViewModel(IRegionManager regionManager, IAuthenticationService authenticationService, 
            IDataService dataService, ILoggerService loggerService)
        {
            _regionManager = regionManager;
            _authenticationService = authenticationService;
            _dataService = dataService;
            _loggerService = loggerService;

            SaveCommand = new DelegateCommand(OnSave).ObservesCanExecute(() => CanSave);
            BackCommand = new DelegateCommand(OnBack);
        }

        private void OnBack()
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(Main).FullName);
            _regionManager.RequestNavigate(RegionNames.VehicleHeaderRegion, typeof(VehicleHeader).FullName);
        }

        private void OnSave()
        {
            try
            {
                _currentUser.Name = Name;
                _dataService.UpdateUser(_currentUser);
                CanSave = false;
            }
            catch (Exception e)
            {
                _loggerService.LogException(e, "An error occured during the updating of a user.");
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _currentUser = _authenticationService.CurrentUser();
            Name = _currentUser.Name;
            IsReadonly = _currentUser.IsReadonly;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
