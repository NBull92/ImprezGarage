//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.ViewModels
{
    using ImprezGarage.Infrastructure.Dialogs;
    using Infrastructure.Model;
    using Prism.Commands;
    using Prism.Mvvm;
    using System;

    public class VehicleViewModel : BindableBase
    {
        public IDialogService DialogService;
        public IDataService DataService;

        internal Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get
            {
                return _vehicle;
            }
            set
            {
                if (value == _vehicle)
                    return;

                SetProperty(ref _vehicle, value);
            }
        }
        
        private VehicleType _vehicleType;
        public VehicleType VehicleType
        {
            get => _vehicleType;
            set
            {
                if (value == _vehicleType)
                    return;

                SetProperty(ref _vehicleType, value);
            }
        }

        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set
            {
                if (value == _dateCreated)
                    return;

                SetProperty(ref _dateCreated, value);
            }
        }

        private DateTime _dateModified;
        public DateTime DateModified
        {
            get => _dateModified;
            set
            {
                if (value == _dateModified)
                    return;

                _dateModified = value;
                RaisePropertyChanged("DateModified");
            }
        }

        private string _make;
        public string Make
        {
            get { return _make; }
            set
            {
                if (value == _make)
                    return;

                _make = value;
                RaisePropertyChanged("Make");
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                if (value == _model)
                    return;

                _model = value;
                RaisePropertyChanged("Model");
            }
        }

        private string _registration;
        public string Registration
        {
            get { return _registration; }
            set
            {
                if (value == _registration)
                    return;

                _registration = value;
                RaisePropertyChanged("Registration");
            }
        }

        private DateTime? _taxExpiryDate;
        public DateTime? TaxExpiryDate
        {
            get { return _taxExpiryDate; }
            set
            {
                if (value == _taxExpiryDate)
                    return;

                _taxExpiryDate = value;
                RaisePropertyChanged("TaxExpiryDate");
            }
        }

        private DateTime? _insuranceRenewalDate;
        public DateTime? InsuranceRenewalDate
        {
            get { return _insuranceRenewalDate; }
            set
            {
                if (value == _insuranceRenewalDate)
                    return;

                _insuranceRenewalDate = value;
                RaisePropertyChanged("InsuranceRenewalDate");
            }
        }

        public virtual DelegateCommand EditVehicleCommand { get; set; }
        public virtual DelegateCommand DeleteVehicleCommand { get; set; }

        public VehicleViewModel(IDialogService dialogService, IDataService dataService)
        {
            DialogService = dialogService;
            DataService = dataService;
        }

        public void LoadInstanceViaId(int selectedVehicleId)
        {
            DataService.GetVehicleByItsId((vehicle,error) => 
            {
                if(error != null)
                {

                }

                LoadInstance(vehicle);
            },selectedVehicleId);
        }

        /// <summary>
        /// Calls load instance to happen again but refresh the data from the vehicle in this view model.
        /// </summary>
        public virtual void Refresh()
        {
            LoadInstance();
        }
        
        /// <summary>
        /// Loads the data from the vehicle passed through are the one already assigned to this viewmodel.
        /// </summary>
        /// <param name="vehicle"></param>
        public void LoadInstance(Vehicle vehicle = null)
        {
            if (vehicle != null)
            {
                Vehicle = vehicle;
            }

            DateCreated = Vehicle.DateCreated;
            DateModified = Vehicle.DateModified;
            Make = Vehicle.Make;
            Model = Vehicle.Model;

            DataService.GetVehicleType((type,error) => 
            {
                if(error != null)
                {
                    return;
                }

                VehicleType = type;

                switch (VehicleType.Id)
                {
                    case 1:
                        Registration = Vehicle.Registration;
                        TaxExpiryDate = Vehicle.TaxExpiryDate;
                        InsuranceRenewalDate = Vehicle.InsuranceRenewalDate;
                        break;
                    case 2:
                        Registration = Vehicle.Registration;
                        TaxExpiryDate = Vehicle.TaxExpiryDate;
                        InsuranceRenewalDate = Vehicle.InsuranceRenewalDate;
                        break;
                    case 3:
                        break;
                }
            }, Vehicle.VehicleType);
        }
    }
}   //ImprezGarage.Infrastructure.ViewModels namespace 