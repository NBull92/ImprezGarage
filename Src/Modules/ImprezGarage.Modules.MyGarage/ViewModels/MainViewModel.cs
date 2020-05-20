//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using Infrastructure;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using MahApps.Metro.Controls;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
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
        private readonly INotificationsService _notificationsService;
        private readonly ILoggerService _loggerService;
        private readonly IVehicleService _vehicleService;
        private string _userId;
        #endregion

        #region Properties
        private ObservableCollection<VehicleViewModel> _vehicles = new ObservableCollection<VehicleViewModel>();
        public ObservableCollection<VehicleViewModel> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        private VehicleViewModel _selectedVehicle;
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
        public MainViewModel(IDataService dataService, IEventAggregator eventAggregator, 
            INotificationsService notificationsService, ILoggerService loggerService, IVehicleService vehicleService)
        {
            _dataService = dataService;
            _notificationsService = notificationsService;
            _loggerService = loggerService;
            _vehicleService = vehicleService;

            AddNewVehicleCommand = new DelegateCommand(AddNewVehicleExecute);
            SelectedVehicleChanged = new DelegateCommand<SelectionChangedEventArgs>(SelectedVehicleChangedExecute);

            eventAggregator.GetEvent<Events.RefreshDataEvent>().Subscribe(OnRefresh);
            eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);
        }

        #region Command Handlers
        private void SelectedVehicleChangedExecute(SelectionChangedEventArgs obj)
        {
            if (SelectedVehicle != null)
            {
                _vehicleService.RaiseSelectedVehicleChanged(SelectedVehicle.Vehicle);
            }
            else
            {
                _vehicleService.ClearSelectedVehicle();
            }
        }

        /// <summary>
        /// Open a new window to allow the user to create a new vehicle.
        /// </summary>
        private void AddNewVehicleExecute()
        {
            var vehicleControl = new ManageVehicle();
            var window = new MetroWindow
            {
                Height = 350,
                Width = 305,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = vehicleControl
            };

            if (vehicleControl.DataContext is ManageVehicleViewModel model)
            {
                model.CloseRequest += (sender, e) => window.Close();
                window.Closed += (s, a) =>
                {
                    if (model.DialogResult)
                        LoadVehicles();
                };
            }
            window.ShowDialog();
        }
        #endregion

        #region Event Handlers
        private void OnUserAccountChange(Tuple<bool, Account> loginData)
        {
            if (loginData.Item1)
            {
                _userId = loginData.Item2.UserId;
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

            if (SelectedVehicle != null)
            {
                _vehicleService.RaiseSelectedVehicleChanged(SelectedVehicle.Vehicle);
            }
            else
            {
                _vehicleService.ClearSelectedVehicle();
            }
        }
        #endregion

        /// <summary>
        /// Retrieves all of the vehicles saved to the database and create a view model for each one.
        /// </summary>
        public async void LoadVehicles()
        {
            Vehicles.Clear();

            var vehicles = await _dataService.GetUserVehicles(_userId, true);

            if (vehicles == null)
                return;

            try
            {
                await _dataService.GetVehicleTypesAsync().ContinueWith(task =>
                {
                    foreach (var vehicle in vehicles)
                    {
                        var viewModel = new VehicleViewModel(_dataService, _notificationsService);
                        viewModel.LoadInstance(vehicle);
                        Application.Current.Dispatcher?.Invoke(() =>
                        {
                            Vehicles.Add(viewModel);
                        });
                    }

                    SelectedVehicle = Vehicles.FirstOrDefault();
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
            }
        }
        #endregion
    }
}   //ImprezGarage.Modules.Dashboard.ViewModels namespace 