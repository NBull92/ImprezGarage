//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure.Model;
    using Prism.Commands;
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    
    public class SelectMaintenanceTypeViewModel : BindableBase
    {
        private IDataService _dataService;
        public event EventHandler ClosingRequest;

        private MaintenanceCheckType _selectedMaintenanceCheckType;
        public MaintenanceCheckType SelectedMaintenanceCheckType
        {
            get => _selectedMaintenanceCheckType; 
            set
            {
                _selectedMaintenanceCheckType = value;
                RaisePropertyChanged("SelectedMaintenanceCheckType");
                OkayCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<MaintenanceCheckType> _maintenanceCheckTypes;
        public ObservableCollection<MaintenanceCheckType> MaintenanceCheckTypes
        {
            get => _maintenanceCheckTypes; 
            set
            {
                _maintenanceCheckTypes = value;
                RaisePropertyChanged("MaintenanceCheckTypes");
            }
        }

        private bool _dialogResult;
        public bool DialogResult
        {
            get => _dialogResult; 
            set
            {
                _dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }

        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand OkayCommand { get; private set; }
        
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
            _dataService.GetMaintenanceCheckTypes((types, error) =>
            {
                if (error != null)
                {
                    return;
                }

                MaintenanceCheckTypes = new ObservableCollection<MaintenanceCheckType>(types);
            });
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        private void Close()
        {
            if (this.ClosingRequest != null)
            {
                this.ClosingRequest(this, EventArgs.Empty);
            }
        }
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 