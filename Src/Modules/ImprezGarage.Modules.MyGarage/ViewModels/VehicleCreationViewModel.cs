//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using Prism.Commands;
    using Prism.Mvvm;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Model;
    using System.ComponentModel;

    public class VehicleCreationViewModel : BindableBase, IDataErrorInfo
    {
        internal AddVehicleViewModel AddVehicleVm { get; set; }

        public int Id { get; set; }

        private string _make;
        public string Make
        {
            get => _make;
            set
            {
                if (value == _make)
                    return;

                _make = value;
                RaisePropertyChanged("Make");
            }
        }

        private string _model { get; set; }
        public string Model
        {
            get => _model;
            set
            {
                if (value == _model)
                    return;

                _model = value;
                RaisePropertyChanged("Model");
            }
        }

        private VehicleType _vehicleType { get; set; }
        public VehicleType VehicleType
        {
            get => _vehicleType;
            set
            {
                if (value == _vehicleType)
                    return;

                _vehicleType = value;
                RaisePropertyChanged("VehicleType");
            }
        }

        private DateTime _dateCreated { get; set; }
        public DateTime DateCreated
        {
            get => _dateCreated;
            set
            {
                if (value == _dateCreated)
                    return;

                _dateCreated = value;
                RaisePropertyChanged("DateCreated");
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

        public string Error => throw new NotImplementedException();
        
        public virtual string this[string columnName]
        {
            get
            {
                string error = null;

                switch(columnName)
                {
                    case "VehicleType":
                        if(_vehicleType == null)
                        {
                            error = "There is no associated vechile type selected.";
                        }
                        break;
                    case "Model":
                        if(string.IsNullOrEmpty(_model))
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
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 