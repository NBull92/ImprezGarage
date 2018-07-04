//------------------------------------------------------------------------------
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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;

    public class MainViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<VehicleViewModel> _vehicles;
        private VehicleViewModel _selectedVehicle;
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
        public MainViewModel(IDataService dataService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            AddNewVehicleCommand = new DelegateCommand<object>(AddNewVehicleExecute);
            SelectedVehicleChanged = new DelegateCommand<SelectionChangedEventArgs>(SelectedVehicleChangedExecute);
            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Subscribe(OnRefresh);
            _eventAggregator.GetEvent<Events.EditVehicleEvent>().Subscribe(OnEditVehicle);

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
        #endregion
    }
}   //ImprezGarage.Modules.Dashboard.ViewModels namespace 