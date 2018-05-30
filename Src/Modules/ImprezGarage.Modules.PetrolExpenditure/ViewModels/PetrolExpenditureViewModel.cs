//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.ViewModels;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Events;
    using Prism.Mvvm;
    using System.Collections.ObjectModel;

    public class PetrolExpenditureViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<PetrolExpenseViewModel> _expenses;

        public VehicleViewModel SelectedVehicle;
        #endregion

        #region Properties
        public ObservableCollection<PetrolExpenseViewModel> Expenses
        {
            get => _expenses;
            set => SetProperty(ref _expenses, value);
        }
        #endregion

        #region Methods
        public PetrolExpenditureViewModel(IDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
        }

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            SelectedVehicle = vehicleViewModel;
            GetSelectedVehiclePetrolExpenses();
        }

        private void GetSelectedVehiclePetrolExpenses()
        {
            if (SelectedVehicle == null)
            {
                Expenses = null;
            }
            else
            {
                _dataService.GetPetrolExpensesByVehicleId((expenses, error) =>
                {
                    if (error != null)
                    {

                    }

                    Expenses = new ObservableCollection<PetrolExpenseViewModel>();

                    foreach (var expense in expenses)
                    {
                        var viewModel = ServiceLocator.Current.GetInstance<PetrolExpenseViewModel>();
                        viewModel.LoadInstance(expense);
                        Expenses.Add(viewModel);
                    }
                }, SelectedVehicle.Vehicle.Id);
            }
        }
        #endregion
    }
}   // ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 