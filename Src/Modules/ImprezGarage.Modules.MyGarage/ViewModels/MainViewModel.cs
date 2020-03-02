//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using Infrastructure;
    using Infrastructure.Services;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Views;

    public class MainViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationsService _notificationsService;
        private readonly ILoggerService _loggerService;
        private ObservableCollection<VehicleViewModel> _vehicles = new ObservableCollection<VehicleViewModel>();
        private VehicleViewModel _selectedVehicle;
        private string _userId;
        #endregion

        #region Properties
        public ObservableCollection<VehicleViewModel> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }
        

        #region Commands
        public DelegateCommand AddNewVehicleCommand { get; set; }
        public DelegateCommand<SelectionChangedEventArgs> SelectedVehicleChanged { get; set; }
        #endregion
        #endregion
        
        #region Methods
        public MainViewModel(IDataService dataService, IRegionManager regionManager, IEventAggregator eventAggregator, 
            INotificationsService notificationsService, ILoggerService loggerService)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _notificationsService = notificationsService;
            _loggerService = loggerService;

            AddNewVehicleCommand = new DelegateCommand(AddNewVehicleExecute);
            SelectedVehicleChanged = new DelegateCommand<SelectionChangedEventArgs>(SelectedVehicleChangedExecute);

            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Subscribe(OnRefresh);
            _eventAggregator.GetEvent<Events.EditVehicleEvent>().Subscribe(OnEditVehicle);
            _eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);
        }

        #region Command Handlers
        private void SelectedVehicleChangedExecute(SelectionChangedEventArgs obj)
        {
            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Publish(SelectedVehicle);
            RequestNavigationToPetrolExpenditureMainView();
        }

        /// <summary>
        /// Open a new window to allow the user to create a new vehicle.
        /// </summary>
        private void AddNewVehicleExecute()
        {
            var unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            var model = new AddVehicleViewModel(_dataService, unityContainer, _notificationsService, _loggerService);

            var view = new AddVehicle(model);
            view.ShowDialog();

            if (model.DialogResult)
                LoadVehicles();
        }
        #endregion

        #region Event Handlers
        private void OnUserAccountChange(Tuple<bool,string> loginData)
        {
            if (loginData.Item1)
            {
                _userId = loginData.Item2;
                OnRefresh();
            }
            else
            {
                _userId = string.Empty;
                SelectedVehicle = null;
                Vehicles.Clear();
            }
        }

        private void OnRefresh()
        {
            var currentlySelectedVehicle = SelectedVehicle;
            SelectedVehicle = null;
            LoadVehicles();

            if (currentlySelectedVehicle != null && Vehicles.Any(o => o.Vehicle.Id == currentlySelectedVehicle.Vehicle.Id))
            {
                SelectedVehicle = Vehicles.First(o => o.Vehicle.Id == currentlySelectedVehicle.Vehicle.Id);
            }

            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Publish(SelectedVehicle);
        }

        private void OnEditVehicle(VehicleViewModel vehicle)
        {
            var unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            var model = new AddVehicleViewModel(_dataService, unityContainer, _notificationsService, _loggerService, vehicle);
            var view = new AddVehicle(model);
            view.ShowDialog();

            if (model.DialogResult)
                OnRefresh();
        }
        #endregion

        /// <summary>
        /// Retrieves all of the vehicles saved to the database and create a view model for each one.
        /// </summary>
        public async void LoadVehicles()
        {
            Vehicles.Clear();;

            var vehicles = await _dataService.GetVehicles(true);

            if (vehicles == null)
                return;

            try
            {
                await _dataService.GetVehicleTypesAsync().ContinueWith(task =>
                {
                    //TODO - do the query.Subscribe bit here Reactive
                    foreach (var vehicle in vehicles.Where(o => o.UserId == _userId))
                    {
                        var viewModel = new VehicleViewModel(_dataService, _notificationsService);
                        viewModel.LoadInstance(vehicle);
                        Application.Current.Dispatcher?.Invoke(() =>
                        {
                            Vehicles.Add(viewModel);
                        });
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
            }
        }

        /// <summary>
        /// This function will request that the main view of the petrol expenditure module is navigated to in the 'PetrolRegion'.
        /// It will also pass through the selected vehicle to that view also.
        /// </summary>
        private void RequestNavigationToPetrolExpenditureMainView()
        {
            //check if vehicle was not selected and exit this function.
            if (SelectedVehicle == null)
                return;

            //create navigation parameters using the selected vehicle.
            var parameters = new NavigationParameters
            {
                { "SelectedVehicle", SelectedVehicle}
            };

            _regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(PetrolExpenditure.Views.Main).FullName + parameters);
        }
        #endregion
    }
}   //ImprezGarage.Modules.Dashboard.ViewModels namespace 