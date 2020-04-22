//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using CreationViewModels;
    using Infrastructure;
    using Infrastructure.BaseClasses;
    using Infrastructure.Model;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    public class ManageVehicleViewModel : DialogViewModelBase, INavigationAware
    {
        #region Attributes
        /// <summary>
        /// Store the injected Data Service
        /// </summary>
        private readonly IDataService _dataService;

        /// <summary>
        /// Store the injected unity container
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Store the injected notification service
        /// </summary>
        private readonly INotificationsService _notificationsService;

        /// <summary>
        /// Store the injected logger service
        /// </summary>
        private readonly ILoggerService _loggerService;

        private readonly IEventAggregator _eventAggregator;

        private const string InsuranceDateWarning = "You have selected to add the vehicles insurance date, however the data entered is in the past. \n\nIs this intended?";
        private const string TaxRenewalDateWarning = "You have selected to add the vehicles tax renewal date, however the data entered is in the past. \n\nIs this intended?";
        private const string NotificationHeader = "Alert!";
        private const string ErrorOccuredDuringSave = "An error occured during the save. \n\nPlease check all the data provided is correct and try again. If this issue persists, then contact support.";
        private const string VehicleUpdated = "Vehicle updated successfully.";
        private const string VehicleAdded = "Vehicle added successfully!";
        #endregion

        #region Parameters
        /// <summary>
        /// A collection of all the vehicle types.
        /// </summary>
        private ObservableCollection<VehicleType> _vehicleTypes;
        public ObservableCollection<VehicleType> VehicleTypes
        {
            get => _vehicleTypes;
            set => SetProperty(ref _vehicleTypes, value);
        }

        /// <summary>
        /// Store which vehicle type the user selected.
        /// Then go through and setup the defaults of the selected view model type.
        /// </summary>
        private VehicleType _selectedVehicleType;
        public VehicleType SelectedVehicleType
        {
            get => _selectedVehicleType;
            set
            {
                SetProperty(ref _selectedVehicleType, value);

                if (_selectedVehicleType != null)
                {
                    switch (_selectedVehicleType.Name)
                    {
                        case "Car":
                            VehicleCreationViewModel = _container.Resolve<CarCreationViewModel>();
                            ((CarCreationViewModel)VehicleCreationViewModel).Setup(VehicleTypes.FirstOrDefault(o => o.Name == "Car"));
                            break;
                        case "Bicycle":
                            VehicleCreationViewModel = _container.Resolve<BicycleCreationViewModel>();
                            ((BicycleCreationViewModel)VehicleCreationViewModel).Setup(VehicleTypes.FirstOrDefault(o => o.Name == "Bicycle"));
                            break;
                        case "Motorbike":
                            VehicleCreationViewModel = _container.Resolve<MotorbikeCreationViewModel>();
                            ((MotorbikeCreationViewModel)VehicleCreationViewModel).Setup(VehicleTypes.FirstOrDefault(o => o.Name == "Motorbike"));
                            break;
                    }
                    VehicleCreationViewModel.PropertyChanged += VehicleCreationViewModel_PropertyChanged;
                }
            }
        }
        
        private VehicleCreationViewModel _vehicleCreationViewModel;
        public VehicleCreationViewModel VehicleCreationViewModel
        {
            get => _vehicleCreationViewModel;
            set => SetProperty(ref _vehicleCreationViewModel, value);
        }
        
        /// <summary>
        /// Could be Add or Save, dependent on whether the user is adding a new vehicle or editing a current one.
        /// </summary>
        private string _saveContent;
        public string SaveContent
        {
            get => _saveContent;
            set => SetProperty(ref _saveContent, value);
        }

        private int _width;
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// True if we editing and already existing vehicle.
        /// </summary>
        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public Vehicle EditVehicle { get; internal set; }
        
        #region Commands
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Constructor, with injected interfaces.
        /// Instantiate the commands and get the vehicle types.
        /// </summary> 
        public ManageVehicleViewModel(IDataService dataService, IUnityContainer container, INotificationsService notificationsService,
            ILoggerService loggerService, IEventAggregator eventAggregator, IVehicleService vehicleService, VehicleViewModel vehicle = null)
        {
            _dataService = dataService;
            _container = container;
            _notificationsService = notificationsService;
            _loggerService = loggerService;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(SaveExecute, CanSave);
            CancelCommand = new DelegateCommand(Close);

            Width = 305;
            InitialiseAsync(vehicle);

            vehicleService.SelectedVehicleChanged += OnSelectedVehicleChanged;

        }

        private async void OnSelectedVehicleChanged(object sender, Vehicle vehicle)
        {
            await GetVehicleTypes();

            if (IsEdit)
            {
                Edit(vehicle);
            }
        }

        private async void InitialiseAsync(VehicleViewModel vehicle = null)
        {
            await GetVehicleTypes();

            if (vehicle?.Vehicle != null)
            {
                Edit(vehicle.Vehicle);
            }
            else
            {
                SaveContent = "Add";
            }
        }

        #region Command Handlers
        /// <summary>
        /// Checks as to whether the user has entered all the required data before they can save.
        /// </summary>
        /// <returns>True/False</returns>
        private bool CanSave()
        {
            if (VehicleCreationViewModel?.VehicleType == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(VehicleCreationViewModel.Model))
            {
                return false;
            }

            if (string.IsNullOrEmpty(VehicleCreationViewModel.Make))
            {
                return false;
            }

            return VehicleCreationViewModel.CanSave();
        }

        /// <summary>
        /// Takes all the information the user has provided and creates a new Vehicle entity to add to the database.
        /// </summary>
        private void SaveExecute()
        {
            if (IsEdit)
            {
                UpdateExisting();
            }
            else
            {
                SaveNewVehicle();
            }
        }

        private void UpdateExisting()
        {
            if (!_notificationsService.Confirm("Are you sure you wish to save these changes?"))
                return;

            var proceed = true;

            // Check the insurance and tax are selected. 
            // If so check the dates provided are not in the past. 
            // If they are then warn the user and see if this is as intended.
            switch (_selectedVehicleType.Name)
            {
                case "Car":
                    var carCreate = ((CarCreationViewModel) VehicleCreationViewModel);
                    if (carCreate.HasValidTax && carCreate.TaxExpiryDate.Date <= DateTime.Today)
                    {
                        proceed = _notificationsService.Confirm(TaxRenewalDateWarning, "Warning!");
                    }
                    else if (carCreate.HasInsurance && carCreate.InsuranceRenewalDate.Date <= DateTime.Today)
                    {
                        proceed = _notificationsService.Confirm(InsuranceDateWarning, "Warning!");
                    }

                    break;
                case "Bicycle":
                    break;
                case "Motorbike":
                    var motorbikeCreate = ((MotorbikeCreationViewModel) VehicleCreationViewModel);
                    if (motorbikeCreate.HasValidTax && motorbikeCreate.TaxExpiryDate.Date <= DateTime.Today)
                    {
                        proceed = _notificationsService.Confirm(TaxRenewalDateWarning, "Warning!");
                    }
                    else if (motorbikeCreate.HasInsurance && motorbikeCreate.InsuranceRenewalDate.Date <= DateTime.Today)
                    {
                        proceed = _notificationsService.Confirm(InsuranceDateWarning, "Warning!");
                    }

                    break;
            }

            if (!proceed)
                return;

            VehicleCreationViewModel.Update(EditVehicle);

            try
            {
                _dataService.UpdateVehicle(EditVehicle);
                _notificationsService.Alert(VehicleUpdated, NotificationHeader);
                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
                _notificationsService.Alert(ErrorOccuredDuringSave, NotificationHeader);
            }
        }

        #endregion

        /// <summary>
        /// Take all the data the user has set and adds it to a new vehicle. This vehicle is then added to the database.
        /// </summary>
        private void SaveNewVehicle()
        {
            var authentication = ServiceLocator.Current.GetInstance<IAuthenticationService>();

            var newVehicle = new Vehicle
            {
                VehicleType = VehicleCreationViewModel.VehicleType.Id,
                Model = VehicleCreationViewModel.Model,
                Make = VehicleCreationViewModel.Make,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                UserId = authentication.CurrentUser().UserId
            };

            VehicleCreationViewModel.SaveNew(newVehicle);

            try
            {
                _dataService.AddNewVehicle(newVehicle);
                _notificationsService.Alert(VehicleAdded, NotificationHeader);
                DialogResult = true;
                Close();
            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
            }
        }

        /// <summary>
        /// This function retrieves the vehicle types from the database.
        /// </summary>
        private async Task GetVehicleTypes()
        {
            var types = await _dataService.GetVehicleTypesAsync();

            VehicleTypes = types == null ? VehicleTypes = new ObservableCollection<VehicleType>() 
                : VehicleTypes = new ObservableCollection<VehicleType>(types);

            SelectedVehicleType = VehicleTypes.FirstOrDefault();
        }

        /// <summary>
        /// Takes the passed through vehicle the user has selected to edit and applies its data to the variables that are shown on the UI.
        /// </summary>
        /// <param name="vehicle"></param>
        private void Edit(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                EditVehicle = null;
                VehicleCreationViewModel?.CleanUp();
                VehicleCreationViewModel = null;
                SelectedVehicleType = null;
                return;
            }

            IsEdit = true;
            EditVehicle = vehicle;

            if (VehicleTypes.Any(o => o.Id == EditVehicle.VehicleType))
            {
                SelectedVehicleType = VehicleTypes.First(o => o.Id == EditVehicle.VehicleType);
                Width = 610;
                VehicleCreationViewModel.EditInitialise(EditVehicle);
            }

            SaveContent = "Save";
        }

        /// <summary>
        /// Forces the save command to retry its can save event.
        /// </summary>
        private void VehicleCreationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        
        #region IDisposable
        public override void Dispose()
        {
            VehicleTypes.Clear();
            VehicleTypes = null;

            if (VehicleCreationViewModel != null)
                VehicleCreationViewModel.PropertyChanged -= VehicleCreationViewModel_PropertyChanged;
        }
        #endregion

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var vehicleService = ServiceLocator.Current.GetInstance<IVehicleService>();
            var vehicle = vehicleService.GetSelectedVehicle();

            if (vehicle == null)
                return;

            var model = new VehicleViewModel(_dataService, _notificationsService);
            model.LoadInstance(vehicle);
            InitialiseAsync(model);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Dispose();
        }
        #endregion
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 