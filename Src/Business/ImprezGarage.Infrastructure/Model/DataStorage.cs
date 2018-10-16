//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model
{
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// A storage of all of the data retrieved from the database.
    /// </summary>
    internal class DataStorage
    {
        #region Attributes
        /// <summary>
        /// Store all of the vehicles
        /// </summary>
        private ObservableCollection<Vehicle> _vehicles;
        /// <summary>
        /// Store all of the vehicle types
        /// </summary>
        private ObservableCollection<VehicleType> _vehicleTypes;

        /// <summary>
        /// Store all of the maintenance types
        /// </summary>
        private ObservableCollection<MaintenanceCheckType> _maintenanceTypes;
        #endregion

        #region Methods
        /// <summary>
        /// Construct the data model and instantiate the collections.
        /// </summary>
        internal DataStorage()
        {
            _vehicles = new ObservableCollection<Vehicle>();
            _vehicleTypes = new ObservableCollection<VehicleType>();
            _maintenanceTypes = new ObservableCollection<MaintenanceCheckType>();
        }

        /// <summary>
        /// return the list of vehicle types.
        /// </summary>
        internal ObservableCollection<VehicleType> GetVehicleTypes()
        {
            return _vehicleTypes;
        }

        /// <summary>
        /// Set the collection of vehicle types to the passed through list.
        /// </summary>
        internal void SetVehicleTypes(List<VehicleType> vehicleTypes)
        {
            _vehicleTypes = new ObservableCollection<VehicleType>(vehicleTypes);
        }

        /// <summary>
        /// Return the collection of maintenance types.
        /// </summary>
        internal ObservableCollection<MaintenanceCheckType> GetMaintenanceTypes()
        {
            return _maintenanceTypes;
        }

        /// <summary>
        /// Set the collection of maintenance types to the passed through list.
        /// </summary>
        internal void SetMaintenanceTypes(List<MaintenanceCheckType> maintenanceCheckType)
        {
            _maintenanceTypes = new ObservableCollection<MaintenanceCheckType>(maintenanceCheckType);
        }

        /// <summary>
        /// Set the collection of vehicle to the passed through list.
        /// </summary>
        internal void SetVehicles(List<Vehicle> vehicles)
        {
            _vehicles = new ObservableCollection<Vehicle>(vehicles);
        }

        /// <summary>
        /// Return the collection of vehicles.
        /// </summary>
        /// <returns></returns>
        internal ObservableCollection<Vehicle> GetVehicles()
        {
            return _vehicles;
        }

        /// <summary>
        /// Add a new vehicle to the collection of vehicles.
        /// </summary>
        internal void AddNewVehicle(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
        }
        #endregion
    }
}   //ImprezGarage.Infrastructure.Model namespace 