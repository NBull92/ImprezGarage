﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using Infrastructure;
    using Infrastructure.Services;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.ServiceLocation;
    using Modules.MyGarage.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public class MainViewModel : BindableBase
    {
        #region Attributes
        private const int _daysAllowanceBeforeReminder = 30;
        private readonly IDataService _dataService;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationsService _notificationsService;
        private ObservableCollection<VehicleViewModel> _vehicles;
        private VehicleViewModel _selectedVehicle;
        private List<string> _reminders;
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
        public DelegateCommand<object> AddNewVehicleCommand { get; set; }
        public DelegateCommand<SelectionChangedEventArgs> SelectedVehicleChanged { get; set; }
        #endregion
        #endregion
        
        #region Methods
        public MainViewModel(IDataService dataService, IRegionManager regionManager, IEventAggregator eventAggregator, INotificationsService notificationsService)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _notificationsService = notificationsService;

            AddNewVehicleCommand = new DelegateCommand<object>(AddNewVehicleExecute);
            SelectedVehicleChanged = new DelegateCommand<SelectionChangedEventArgs>(SelectedVehicleChangedExecute);

            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Subscribe(OnRefresh);
            _eventAggregator.GetEvent<Events.EditVehicleEvent>().Subscribe(OnEditVehicle);

            _reminders = new List<string>();

            LoadVehicles();
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
        private void AddNewVehicleExecute(object navigationPath)
        {
            var view = new AddVehicle();
            var viewModel = view.DataContext as AddVehicleViewModel;
            view.ShowDialog();

            if (viewModel.DialogResult)
            {
                LoadVehicles();
            }            
        }
        #endregion

        #region Event Handlers
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
            var view = new AddVehicle();
            var viewModel = view.DataContext as AddVehicleViewModel;
            viewModel.IsEdit = true;
            viewModel.Edit(vehicle.Vehicle);
            view.ShowDialog();
            OnRefresh();
        }
        #endregion

        /// <summary>
        /// Retrieves all of the vehicles saved to the database and create a view model for each one.
        /// </summary>
        public void LoadVehicles()
        {
            Vehicles = new ObservableCollection<VehicleViewModel>();

            var vehicles = _dataService.GetVehicles(true);

            if (vehicles == null || vehicles.Result == null)
                return;

            //TODO - do the query.Subscribe bit here Reactive
            foreach (var vehicle in vehicles.Result)
            {
                var viewModel = ServiceLocator.Current.GetInstance<VehicleViewModel>();
                viewModel.LoadInstance(vehicle);
                Vehicles.Add(viewModel);
            }

            CheckForVehicleReminders();
        }

        /// <summary>
        /// Go through each vehicle and check for tax expiry dates, insurance renewal dates, MOT dates, service dates. Last time performed a maintainence check etc.
        /// </summary>
        private void CheckForVehicleReminders()
        {
            foreach (var vehicle in Vehicles)
            {
                //Check vehicle type
                switch(vehicle.VehicleType.Id)
                {
                        // Motorbike
                    case 2:
                        CheckLastMaintainenceCheck(vehicle);
                        CheckInsuranceRenewalDate(vehicle);
                        CheckTaxDate(vehicle);
                        // CheckLastServiceDate(vehicle);
                        break;
                    // Bicycle
                    case 3:
                        //CheckLastMaintainenceCheck(vehicle);
                        // GetLastServiceDate(vehicle);
                        break;
                    // Car
                    default:
                        CheckLastMaintainenceCheck(vehicle);
                        CheckInsuranceRenewalDate(vehicle);
                        CheckTaxDate(vehicle);
                        //CheckLastServiceDate(vehicle);
                        break;
                }
            }

            // Go through all of the reminders and create toast notifications to inform the user.
            foreach(var reminder in _reminders)
            {
                _notificationsService.Toast(reminder);
            }
        }

        /// <summary>
        /// Check the tax expiry date of the passed through vehicle and if it runs out in less than 30 days, inform the user.
        /// </summary>
        private void CheckTaxDate(VehicleViewModel vehicle)
        {
            if (vehicle.Vehicle.HasValidTax == false)
                return;

               var taxDate = Convert.ToDateTime(vehicle.TaxExpiryDate);
            if ((taxDate - DateTime.Now).TotalDays < _daysAllowanceBeforeReminder)
            {
                _reminders.Add("The tax of vehicle:  " + vehicle.Registration + " runs out on the : " + taxDate.ToShortDateString());
            }
        }

        /// <summary>
        /// Check the insurance renewal date of the passed through vehicle and if it runs out in less than 30 days, inform the user.
        /// </summary>
        private void CheckInsuranceRenewalDate(VehicleViewModel vehicle)
        {
            if (vehicle.Vehicle.HasInsurance == false)
                return;

            var insuranceRenewalDate = Convert.ToDateTime(vehicle.InsuranceRenewalDate);
            if ((insuranceRenewalDate - DateTime.Now).TotalDays < _daysAllowanceBeforeReminder)
            {
                _reminders.Add("The insurance of vehicle:  " + vehicle.Registration + " runs out on the : " + insuranceRenewalDate.ToShortDateString());
            }
        }

        /// <summary>
        /// Find the last maintainence check performed on the passed through vehicle and if it is more than 30 days old, then add this to the reminders;
        /// </summary>
        private void CheckLastMaintainenceCheck(VehicleViewModel vehicle)
        {
            var lastDate = _dataService.GetLastMaintenanceCheckDateForVehicleByVehicleId(vehicle.Vehicle.Id);

            if (lastDate == null || lastDate.Result == null)
                return;

            var date = Convert.ToDateTime(lastDate.Result);
            if ((date - DateTime.Now).TotalDays < _daysAllowanceBeforeReminder)
            {
                _reminders.Add("A maintainence check was last performed on vehicle " + vehicle.Registration + " on: " + date.ToShortDateString());
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

            _regionManager.RequestNavigate(RegionNames.PetrolRegion, typeof(PetrolExpenditure.Views.Main).FullName + parameters);
        }
        #endregion
    }
}   //ImprezGarage.Modules.Dashboard.ViewModels namespace 