//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

using System.Linq;

namespace ImprezGarage.Infrastructure.Model
{
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// A storage of all of the data retrieved from the database.
    /// </summary>
    public class DataStorage : IRepository
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
        public DataStorage()
        {
            _vehicles = new ObservableCollection<Vehicle>();
            _vehicleTypes = new ObservableCollection<VehicleType>();
            _maintenanceTypes = new ObservableCollection<MaintenanceCheckType>();
        }

        /// <summary>
        /// return the list of vehicle types.
        /// </summary>
        public List<VehicleType> GetVehicleTypes()
        {
            return _vehicleTypes.ToList();
        }

        /// <summary>
        /// Set the collection of vehicle types to the passed through list.
        /// </summary>
        public void SetVehicleTypes(List<VehicleType> vehicleTypes)
        {
            _vehicleTypes = new ObservableCollection<VehicleType>(vehicleTypes);
        }

        /// <summary>
        /// Return the collection of maintenance types.
        /// </summary>
        public ObservableCollection<MaintenanceCheckType> GetMaintenanceTypes()
        {
            return _maintenanceTypes;
        }

        /// <summary>
        /// Set the collection of maintenance types to the passed through list.
        /// </summary>
        public void SetMaintenanceTypes(List<MaintenanceCheckType> maintenanceCheckType)
        {
            _maintenanceTypes = new ObservableCollection<MaintenanceCheckType>(maintenanceCheckType);
        }

        /// <summary>
        /// Set the collection of vehicle to the passed through list.
        /// </summary>
        public void SetVehicles(List<Vehicle> vehicles)
        {
            _vehicles = new ObservableCollection<Vehicle>(vehicles);
        }

        /// <summary>
        /// Return the collection of vehicles.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Vehicle> GetVehicles()
        {
            return _vehicles;
        }

        /// <summary>
        /// Add a new vehicle to the collection of vehicles.
        /// </summary>
        public void AddNewVehicle(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
        }
        #endregion
    }
}   //ImprezGarage.Infrastructure.Model namespace 