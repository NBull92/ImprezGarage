//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using Infrastructure;
    using Infrastructure.Model.Temp_New_Classes;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Views;

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
        private ObservableCollection<Option> _maintenanceOptionsPerformed;
        #endregion

        #region Properties
        public MaintenanceCheckType SelectedMaintenanceCheckType
        {
            get => _selectedMaintenanceCheckType;
            set => SetProperty(ref _selectedMaintenanceCheckType, value);
        }

        public ObservableCollection<Option> MaintenanceOptionsPerformed
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
        /// Take all of the variables required to 'perform' a maintenance check and create a new instance, 
        /// before submitting it to the database.
        /// </summary>
        private async void SubmitExecute()
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
                try
                {
                    _dataService.SetMaintenanceCheckAsync(_maintenanceCheck);
                    
                    var performedChecks = new List<PerformedMaintenanceOption>();

                    foreach (var option in MaintenanceOptionsPerformed)
                    {
                        var performedOption = new PerformedMaintenanceOption
                        {
                            IsChecked = option.IsChecked,
                            Id = option.Id,
                            Notes = option.Notes,
                            MaintenanceOption = option.MaintenanceOption.Id,
                            MaintenanceCheck = _maintenanceCheck.Id
                        };
                        performedChecks.Add(performedOption);
                    }

                    await _dataService.SetOptionsPerformedAsync(performedChecks);

                    _notificationsService.Alert(CheckUpdated, NotificationHeader);
                    _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(Main).FullName, new NavigationParameters { { "Refresh", true } });
                }
                catch (Exception e)
                {
                    _loggerService.LogException(e);
                }
            }
            else
            {
                try
                {
                    var returnedId = await _dataService.SetMaintenanceCheckAsync(_maintenanceCheck);

                    var performedChecks = new List<PerformedMaintenanceOption>();

                    foreach (var option in MaintenanceOptionsPerformed.Where(o => o.IsChecked == true))
                    {
                        var performedOption = new PerformedMaintenanceOption
                        {
                            IsChecked = option.IsChecked,
                            Notes = option.Notes,
                            MaintenanceOption = option.MaintenanceOption.Id,
                            MaintenanceCheck = returnedId
                        };
                        performedChecks.Add(performedOption);
                    }

                    await _dataService.SetOptionsPerformedAsync(performedChecks);

                    _notificationsService.Alert(CheckCompleted, NotificationHeader);
                    _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(Main).FullName, new NavigationParameters { { "Refresh", true } });
                }
                catch (Exception e)
                {
                    _loggerService.LogException(e);
                }
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
            MaintenanceOptionsPerformed = new ObservableCollection<Option>();
            SelectedMaintenanceCheckType = null;

            var type = _dataService.GetMaintenanceCheckTypeById(maintenanceCheckTypeId);

            if (type == null)
                return;

            SelectedMaintenanceCheckType = type;

            GetMaintenanceCheckOptions();
        }

        /// <summary>
        /// Retrieve all of the current Options associated with the selected maintenance type and make a list of OptionsPerformed, based off of these Options.
        /// </summary>
        private async void GetMaintenanceCheckOptions()
        {
            var options = _dataService.GetMaintenanceCheckOptionsForType(SelectedMaintenanceCheckType.Id);

            if (options?.Result == null)
                return;

            if (_maintenanceCheck != null)
            {
                var currentOptionsSelected = await _dataService.GetOptionsPerformedAsync(_maintenanceCheck.Id);
                var performedMaintenanceOptions = currentOptionsSelected.ToList();

                foreach (var option in options.Result)
                {
                    var currentState = performedMaintenanceOptions.FirstOrDefault(o => o.MaintenanceOption == option.Id);
                    var optionPerformed = currentState == null ? new Option(option) : new Option(option, currentState);
                    MaintenanceOptionsPerformed.Add(optionPerformed);
                }
            }
            else
            {
                foreach (var option in options.Result)
                {
                    var optionPerformed = new Option(option);
                    MaintenanceOptionsPerformed.Add(optionPerformed);
                }
            }
        }

        #region INavigationAware
        public async void OnNavigatedTo(NavigationContext navigationContext)
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
                    var check = await _dataService.GetMaintenanceChecksByIdAsync(Convert.ToInt32(maintenanceCheckId));

                    if (check == null)
                        return;

                    _maintenanceCheck = check;
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