
namespace ImprezGarage.Modules.Account.ViewModels
{
    using CountriesWrapper;
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using MyGarage.Views;
    using PetrolExpenditure.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;


    public class ProfilePageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDataService _dataService;
        private readonly ILoggerService _loggerService;
        private readonly ICountryManager _countryManager;

        private bool _nameAllowSave;
        private bool _countryAllowSave;
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
                _nameAllowSave = !string.IsNullOrWhiteSpace(_name) && !_name.Equals(_currentUser.Name);
                Validate();
            }
        }

        private Country _country;
        public Country Country
        {
            get => _country;
            set
            {
                SetProperty(ref _country, value);
                _countryAllowSave = Country != null && !_country.Name.Equals(_currentUser.Country);
                Validate();
            }
        }

        private ObservableCollection<Country> _countries;
        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
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
            IDataService dataService, ILoggerService loggerService, ICountryManager countryManager)
        {
            _regionManager = regionManager;
            _authenticationService = authenticationService;
            _dataService = dataService;
            _loggerService = loggerService;
            _countryManager = countryManager;

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
                _currentUser.Country = Country.Name;
                _dataService.UpdateUser(_currentUser);
                _nameAllowSave = false;
                _countryAllowSave = false;
                CanSave = false;
            }
            catch (Exception e)
            {
                _loggerService.LogException(e, "An error occured during the updating of a user.");
            }
        }

        private void Validate()
        {
            if (_countryAllowSave || _nameAllowSave)
            {
                CanSave = true;
            }
            else
            {
                CanSave = false;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                _currentUser = _authenticationService.CurrentUser();
                Name = _currentUser.Name;
                IsReadonly = _currentUser.IsReadonly;
                var countries = _countryManager.GetCountries();
                if (countries == null)
                    return;

                Countries = new ObservableCollection<Country>(countries);
                Country = Countries.FirstOrDefault(o => o.Name.Equals(_currentUser.Country));
            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
            }
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
