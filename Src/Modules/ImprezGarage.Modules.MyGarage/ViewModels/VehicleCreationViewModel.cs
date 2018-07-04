//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using Infrastructure.Services;
    using Prism.Mvvm;
    using System;
    using System.ComponentModel;

    public class VehicleCreationViewModel : BindableBase, IDataErrorInfo
    {
        #region Attributes
        private string _make;
        private string _model;
        private VehicleType _vehicleType;
        private DateTime _dateCreated;
        private DateTime _dateModified;
        #endregion

        #region Properties
        internal AddVehicleViewModel AddVehicleVm { get; set; }

        public int Id { get; set; }

        public string Make
        {
            get => _make;
            set => SetProperty(ref _make, value);
        }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public VehicleType VehicleType
        {
            get => _vehicleType;
            set => SetProperty(ref _vehicleType, value);
        }

        public DateTime DateCreated
        {
            get => _dateCreated;
            set => SetProperty(ref _dateCreated, value);
        }

        public DateTime DateModified
        {
            get => _dateModified;
            set => SetProperty(ref _dateModified, value);
        }

        #region IDataErrorInfo
        public string Error => throw new NotImplementedException();

        public virtual string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case "VehicleType":
                        if (_vehicleType == null)
                        {
                            error = "There is no associated vechile type selected.";
                        }
                        break;
                    case "Model":
                        if (string.IsNullOrEmpty(_model))
                        {
                            error = "Please indicate the model of this vehicle.";
                        }
                        break;
                    case "Make":
                        if (string.IsNullOrEmpty(_make))
                        {
                            error = "Please indicate the make of this vehicle.";
                        }
                        break;
                }

                return (error);
            }
        }
        #endregion
        #endregion

        #region Methods
        public VehicleCreationViewModel()
        {
        }

        internal virtual void CleanUp()
        {
            VehicleType = null;
            Model = null;
            Make = null;
            DateCreated = new DateTime();
            DateModified = new DateTime();
        }
        #endregion
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 