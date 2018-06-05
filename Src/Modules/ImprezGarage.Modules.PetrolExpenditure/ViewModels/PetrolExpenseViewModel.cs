//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Dialogs;
    using ImprezGarage.Infrastructure.Model;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;

    public class PetrolExpenseViewModel : BindableBase
	{
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private const string CONFIRM_EXPENSE_DELETE = "Are you sure you wish to delete this petrol expense?";
        private int _id;
        private int _vehicleId;
        private double _amount;
        private DateTime _dateEntered;

        public DelegateCommand DeleteExpenseCommand { get; set; }
        #endregion

        #region Properties
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public int VehicleId
        {
            get => _vehicleId;
            set => SetProperty(ref _vehicleId, value);
        }
        public double Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
        public DateTime DateEntered
        {
            get => _dateEntered;
            set => SetProperty(ref _dateEntered, value);
        }
        #endregion

        #region Methods

        public PetrolExpenseViewModel(IDataService dataService, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            DeleteExpenseCommand = new DelegateCommand(OnExpenseDeleted);
        }

        public void LoadInstance(PetrolExpense petrolExpense)
        {
            Id = petrolExpense.Id;
            Amount = Convert.ToDouble(petrolExpense.Amount);
            VehicleId = Convert.ToInt32(petrolExpense.VehicleId);
            DateEntered = Convert.ToDateTime(petrolExpense.DateEntered);
        }

        private void OnExpenseDeleted()
        {
            if (!_dialogService.Confirm(CONFIRM_EXPENSE_DELETE))
                return;

            _dataService.DeletePetrolExpense((error) => 
            {
                if(error != null)
                {

                }

                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
            }, Id);
        }

        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 