//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using Infrastructure;
    using Infrastructure.Services;
    using Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using ImprezGarage.Infrastructure.Model;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Prism.Events;

    public class PerformNewCheckViewModel : BindableBase, INavigationAware
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IRegionManager _regionManager;
        private readonly INotificationsService _notificationsService;
        private readonly ILoggerService _loggerService;
        private readonly IEventAggregator _eventAggregator;
        private static int _selectedVehicleId;
        private MaintenanceCheck _maintenanceCheck;
        private const string CheckCompleted = "Maintenance check completed!";
        private const string CheckUpdated = "Maintenance check updated!";
        private const string NotificationHeader = "Alert!";
        private MaintenanceCheckType _selectedMaintenanceCheckType;
        private bool _isEditMode;
        private string _submitText;        
        private ObservableCollection<MaintenanceOptionsPerformed> _maintenanceOptionsPerformed;
        #endregion

        #region Properties
        public MaintenanceCheckType SelectedMaintenanceCheckType
        {
            get => _selectedMaintenanceCheckType;
            set => SetProperty(ref _selectedMaintenanceCheckType, value);
        }

        public ObservableCollection<MaintenanceOptionsPerformed> MaintenanceOptionsPerformed
        {
            get => _maintenanceOptionsPerformed;
            set => SetProperty(ref _maintenanceOptionsPerformed, value);
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
        
        #region Commands
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SubmitCommand { get; set; }
        #endregion
        #endregion
        
        #region Methods
        public PerformNewCheckViewModel(IDataService dataService, INotificationsService notificationsService, IRegionManager regionManager,
            ILoggerService loggerService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _notificationsService = notificationsService;
            _regionManager = regionManager;
            _loggerService = loggerService;
            _eventAggregator = eventAggregator;

            SubmitText = "Submit";
            SubmitCommand = new DelegateCommand(SubmitExecute);
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

            if (IsEditMode)
            {
                _dataService.UpdateMaintenanceCheck((error) =>
                {
                    if (error != null)
                    {
                        _loggerService.LogException(error);
                        return;
                    }

                    foreach (var option in MaintenanceOptionsPerformed)
                    {
                        option.MaintenanceCheck = _maintenanceCheck.Id;
                    }

                    _dataService.UpdateOptionsPerformed((optionsError) =>
                    {
                        if (optionsError != null)
                        {
                            _loggerService.LogException(optionsError);
                            return;
                        }

                        _notificationsService.Alert(CheckUpdated, NotificationHeader);
                        _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(Main).FullName, new NavigationParameters { { "Refresh", true } });
                    }, MaintenanceOptionsPerformed);
                }, _maintenanceCheck);
            }
            else
            {
                //Add the new check to the database.
                _dataService.SubmitMaintenanceCheck((error) =>
                {
                    if (error != null)
                    {
                        _loggerService.LogException(error);
                        return;
                    }

                    foreach (var option in MaintenanceOptionsPerformed)
                    {
                        option.MaintenanceCheck = _maintenanceCheck.Id;
                    }

                    _dataService.UpdateOptionsPerformed((optionsError) =>
                    {
                        if (optionsError != null)
                        {
                            _loggerService.LogException(optionsError);
                            return;
                        }

                        _notificationsService.Alert(CheckCompleted, NotificationHeader);
                        _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(Main).FullName, new NavigationParameters { { "Refresh", true } });
                    }, MaintenanceOptionsPerformed);
                }, _maintenanceCheck);
            }
        }

        /// <summary>
        /// Validate as to whethere the user can submit or not.
        /// </summary>
        /// <returns>True/False</returns>
        private bool CanSubmit()
        {
            if (MaintenanceOptionsPerformed == null)
                return false;

            //check if the user has actually done anything.
            return MaintenanceOptionsPerformed.Any(o => o.IsChecked == true);
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
        
        private void SetProperties()
        {
            SubmitText = "Update";            
        }

        private void GetMaintenanceCheckType(int maintenanceCheckTypeId)
        {
            MaintenanceOptionsPerformed = new ObservableCollection<MaintenanceOptionsPerformed>();
            SelectedMaintenanceCheckType = null;

            var type = _dataService.GetMaintenanceCheckTypeById(maintenanceCheckTypeId);

            if (type == null || type.Result == null)
                return;

            SelectedMaintenanceCheckType = SelectedMaintenanceCheckType = type.Result;

            GetMaintenanceCheckOptions();
        }

        /// <summary>
        /// Retrieve all of the current Options assocaited with the selected maintenance type and make a list of OptionsPerformed, based off of these Options.
        /// </summary>
        private void GetMaintenanceCheckOptions()
        {
            var options = _dataService.GetMaintenanceCheckOptionsForType(SelectedMaintenanceCheckType.Id);

            if (options == null || options.Result == null)
                return;

            foreach (var option in options.Result)
            {
                MaintenanceOptionsPerformed optionPerformed = null;

                if(IsEditMode && _maintenanceCheck != null)
                {
                    var currentState = _dataService.GetOptionPerformedCurrentState(_maintenanceCheck.Id, option.Id);

                    if (currentState == null || currentState.Result == null)
                    {
                        optionPerformed = new MaintenanceOptionsPerformed
                        {
                            MaintenanceCheckOption = option,
                            IsChecked = false
                        };
                    }
                    else
                    {
                        optionPerformed = currentState.Result;
                        optionPerformed.MaintenanceCheckOption = option;
                    }
                }
                else
                {
                    optionPerformed = new MaintenanceOptionsPerformed
                    {
                        MaintenanceCheckOption = option,
                        IsChecked = false
                    };
                }

                MaintenanceOptionsPerformed.Add(optionPerformed);
            }
        }

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
                    var check = _dataService.GetMaintenanceChecksById(Convert.ToInt32(maintenanceCheckId));

                    if (check == null || check.Result == null)
                        return;

                    _maintenanceCheck = check.Result;
                    GetMaintenanceCheckType(Convert.ToInt32(_maintenanceCheck.MaintenanceCheckType));
                    SetProperties();
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
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace