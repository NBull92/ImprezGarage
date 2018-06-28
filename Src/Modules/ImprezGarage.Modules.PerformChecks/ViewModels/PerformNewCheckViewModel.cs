//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Dialogs;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Modules.PerformChecks.Views;
    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;

    public class PerformNewCheckViewModel : BindableBase, INavigationAware
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private static int _selectedVehicleId;
        private MaintenanceCheck _maintenanceCheck;
        private const string CHECK_COMPLETED = "Maintenance check completed!";
        private const string CHECK_UPDATED = "Maintenance check updated!";
        private const string NOTIFICATION_HEADER = "Alert!";
        private MaintenanceCheckType _selectedMaintenanceCheckType;
        private bool _isEditMode;
        private string _submitText;

        #region Check box variables
        private bool _checkedAirFilter;
        private bool _replacedAirFilter;
        private bool _checkCoolantLevels;
        private bool _flushedSystemAndChangeCoolant;
        private bool _changeFanBelt;
        private bool _checkedBattery;
        private bool _checkedOilLevels;
        private bool _changedOil;
        private bool _replacedOilFilter;
        private bool _checkAutoTransmissionFluid;
        private bool _addedAutoTransmissionFluid;
        private bool _checkPowerSteeringFluidLevels;
        #endregion

        #region Notes variables
        private string _airFilterNotes;
        private string _coolantNotes;
        private string _batteryNotes;
        private string _oilLevelNotes;
        private string _autoTransmissionFluidNotes;
        private string _powerSteeringNotes;
        #endregion

        #endregion

        #region Properties
        public MaintenanceCheckType SelectedMaintenanceCheckType
        {
            get => _selectedMaintenanceCheckType;
            set => SetProperty(ref _selectedMaintenanceCheckType, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public string SubmitText
        {
            get => _submitText;
            set => SetProperty(ref _submitText, value);
        }

        #region Check box variables
        public bool CheckedAirFilter
        {
            get => _checkedAirFilter;
            set
            {
                SetProperty(ref _checkedAirFilter, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool ReplacedAirFilter
        {
            get => _replacedAirFilter;
            set
            {
                SetProperty(ref _replacedAirFilter, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CheckCoolantLevels
        {
            get => _checkCoolantLevels;
            set
            {
                SetProperty(ref _checkCoolantLevels, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool FlushedSystemAndChangeCoolant
        {
            get => _flushedSystemAndChangeCoolant;
            set
            {
                SetProperty(ref _flushedSystemAndChangeCoolant, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool ChangeFanBelt
        {
            get => _changeFanBelt;
            set
            {
                SetProperty(ref _changeFanBelt, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CheckedBattery
        {
            get => _checkedBattery;
            set
            {
                SetProperty(ref _checkedBattery, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CheckedOilLevels
        {
            get => _checkedOilLevels;
            set
            {
                SetProperty(ref _checkedOilLevels, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool ChangedOil
        {
            get => _changedOil;
            set
            {
                SetProperty(ref _changedOil, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool ReplacedOilFilter
        {
            get => _replacedOilFilter;
            set
            {
                SetProperty(ref _replacedOilFilter, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CheckAutoTransmissionFluid
        {
            get => _checkAutoTransmissionFluid;
            set
            {
                SetProperty(ref _checkAutoTransmissionFluid, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool AddedAutoTransmissionFluid
        {
            get => _addedAutoTransmissionFluid;
            set
            {
                SetProperty(ref _addedAutoTransmissionFluid, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CheckPowerSteeringFluidLevels
        {
            get => _checkPowerSteeringFluidLevels;
            set
            {               
                SetProperty(ref _checkPowerSteeringFluidLevels, value);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Notes variables
        public string AirFilterNotes
        {
            get => _airFilterNotes;
            set => SetProperty(ref _airFilterNotes, value);
        }

        public string CoolantNotes
        {
            get => _coolantNotes;
            set => SetProperty(ref _coolantNotes, value);
        }

        public string BatteryNotes
        {
            get => _batteryNotes;
            set => SetProperty(ref _batteryNotes, value);
        }

        public string OilLevelNotes
        {
            get => _oilLevelNotes;
            set => SetProperty(ref _oilLevelNotes, value);
        }

        public string AutoTransmissionFluidNotes
        {
            get => _autoTransmissionFluidNotes;
            set => SetProperty(ref _autoTransmissionFluidNotes, value);
        }

        public string PowerSteeringNotes
        {
            get => _powerSteeringNotes;
            set => SetProperty(ref _powerSteeringNotes, value);
        }
        #endregion
        
        public InteractionRequest<INotification> NotificationRequest { get; set; }

        #region Commands
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }
        #endregion
        #endregion
        
        #region Methods
        public PerformNewCheckViewModel(IDataService dataService, IDialogService dialogService, IRegionManager regionManager)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _regionManager = regionManager;

            NotificationRequest = new InteractionRequest<INotification>();

            SubmitText = "Submit";
            SubmitCommand = new DelegateCommand(SubmitExecute, CanSubmit);
            CancelCommand = new DelegateCommand(CancelExecute);
        }

        #region Command Handlers
        /// <summary>
        /// Take all of the variable equired to 'perform' a maintenance check and create a new instance, 
        /// before submiting it to the database.
        /// </summary>
        private void SubmitExecute()
        {
            if (!IsEditMode)
            {
                //Create a new maintenance check.
                _maintenanceCheck = new MaintenanceCheck
                {
                    DatePerformed = DateTime.Now,
                    MaintenanceCheckType = SelectedMaintenanceCheckType.Id,
                    VehicleId = _selectedVehicleId
                };
            }

            _maintenanceCheck.CheckedAirFilter = Convert.ToBoolean(CheckedAirFilter);
            _maintenanceCheck.ReplacedAirFilter = Convert.ToBoolean(ReplacedAirFilter);
            _maintenanceCheck.CheckCoolantLevels = Convert.ToBoolean(CheckCoolantLevels);
            _maintenanceCheck.FlushedSystemAndChangeCoolant = Convert.ToBoolean(FlushedSystemAndChangeCoolant);
            _maintenanceCheck.ChangeFanBelt = Convert.ToBoolean(ChangeFanBelt);
            _maintenanceCheck.CheckedBattery = Convert.ToBoolean(CheckedBattery);
            _maintenanceCheck.CheckedOilLevels = Convert.ToBoolean(CheckedOilLevels);
            _maintenanceCheck.ReplacedOilFilter = Convert.ToBoolean(ReplacedOilFilter);
            _maintenanceCheck.CheckAutoTransmissionFluid = Convert.ToBoolean(CheckAutoTransmissionFluid);
            _maintenanceCheck.AddedAutoTransmissionFluid = Convert.ToBoolean(AddedAutoTransmissionFluid);
            _maintenanceCheck.CheckPowerSteeringFluidLevels = Convert.ToBoolean(CheckPowerSteeringFluidLevels);

            //Check if the user has added any notes and add them to the maintenance check.
            if (!string.IsNullOrEmpty(AirFilterNotes))
            {
                _maintenanceCheck.AirFilterNotes = AirFilterNotes;
            }

            if (!string.IsNullOrEmpty(CoolantNotes))
            {
                _maintenanceCheck.CoolantNotes = CoolantNotes;
            }

            if (!string.IsNullOrEmpty(BatteryNotes))
            {
                _maintenanceCheck.BatteryNotes = BatteryNotes;
            }

            if (!string.IsNullOrEmpty(OilLevelNotes))
            {
                _maintenanceCheck.OilLevelNotes = OilLevelNotes;
            }

            if (!string.IsNullOrEmpty(AutoTransmissionFluidNotes))
            {
                _maintenanceCheck.AutoTransmissionFluidNotes = AutoTransmissionFluidNotes;
            }

            if (!string.IsNullOrEmpty(PowerSteeringNotes))
            {
                _maintenanceCheck.PowerSteeringNotes = PowerSteeringNotes;
            }

            if (IsEditMode)
            {
                _dataService.UpdateMaintenanceCheck((error) =>
                {
                    if (error != null)
                    {

                    }

                    NotificationRequest.Raise(new Notification { Title = NOTIFICATION_HEADER, Content = CHECK_UPDATED });
                    _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(Main).FullName);
                }, _maintenanceCheck);
            }
            else
            {
                //Add the new check to the database.
                _dataService.SubmitMaintenanceCheck((error) =>
                {
                    if (error != null)
                    {

                    }

                    NotificationRequest.Raise(new Notification { Title = NOTIFICATION_HEADER, Content = CHECK_COMPLETED });
                    _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(Main).FullName);
                }, _maintenanceCheck);
            }
        }

        /// <summary>
        /// Validate as to whethere the user can submit or not.
        /// </summary>
        /// <returns>True/False</returns>
        private bool CanSubmit()
        {
            //check if the user has actually done anything.
            if (!CheckedAirFilter && !ReplacedAirFilter && !CheckCoolantLevels && !FlushedSystemAndChangeCoolant
                 && !ChangeFanBelt && !CheckedBattery && !CheckedOilLevels && !ReplacedOilFilter && !CheckAutoTransmissionFluid
                 && !AddedAutoTransmissionFluid && !CheckPowerSteeringFluidLevels)
                return false;

            return true;
        }

        /// <summary>
        /// Cancel the check and return to the PerformChecks main view.
        /// </summary>
        private void CancelExecute()
        {
            //puts the Main View in the checksregion of the interface.
            _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(Main).FullName);
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //store the passed through bool of whether or not this is in edit mode.
            var isEditMode = navigationContext.Parameters["IsEditMode"];

            //check it is not null.
            if (isEditMode != null)
            {
                IsEditMode = Convert.ToBoolean(isEditMode);

                //store the passed through vehicle Id.
                var maintenanceCheckId = navigationContext.Parameters["MaintenanceCheckId"];

                //check it is not null.
                if (maintenanceCheckId != null)
                {
                    _dataService.GetMaintenanceChecksById((check, error) =>
                    {
                        if (error != null)
                        {

                        }

                        if (check != null)
                        {
                            _maintenanceCheck = check;
                            GetMaintenanceCheckType(Convert.ToInt32(_maintenanceCheck.MaintenanceCheckType));
                            SetProperties();
                        }
                    }, Convert.ToInt32(maintenanceCheckId));
                }
            }
            else
            {
                //from the parameters passed through the navigation, get the maintenance check type's ID the user selected.
                var selectedType = navigationContext.Parameters["SelectedTypeId"];

                //check it is not null.
                if (selectedType != null)
                {
                    GetMaintenanceCheckType(Convert.ToInt32(selectedType));
                }
            }

            //store the passed through vehicle Id.
            var selectedVehicleId = navigationContext.Parameters["SelectedVehicleId"];

            //check it is not null.
            if (selectedVehicleId != null)
            {
                _selectedVehicleId = Convert.ToInt32(selectedVehicleId);
            }
        }

        /// <summary>
        /// Check if this view model is the target one
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //store the passed through vehicle Id.
            var maintenanceCheckId = navigationContext.Parameters["MaintenanceCheckId"];

            //check it is not null and if there is a maintenance check already stored.
            if (maintenanceCheckId != null && _maintenanceCheck != null)
            {
                //see if they are the same and return a bool accordingly.
                if(Convert.ToInt32(maintenanceCheckId) == _maintenanceCheck.Id)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion
        
        private void SetProperties()
        {
            SubmitText = "Update";
            CheckedAirFilter = Convert.ToBoolean(_maintenanceCheck.CheckedAirFilter);
            ReplacedAirFilter = Convert.ToBoolean(_maintenanceCheck.ReplacedAirFilter);
            CheckCoolantLevels = Convert.ToBoolean(_maintenanceCheck.CheckCoolantLevels);
            FlushedSystemAndChangeCoolant = Convert.ToBoolean(_maintenanceCheck.FlushedSystemAndChangeCoolant);
            ChangeFanBelt = Convert.ToBoolean(_maintenanceCheck.ChangeFanBelt);
            CheckedBattery = Convert.ToBoolean(_maintenanceCheck.CheckedBattery);
            CheckedOilLevels = Convert.ToBoolean(_maintenanceCheck.CheckedOilLevels);
            ReplacedOilFilter = Convert.ToBoolean(_maintenanceCheck.ReplacedOilFilter);
            CheckAutoTransmissionFluid = Convert.ToBoolean(_maintenanceCheck.CheckAutoTransmissionFluid);
            AddedAutoTransmissionFluid = Convert.ToBoolean(_maintenanceCheck.AddedAutoTransmissionFluid);
            CheckPowerSteeringFluidLevels = Convert.ToBoolean(_maintenanceCheck.CheckPowerSteeringFluidLevels);

            //Check if the user has added any notes and add them to the maintenance check.
            if (!string.IsNullOrEmpty(_maintenanceCheck.AirFilterNotes))
            {
                AirFilterNotes = _maintenanceCheck.AirFilterNotes;
            }

            if (!string.IsNullOrEmpty(_maintenanceCheck.CoolantNotes))
            {
                CoolantNotes = _maintenanceCheck.CoolantNotes;
            }

            if (!string.IsNullOrEmpty(_maintenanceCheck.BatteryNotes))
            {
                BatteryNotes = _maintenanceCheck.BatteryNotes;
            }

            if (!string.IsNullOrEmpty(_maintenanceCheck.OilLevelNotes))
            {
                OilLevelNotes = _maintenanceCheck.OilLevelNotes;
            }

            if (!string.IsNullOrEmpty(_maintenanceCheck.AutoTransmissionFluidNotes))
            {
                AutoTransmissionFluidNotes = _maintenanceCheck.AutoTransmissionFluidNotes;
            }

            if (!string.IsNullOrEmpty(_maintenanceCheck.PowerSteeringNotes))
            {
                PowerSteeringNotes = _maintenanceCheck.PowerSteeringNotes;
            }
        }

        private void GetMaintenanceCheckType(int maintenanceCheckTypeId)
        {
            _dataService.GetMaintenanceCheckTypeById((type, error) =>
            {
                if (error != null)
                {

                }

                if (type == null)
                    return;

                //store it to the view model.
                SelectedMaintenanceCheckType = SelectedMaintenanceCheckType = type;
            }, maintenanceCheckTypeId);
        }
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace