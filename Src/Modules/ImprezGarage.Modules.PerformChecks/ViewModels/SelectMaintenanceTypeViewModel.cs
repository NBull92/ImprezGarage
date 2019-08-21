//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure.BaseClasses;
    using ImprezGarage.Infrastructure.Model;
    using Infrastructure.Services;
    using Prism.Commands;
    using System.Collections.ObjectModel;

    public class SelectMaintenanceTypeViewModel : DialogViewModelBase
    {
        private readonly IDataService _dataService;
        private MaintenanceCheckType _selectedMaintenanceCheckType;
        private ObservableCollection<MaintenanceCheckType> _maintenanceCheckTypes;

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

            LoadMaintenanceCheckTypes();
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
        private async void LoadMaintenanceCheckTypes()
        {
            var types = await _dataService.GetMaintenanceCheckTypesAsync();

            if (types == null)
                return;

            MaintenanceCheckTypes = new ObservableCollection<MaintenanceCheckType>(types);            
        }
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 