//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    
    public class SelectMaintenanceTypeViewModel : BindableBase
    {
        private readonly IDataService _dataService;
        private MaintenanceCheckType _selectedMaintenanceCheckType;
        private ObservableCollection<MaintenanceCheckType> _maintenanceCheckTypes;
        private bool _dialogResult;
        public event EventHandler ClosingRequest;

        #region Properties
        public MaintenanceCheckType SelectedMaintenanceCheckType
        {
            get => _selectedMaintenanceCheckType; 
            set
            {
                SetProperty(ref _selectedMaintenanceCheckType, value);
                OkayCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<MaintenanceCheckType> MaintenanceCheckTypes
        {
            get => _maintenanceCheckTypes; 
            set => SetProperty(ref _maintenanceCheckTypes, value);
        }

        public bool DialogResult
        {
            get => _dialogResult; 
            set => SetProperty(ref _dialogResult, value);
        }

        #region Commands
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand OkayCommand { get; }
        #endregion
        #endregion

        #region Methods
        public SelectMaintenanceTypeViewModel(IDataService dataService)
        {
            _dataService = dataService;

            CancelCommand = new DelegateCommand(CancelExecute);
            OkayCommand = new DelegateCommand(OkayExecute, CanProceed);

            LoadMaintenaceCheckTypes();
        }

        #region Command Handlers
        /// <summary>
        /// Close the window and set the result to true, as the user has selected a type.
        /// </summary>
        private void OkayExecute()
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Checks if the user has selected a maintenance type or not. If not they will not be able to proceed.
        /// </summary>
        private bool CanProceed()
        {
            return SelectedMaintenanceCheckType != null ? true : false;
        }

        /// <summary>
        /// Close the window and cancel the process.
        /// </summary>
        private void CancelExecute()
        {
            DialogResult = false;
            Close();
        }
        #endregion

        /// <summary>
        /// Load all the types from the data service.
        /// </summary>
        private void LoadMaintenaceCheckTypes()
        {
            var types = _dataService.GetMaintenanceCheckTypes();

            if (types == null || types.Result == null)
                return;

            MaintenanceCheckTypes = new ObservableCollection<MaintenanceCheckType>(types.Result);            
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
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 