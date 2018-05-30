//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Dialogs;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Modules.MyGarage.Views;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;

    public class MainViewModel : BindableBase
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<VehicleViewModel> _vehicles;
        public ObservableCollection<VehicleViewModel> Vehicles
        {
            get => _vehicles;
            set
            {
                SetProperty(ref _vehicles, value);
            }
        }

        private VehicleViewModel _selectedVehicle;
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                SetProperty(ref _selectedVehicle, value);
            }
        }

        public InteractionRequest<INotification> NotificationRequest { get; set; }

        public DelegateCommand<object> AddNewVehicleCommand { get; set; }
        public DelegateCommand<SelectionChangedEventArgs> SelectedVehicleChanged { get; set; }

        public MainViewModel(IDataService dataService, IDialogService dialogService, IRegionManager regionManager
            , IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            NotificationRequest = new InteractionRequest<INotification>();

            AddNewVehicleCommand = new DelegateCommand<object>(AddNewVehicleExecute);
            SelectedVehicleChanged = new DelegateCommand<SelectionChangedEventArgs>(SelectedVehicleChangedExecute);
            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Subscribe(OnRefresh);

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
            _dialogService.ShowWindow((viewModel, error) =>
            {
                if (error != null)
                {
                    return;
                }

                if (((AddVehicleViewModel)viewModel).DialogResult)
                {
                    LoadVehicles();
                }
            }, new AddVehicle(), 382, 407);
        }
        #endregion

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

        /// <summary>
        /// Retrieves all of the vehicles saved to the database and create a view model for each one.
        /// </summary>
        public void LoadVehicles()
        {
            Vehicles = new ObservableCollection<VehicleViewModel>();

            _dataService.GetVehicles((vehicles, error) =>
            {
                if (error != null)
                {
                    return;
                }

                //TODO - do the query.Subscribe bit here Reactive
                foreach (var vehicle in vehicles)
                {
                    var viewModel = ServiceLocator.Current.GetInstance<VehicleViewModel>();
                    viewModel.LoadInstance(vehicle);
                    Vehicles.Add(viewModel);
                }
            }, true);
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

    }
}   //ImprezGarage.Modules.Dashboard.ViewModels namespace 