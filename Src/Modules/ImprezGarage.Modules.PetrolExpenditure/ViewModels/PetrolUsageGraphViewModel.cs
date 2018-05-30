﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.ViewModels;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class PetrolUsageGraphViewModel : BindableBase
    {
        #region Attributes
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<ChartData> _expenses;
        private ObservableCollection<ChartData> _filteredExpenses;
        private string _minDate;
        private string _maxDate;
        private double _minCost;
        private double _maxCost;
        private bool _isLineChart;

        public VehicleViewModel SelectedVehicle;

        #region Commands
        public DelegateCommand<string> OnChartDisplayChangeCommand { get; set; }
        #endregion
        #endregion

        #region Properties
        public ObservableCollection<ChartData> FilteredExpenses
        {
            get => _filteredExpenses;
            set => SetProperty(ref _filteredExpenses, value);
        }

        public string MinDate
        {
            get => _minDate;
            set
            {
                SetProperty(ref _minDate, value);
                FilterExpenses();
            }
        }

        public string MaxDate
        {
            get => _maxDate;
            set => SetProperty(ref _maxDate, value);
        }

        public double MinCost
        {
            get => _minCost;
            set => SetProperty(ref _minCost, value);
        }

        public double MaxCost
        {
            get => _maxCost;
            set => SetProperty(ref _maxCost, value);
        }

        public bool IsLineChart
        {
            get => _isLineChart;
            set
            {
                SetProperty(ref _isLineChart, value);
                ChartStyleContent = _isLineChart ? "Line" : "Bar";
            }
        }

        public string _chartStyleContent;
        public string ChartStyleContent
        {
            get => _chartStyleContent;
            set => SetProperty(ref _chartStyleContent, value);
        }
        #endregion

        #region Methods
        public PetrolUsageGraphViewModel(IDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);

            OnChartDisplayChangeCommand = new DelegateCommand<string>(OnChartDisplayChangeExecute);
            ResetParameters();
        }

        private void ResetParameters()
        {
            FilteredExpenses = new ObservableCollection<ChartData>();
            _expenses = new ObservableCollection<ChartData>();
            MaxCost = 1;
            MinCost = 1;
            IsLineChart = true;
            ChartStyleContent = "Line";
            MinDate = DateTime.Now.AddDays(-7).ToShortDateString();
            MaxDate = DateTime.Now.ToShortDateString();
        }

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            ResetParameters();
            SelectedVehicle = vehicleViewModel;
            GetSelectedVehiclePetrolExpenses();
        }

        private void GetSelectedVehiclePetrolExpenses()
        {
            if (SelectedVehicle == null)
            {
                _expenses = null;
                FilteredExpenses = null;
            }
            else
            {
                _dataService.GetPetrolExpensesByVehicleId((expenses, error) =>
                {
                    if (error != null)
                    {

                    }

                    _expenses = new ObservableCollection<ChartData>();
                    FilteredExpenses = new ObservableCollection<ChartData>();

                    if (expenses == null)
                        return;

                    foreach (var expense in expenses)
                    {
                        MaxCost = MaxCost < expense.Amount ? Convert.ToDouble(expense.Amount) : MaxCost;
                        _expenses.Add(new ChartData()
                        {
                            Cost = Convert.ToDouble(expense.Amount),
                            Date = expense.DateEntered.Value.ToShortDateString()
                        });
                    }

                    FilterExpenses();

                }, SelectedVehicle.Vehicle.Id);
            }
        }

        private void FilterExpenses()
        {
            if (_expenses == null)
                return;

            FilteredExpenses = new ObservableCollection<ChartData>(_expenses.Where(o => Convert.ToDateTime(o.Date).Date >= Convert.ToDateTime(MinDate).Date
                                                    && Convert.ToDateTime(o.Date).Date <= Convert.ToDateTime(MaxDate).Date));
        }

        #region Command Handlers
        private void OnChartDisplayChangeExecute(string chartDisplay)
        {
            switch(chartDisplay)
            {
                case "Week":
                    MinDate = DateTime.Now.AddDays(-7).ToShortDateString();
                    break;
                case "Month":
                    MinDate = DateTime.Now.AddMonths(-1).ToShortDateString();
                    break;
                case "Six Months":
                    MinDate = DateTime.Now.AddMonths(-6).ToShortDateString();
                    break;
                case "Year":
                    MinDate = DateTime.Now.AddYears(-1).ToShortDateString();
                    break;
                default:
                    MinDate = "01/01/01";
                    break;
            }
        }
        #endregion
        #endregion
    }

    public class ChartData
    {
        public double Cost { get; set; }
        public string Date { get; set; }
    }
}   //ImprezGarage.Modules.PetrolExpenditure.ViewModels namespace 