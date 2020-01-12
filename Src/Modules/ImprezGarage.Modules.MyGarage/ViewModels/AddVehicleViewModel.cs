//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

using ImprezGarage.Infrastructure.Model;

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using ImprezGarage.Infrastructure.ViewModels;
    using Infrastructure.BaseClasses;
    using Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Prism.Commands;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddVehicleViewModel : DialogViewModelBase
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

        private const string InsuranceDateWarning = "You have selected to add the vehicles insurance date, however the data entered is in the past. \n\nIs this intended?";
        private const string TaxRenewalDateWarning = "You have selected to add the vehicles tax renewal date, however the data entered is in the past. \n\nIs this intended?";
        private const string NotificationHeader = "Alert!";
        private const string ErrorOccuredDuringSave = "An error occured during the save. \n\nPlease check all the data provided is correct and try again. If this issue persists, then contact support.";
        private const string VehicleUpdated = "Vehicle updated successfully.";
        private const string VehicleAdded = "Vehicle added successfully!";

        /// <summary>
        /// A collection of all the vehicle types.
        /// </summary>
        private ObservableCollection<VehicleType> _vehicleTypes;

        /// <summary>
        /// Store which vehicle type the user selected.
        /// </summary>
        private VehicleType _selectedVehicleType;

        private VehicleCreationViewModel _vehicleCreationViewModel;
        
        /// <summary>
        /// Could be Add or Save, dependent on whether the user is adding a new vehicle or editing a current one.
        /// </summary>
        private string _saveContent;

        private int _width;

        /// <summary>
        /// True if we editing and already existing vehicle.
        /// </summary>
        private bool _isEdit;
        #endregion

        #region Parameters
        /// <summary>
        /// A collection of all the vehicle types.
        /// </summary>
        public ObservableCollection<VehicleType> VehicleTypes
        {
            get => _vehicleTypes;
            set => SetProperty(ref _vehicleTypes, value);
        }

        /// <summary>
        /// Store which vehicle type the user selected.
        /// Then go through and setup the defaults of the selected view model type.
        /// </summary>
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
                            VehicleCreationViewModel.AddVehicleVm = this;
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
                }
                else
                {
                    // TODO: GetType of viewmodel then clean up all the bindable variables
                    VehicleCreationViewModel.Dispose();
                    VehicleCreationViewModel = null;
                }
            }
        }

        public VehicleCreationViewModel VehicleCreationViewModel
        {
            get => _vehicleCreationViewModel;
            set
            {                
                SetProperty(ref _vehicleCreationViewModel, value);
                _vehicleCreationViewModel.PropertyChanged += VehicleCreationViewModel_PropertyChanged;
            }
        }
        
        /// <summary>
        /// Could be Add or Save, dependent on whether the user is adding a new vehicle or editing a current one.
        /// </summary>
        public string SaveContent
        {
            get => _saveContent;
            set => SetProperty(ref _saveContent, value);
        }

        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// True if we editing and already existing vehicle.
        /// </summary>
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        #region Commands
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public Vehicle EditVehicle { get; internal set; }
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Constructor, with injected interfaces.
        /// Instantiate the commands and get the vehicle types.
        /// </summary> 
        public AddVehicleViewModel(IDataService dataService, IUnityContainer container, INotificationsService notificationsService,
            ILoggerService loggerService, VehicleViewModel vehicle = null)
        {
            _dataService = dataService;
            _container = container;
            _notificationsService = notificationsService;
            _loggerService = loggerService;

            SaveCommand = new DelegateCommand(SaveExecute, CanSave);
            CancelCommand = new DelegateCommand(Close);
            Width = 387;
            InitialiseAsync(vehicle);
        }

        private async void InitialiseAsync(VehicleViewModel vehicle)
        {
            await GetVehicleTypes();

            if (vehicle != null)
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

            if (VehicleCreationViewModel.GetType() == typeof(CarCreationViewModel))
            {
                if (!(VehicleCreationViewModel is CarCreationViewModel vehicleCreate))
                    return true;

                if (string.IsNullOrEmpty(vehicleCreate.Registration))
                {
                    return false;
                }
            }
            else if (VehicleCreationViewModel.GetType() == typeof(MotorbikeCreationViewModel))
            {
                if (!(VehicleCreationViewModel is MotorbikeCreationViewModel vehicleCreate))
                    return true;

                if (string.IsNullOrEmpty(vehicleCreate.Registration))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Takes all the information the user has provided and creates a new Vehicle entity to add to the database.
        /// </summary>
        private void SaveExecute()
        {
            if (IsEdit)
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
                        var carCreate = ((CarCreationViewModel)VehicleCreationViewModel);
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
                        var motorbikeCreate = ((MotorbikeCreationViewModel)VehicleCreationViewModel);
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

                switch (_selectedVehicleType.Name)
                {
                    case "Car":
                        var car = ((CarCreationViewModel)VehicleCreationViewModel);
                        EditVehicle.Registration = car.Registration;
                        EditVehicle.Make = car.Make;
                        EditVehicle.Model = car.Model;
                        EditVehicle.HasInsurance = car.HasInsurance;
                        EditVehicle.HasValidTax = car.HasValidTax;
                        EditVehicle.HasMot = car.HasMot;
                        EditVehicle.CurrentMileage = car.CurrentMileage;
                        EditVehicle.MileageOnPurchase = car.MileageOnPurchase;
                        EditVehicle.IsManual = car.IsManual;

                        if (car.HasValidTax)
                        {
                            EditVehicle.InsuranceRenewalDate = car.InsuranceRenewalDate;
                        }

                        if (car.HasValidTax)
                        {
                            EditVehicle.TaxExpiryDate = car.TaxExpiryDate;
                        }
                        
                        if (EditVehicle.HasMot)
                        {
                            EditVehicle.MotExpiryDate = car.MotExpiryDate;
                        }
                        break;
                    case "Bicycle":
                        break;
                    case "Motorbike":
                        EditVehicle.Registration = ((MotorbikeCreationViewModel)VehicleCreationViewModel).Registration;
                        EditVehicle.Make = ((MotorbikeCreationViewModel)VehicleCreationViewModel).Make;
                        EditVehicle.Model = ((MotorbikeCreationViewModel)VehicleCreationViewModel).Model;
                        EditVehicle.HasInsurance = ((MotorbikeCreationViewModel)VehicleCreationViewModel).HasInsurance;
                        EditVehicle.HasValidTax = ((MotorbikeCreationViewModel)VehicleCreationViewModel).HasValidTax;

                        if (((MotorbikeCreationViewModel)VehicleCreationViewModel).HasValidTax)
                        {
                            EditVehicle.InsuranceRenewalDate = ((MotorbikeCreationViewModel)VehicleCreationViewModel).InsuranceRenewalDate;
                        }

                        if (((MotorbikeCreationViewModel)VehicleCreationViewModel).HasValidTax)
                        {
                            EditVehicle.TaxExpiryDate = ((MotorbikeCreationViewModel)VehicleCreationViewModel).TaxExpiryDate;
                        }
                        break;
                }

                DialogResult = true;

                try
                {
                    _dataService.UpdateVehicle(EditVehicle);
                    _notificationsService.Alert(VehicleUpdated, NotificationHeader);
                    Close();
                }
                catch (Exception e)
                {
                    _loggerService.LogException(e);
                    _notificationsService.Alert(ErrorOccuredDuringSave, NotificationHeader);
                }
            }
            else
            {
                SaveNewVehicle();
            }
        }
        #endregion

        /// <summary>
        /// Take all the data the user has set and adds it to a new vehicle. This vehicle is then added to the database.
        /// </summary>
        private void SaveNewVehicle()
        {
            var newVehicle = new Vehicle
            {
                VehicleType = VehicleCreationViewModel.VehicleType.Id,
                Model = VehicleCreationViewModel.Model,
                Make = VehicleCreationViewModel.Make,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            if (VehicleCreationViewModel.GetType() == typeof(CarCreationViewModel))
            {
                if (VehicleCreationViewModel is CarCreationViewModel vehicleCreate)
                {
                    newVehicle.Registration = vehicleCreate.Registration;
                    newVehicle.HasInsurance = vehicleCreate.HasInsurance;
                    newVehicle.HasValidTax = vehicleCreate.HasValidTax;
                    newVehicle.HasMot = vehicleCreate.HasMot;
                    newVehicle.CurrentMileage = vehicleCreate.CurrentMileage;
                    newVehicle.MileageOnPurchase = vehicleCreate.MileageOnPurchase;
                    newVehicle.IsManual = vehicleCreate.IsManual;

                    if (newVehicle.HasInsurance)
                    {
                        newVehicle.InsuranceRenewalDate = newVehicle.InsuranceRenewalDate;
                    }


                    if (newVehicle.HasValidTax)
                    {
                        newVehicle.TaxExpiryDate = vehicleCreate.TaxExpiryDate;

                    }
                }
            }
            else if (VehicleCreationViewModel.GetType() == typeof(MotorbikeCreationViewModel))
            {
                if (VehicleCreationViewModel is MotorbikeCreationViewModel vehicleCreate)
                {
                    newVehicle.Registration = vehicleCreate.Registration;
                    newVehicle.HasInsurance = vehicleCreate.HasInsurance;
                    newVehicle.HasValidTax = vehicleCreate.HasValidTax;

                    if (newVehicle.HasInsurance)
                    {
                        newVehicle.InsuranceRenewalDate = newVehicle.InsuranceRenewalDate;
                    }


                    if (newVehicle.HasValidTax)
                    {
                        newVehicle.TaxExpiryDate = vehicleCreate.TaxExpiryDate;

                    }
                }
            }
            else if (VehicleCreationViewModel.GetType() == typeof(BicycleCreationViewModel))
            {
                if (VehicleCreationViewModel is BicycleCreationViewModel vehicleCreate)
                {
                    //newVehicle.VehicleType = vehicleCreate.VehicleType.Id;
                }
            }

            try
            {
                _dataService.AddNewVehicle(newVehicle);
                _notificationsService.Alert(VehicleAdded, NotificationHeader);
                Close(true);
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
        }

        /// <summary>
        /// Takes the passed through vehicle the user has selected to edit and applies its data to the variables that are shown on the UI.
        /// </summary>
        /// <param name="vehicle"></param>
        private void Edit(Vehicle vehicle)
        {
            IsEdit = true;
            EditVehicle = vehicle;

            if (VehicleTypes.Any(o => o.Id == EditVehicle.VehicleType))
            {
                SelectedVehicleType = VehicleTypes.First(o => o.Id == EditVehicle.VehicleType);
                VehicleCreationViewModel.ShowEditView = true;
                Width = 706;

                switch (_selectedVehicleType.Name)
                {
                    case "Car":
                        ((CarCreationViewModel)VehicleCreationViewModel).Registration = EditVehicle.Registration;
                        ((CarCreationViewModel)VehicleCreationViewModel).Make = EditVehicle.Make;
                        ((CarCreationViewModel)VehicleCreationViewModel).Model = EditVehicle.Model;
                        ((CarCreationViewModel)VehicleCreationViewModel).HasInsurance = Convert.ToBoolean(EditVehicle.HasInsurance);
                        ((CarCreationViewModel)VehicleCreationViewModel).HasValidTax = Convert.ToBoolean(EditVehicle.HasValidTax);
                        ((CarCreationViewModel)VehicleCreationViewModel).HasMot = Convert.ToBoolean(EditVehicle.HasMot);
                        ((CarCreationViewModel)VehicleCreationViewModel).InsuranceRenewalDate = ((CarCreationViewModel)VehicleCreationViewModel).HasInsurance ? Convert.ToDateTime(EditVehicle.InsuranceRenewalDate) : DateTime.Now;
                        ((CarCreationViewModel)VehicleCreationViewModel).TaxExpiryDate = ((CarCreationViewModel)VehicleCreationViewModel).HasValidTax ? Convert.ToDateTime(EditVehicle.TaxExpiryDate) : DateTime.Now;
                        ((CarCreationViewModel)VehicleCreationViewModel).MotExpiryDate = ((CarCreationViewModel)VehicleCreationViewModel).HasMot ? Convert.ToDateTime(EditVehicle.MotExpiryDate) : DateTime.Now;
                        ((CarCreationViewModel)VehicleCreationViewModel).CurrentMileage = Convert.ToInt32(EditVehicle.CurrentMileage);
                        ((CarCreationViewModel)VehicleCreationViewModel).MileageOnPurchase = Convert.ToInt32(EditVehicle.MileageOnPurchase);
                        ((CarCreationViewModel)VehicleCreationViewModel).IsManual = Convert.ToBoolean(EditVehicle.IsManual);
                        break;
                    case "Bicycle":
                        break;
                    case "Motorbike":
                        ((MotorbikeCreationViewModel)VehicleCreationViewModel).Registration = EditVehicle.Registration;
                        ((MotorbikeCreationViewModel)VehicleCreationViewModel).Make = EditVehicle.Make;
                        ((MotorbikeCreationViewModel)VehicleCreationViewModel).Model = EditVehicle.Model;
                        ((MotorbikeCreationViewModel)VehicleCreationViewModel).HasInsurance = Convert.ToBoolean(EditVehicle.HasInsurance);
                        ((MotorbikeCreationViewModel)VehicleCreationViewModel).HasValidTax = Convert.ToBoolean(EditVehicle.HasValidTax);
                        ((MotorbikeCreationViewModel)VehicleCreationViewModel).InsuranceRenewalDate = ((MotorbikeCreationViewModel)VehicleCreationViewModel).HasInsurance ? Convert.ToDateTime(EditVehicle.InsuranceRenewalDate) : DateTime.Now;
                        ((MotorbikeCreationViewModel)VehicleCreationViewModel).TaxExpiryDate = ((MotorbikeCreationViewModel)VehicleCreationViewModel).HasValidTax ? Convert.ToDateTime(EditVehicle.TaxExpiryDate) : DateTime.Now;
                        break;
                }
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
            if(VehicleCreationViewModel != null)
                VehicleCreationViewModel.PropertyChanged -= VehicleCreationViewModel_PropertyChanged;
        }
        #endregion
        #endregion
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 