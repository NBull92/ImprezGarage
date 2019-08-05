//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------


namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using Infrastructure.Model.Temp_New_Classes;
    using Infrastructure;
    using Infrastructure.Services;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;

    public class PetrolExpenseViewModel : BindableBase
	{
        #region Attributes
        private readonly IDataService _dataService;
        private readonly INotificationsService _notificationsService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILoggerService _loggerService;

        private const string ConfirmExpenseDelete = "Are you sure you wish to delete this petrol expense?";
        private int _id;
        private int _vehicleId;
        private double _amount;
        private DateTime _dateEntered;
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

        #region Command
        public DelegateCommand DeleteExpenseCommand { get; set; }
        #endregion
        #endregion

        #region Methods

        public PetrolExpenseViewModel(IDataService dataService, INotificationsService notificationsService, IEventAggregator eventAggregator, 
            ILoggerService loggerService)
        {
            _dataService = dataService;
            _notificationsService = notificationsService;
            _eventAggregator = eventAggregator;
            _loggerService = loggerService;

            DeleteExpenseCommand = new DelegateCommand(OnExpenseDeleted);
        }

        public void LoadInstance(PetrolExpense petrolExpense)
        {
            Id = petrolExpense.Id;
            Amount = Convert.ToDouble(petrolExpense.Amount);
            VehicleId = Convert.ToInt32(petrolExpense.VehicleId);
            DateEntered = Convert.ToDateTime(petrolExpense.DateEntered);
        }

        #region Command Handler
        private void OnExpenseDeleted()
        {
            if (!_notificationsService.Confirm(ConfirmExpenseDelete))
                return;

            try
            {
                _dataService.DeletePetrolExpense(Id);
                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
            }
            catch (Exception e)
            {
                _loggerService.LogException(e);
            }
        }
        #endregion
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 