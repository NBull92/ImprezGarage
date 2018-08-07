//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class AddVehicleViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly INotificationsService _notificationsService;

        private ObservableCollection<VehicleType> _vehicleTypes;
        private VehicleType _selectedVehicleType;
        private VehicleCreationViewModel _vehicleCreationViewModel;
        private bool _dialogResult;
        private string _saveContent;

        private const string INSURANCE_DATE_WARNING = "You have selected to add the vehicles insurance date, however the data entered is in the past. \n\nIs this intended?";
        private const string TAX_RENEWAL_DATE_WARNING = "You have selected to add the vehicles tax renewal date, however the data entered is in the past. \n\nIs this intended?";
        private const string NOTIFICATION_HEADER = "Alert!";
        private const string ERROR_OCCURED_DURING_SAVE = "An error occured during the save. \n\nPlease check all the data provided is correct and try again. If this issue persists, then contact support.";
        private const string VEHICLE_UPDATED = "Vehicle updated successfully.";
        private const string VEHICLE_ADDED = "Vehicle added successfully!";

        public bool IsEdit;
        public event EventHandler ClosingRequest;
        #endregion

        #region Parameters
        public ObservableCollection<VehicleType> VehicleTypes
        {
            get => _vehicleTypes;
            set => SetProperty(ref _vehicleTypes, value);
        }

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
                            VehicleCreationViewModel.VehicleType = VehicleTypes.FirstOrDefault(o => o.Name == "Car");
                            VehicleCreationViewModel.AddVehicleVm = this;
                            ((CarCreationViewModel)VehicleCreationViewModel).InsuranceRenewalDate = DateTime.Now;
                            ((CarCreationViewModel)VehicleCreationViewModel).TaxExpiryDate = DateTime.Now;
                            break;
                        case "Bicycle":
                            VehicleCreationViewModel = _container.Resolve<BicycleCreationViewModel>();
                            VehicleCreationViewModel.VehicleType = VehicleTypes.FirstOrDefault(o => o.Name == "Bicycle");
                            break;
                        case "Motorbike":
                            VehicleCreationViewModel = _container.Resolve<MotorbikeCreationViewModel>();
                            VehicleCreationViewModel.VehicleType = VehicleTypes.FirstOrDefault(o => o.Name == "Motorbike");
                            ((MotorbikeCreationViewModel)VehicleCreationViewModel).InsuranceRenewalDate = DateTime.Now;
                            ((MotorbikeCreationViewModel)VehicleCreationViewModel).TaxExpiryDate = DateTime.Now;
                            break;
                    }
                }
                else
                {
                    // TODO: GetType of viewmodel then clean up all the bindable variables
                    VehicleCreationViewModel = null;
                    VehicleCreationViewModel.CleanUp();
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
                RaisePropertyChanged("VehicleCreationViewModel");
            }
        }

        public bool DialogResult
        {
            get => _dialogResult;
            set => SetProperty(ref _dialogResult, value);
        }

        // Could be Add or Save, dependant on whether the user is adding a new vehicle or editing a current one.
        public string SaveContent
        {
            get => _saveContent;
            set => SetProperty(ref _saveContent, value);
        }

        #region Commands
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand<object> CancelCommand { get; private set; }
        public Vehicle EditVehicle { get; internal set; }
        #endregion

        #endregion

        #region Methods
        public AddVehicleViewModel(IDataService dataService, IRegionManager regionManager, IUnityContainer container, INotificationsService notificationsService)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _container = container;
            _notificationsService = notificationsService;

            SaveCommand = new DelegateCommand(SaveExecute, CanSave);
            CancelCommand = new DelegateCommand<object>(CancelExecute);

            GetVehicleTypes();
            SaveContent = "Add";
        }

        #region Command Handlers
        /// <summary>
        /// Cancel the add or edit by closing the window
        /// </summary>
        /// <param name="navigationPath"></param>
        private void CancelExecute(object navigationPath)
        {
            DialogResult = false;
            Close();
        }

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
                if (_notificationsService.Confirm("Are you sure you wish to save these changes?"))
                {
                    bool proceed = false;

                    // Check the insurance and tax are selected. 
                    // If so check the dates provided are not in the past. 
                    // If they are then warn the user and see if this is as intended.
                    switch (_selectedVehicleType.Name)
                    {
                        case "Car":
                            var carCreate = ((CarCreationViewModel)VehicleCreationViewModel);
                            if (carCreate.HasValidTax && carCreate.TaxExpiryDate.Date <= DateTime.Today)
                            {
                                proceed = _notificationsService.Confirm(TAX_RENEWAL_DATE_WARNING, "Warning!");
                            }
                            else if (carCreate.HasInsurance && carCreate.InsuranceRenewalDate.Date <= DateTime.Today)
                            {
                                proceed = _notificationsService.Confirm(INSURANCE_DATE_WARNING, "Warning!");
                            }
                            break;
                        case "Bicycle":
                            break;
                        case "Motorbike":
                            var motorbikeCreate = ((MotorbikeCreationViewModel)VehicleCreationViewModel);
                            if (motorbikeCreate.HasValidTax && motorbikeCreate.TaxExpiryDate.Date <= DateTime.Today)
                            {
                                proceed = _notificationsService.Confirm(TAX_RENEWAL_DATE_WARNING, "Warning!");
                            }
                            else if (motorbikeCreate.HasInsurance && motorbikeCreate.InsuranceRenewalDate.Date <= DateTime.Today)
                            {
                                proceed = _notificationsService.Confirm(INSURANCE_DATE_WARNING, "Warning!");
                            }
                            break;
                    }

                    if (!proceed)
                        return;

                    switch (_selectedVehicleType.Name)
                    {
                        case "Car":
                            EditVehicle.Registration = ((CarCreationViewModel)VehicleCreationViewModel).Registration;
                            EditVehicle.Make = ((CarCreationViewModel)VehicleCreationViewModel).Make;
                            EditVehicle.Model = ((CarCreationViewModel)VehicleCreationViewModel).Model;
                            EditVehicle.HasInsurance = ((CarCreationViewModel)VehicleCreationViewModel).HasInsurance;
                            EditVehicle.HasValidTax = ((CarCreationViewModel)VehicleCreationViewModel).HasValidTax;

                            if (((CarCreationViewModel)VehicleCreationViewModel).HasValidTax)
                            {
                                EditVehicle.InsuranceRenewalDate = ((CarCreationViewModel)VehicleCreationViewModel).InsuranceRenewalDate;
                            }
                            else
                            {
                                EditVehicle.InsuranceRenewalDate = null;
                            }

                            if (((CarCreationViewModel)VehicleCreationViewModel).HasValidTax)
                            {
                                EditVehicle.TaxExpiryDate = ((CarCreationViewModel)VehicleCreationViewModel).TaxExpiryDate;
                            }
                            else
                            {
                                EditVehicle.TaxExpiryDate = null;
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
                            else
                            {
                                EditVehicle.InsuranceRenewalDate = null;
                            }

                            if (((MotorbikeCreationViewModel)VehicleCreationViewModel).HasValidTax)
                            {
                                EditVehicle.TaxExpiryDate = ((MotorbikeCreationViewModel)VehicleCreationViewModel).TaxExpiryDate;
                            }
                            else
                            {
                                EditVehicle.TaxExpiryDate = null;
                            }
                            break;
                    }

                    DialogResult = true;

                    _dataService.UpdateVehicle((error) =>
                    {
                        if (error != null)
                        {
                            _notificationsService.Alert(ERROR_OCCURED_DURING_SAVE, NOTIFICATION_HEADER);
                        }
                        else
                        {
                            _notificationsService.Alert(VEHICLE_UPDATED, NOTIFICATION_HEADER);
                            Close();
                        }
                    }, EditVehicle);
                }
            }
            else
            {
                DialogResult = true;
                SaveNewVehicle();
            }
        }

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

                    if (vehicleCreate.HasInsurance)
                    {
                        newVehicle.InsuranceRenewalDate = vehicleCreate.InsuranceRenewalDate;
                    }

                    if (vehicleCreate.HasValidTax)
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

                    if (vehicleCreate.HasInsurance)
                    {
                        newVehicle.InsuranceRenewalDate = vehicleCreate.InsuranceRenewalDate;
                    }

                    if (vehicleCreate.HasValidTax)
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

            _dataService.AddNewVehicle((error) =>
            {
                if (error != null)
                {
                    return;
                }
                                
                _notificationsService.Alert(VEHICLE_ADDED, NOTIFICATION_HEADER);
                Close();
            }, newVehicle);
        }
        #endregion

        /// <summary>
        /// This function retrieves the vehicle types from the database.
        /// </summary>
        private void GetVehicleTypes()
        {
            var types = _dataService.GetVehicleTypes();

            VehicleTypes = types == null && types.Result == null ? VehicleTypes = new ObservableCollection<VehicleType>() 
                : VehicleTypes = new ObservableCollection<VehicleType>(types.Result);
        }

        /// <summary>
        /// Takes the passed through vehicle the user has selected to edit and applies its data to the variables that are shown on the UI.
        /// </summary>
        /// <param name="vehicle"></param>
        internal void Edit(Vehicle vehicle)
        {
            EditVehicle = vehicle;

            if (VehicleTypes.Any(o => o.Id == EditVehicle.VehicleType))
            {
                SelectedVehicleType = VehicleTypes.First(o => o.Id == EditVehicle.VehicleType);

                switch (_selectedVehicleType.Name)
                {
                    case "Car":
                        ((CarCreationViewModel)VehicleCreationViewModel).Registration = EditVehicle.Registration;
                        ((CarCreationViewModel)VehicleCreationViewModel).Make = EditVehicle.Make;
                        ((CarCreationViewModel)VehicleCreationViewModel).Model = EditVehicle.Model;
                        ((CarCreationViewModel)VehicleCreationViewModel).HasInsurance = Convert.ToBoolean(EditVehicle.HasInsurance);
                        ((CarCreationViewModel)VehicleCreationViewModel).HasValidTax = Convert.ToBoolean(EditVehicle.HasValidTax);
                        ((CarCreationViewModel)VehicleCreationViewModel).InsuranceRenewalDate = ((CarCreationViewModel)VehicleCreationViewModel).HasInsurance ? Convert.ToDateTime(EditVehicle.InsuranceRenewalDate) : DateTime.Now;
                        ((CarCreationViewModel)VehicleCreationViewModel).TaxExpiryDate = ((CarCreationViewModel)VehicleCreationViewModel).HasValidTax ? Convert.ToDateTime(EditVehicle.TaxExpiryDate) : DateTime.Now;
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
        /// Forces the save coommand to retry its can save event.
        /// </summary>
        private void VehicleCreationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        private void Close()
        {
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 