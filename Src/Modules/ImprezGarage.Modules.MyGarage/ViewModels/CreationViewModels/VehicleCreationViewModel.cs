﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels.CreationViewModels
{
    using Infrastructure.Model;
    using Prism.Mvvm;
    using System;
    using System.ComponentModel;

    public class VehicleCreationViewModel : BindableBase, IDataErrorInfo, IDisposable
    {
        #region Attributes
        private string _make;
        private string _model;
        private VehicleType _vehicleType;
        private DateTime _dateCreated;
        private DateTime _dateModified;

        /// <summary>
        /// Bool to decide whether to show the extra content that can be added when managing a vehicle.
        /// </summary>
        private bool _showEditView;
        #endregion

        #region Properties
        internal ManageVehicleViewModel AddVehicleVm { get; set; }

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

        /// <summary>
        /// Bool to decide whether to show the extra content that can be added when managing a vehicle.
        /// </summary>
        public bool ShowEditView
        {
            get => _showEditView;
            set => SetProperty(ref _showEditView, value);
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
                            error = "There is no associated vehicle type selected.";
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

        public virtual void Setup(VehicleType vehicleType)
        {

        }

        public virtual void SaveNew(Vehicle newVehicle)
        {
            
        }

        public virtual void Update(Vehicle vehicle)
        {
            
        }

        public virtual bool CanSave()
        {
            return true;
        }

        public virtual void EditInitialise(Vehicle vehicle)
        {
            
        }

        public virtual void CleanUp()
        {
            
        }

        public void Dispose()
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