//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model
{
    using ImprezGarage.Infrastructure.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class DataStorage
    {
        private ObservableCollection<Vehicle> _vehicles;
        private ObservableCollection<VehicleType> _vehicleTypes;
        private ObservableCollection<MaintenanceCheckType> _maintenanceTypes;

        internal DataStorage()
        {
            _vehicles = new ObservableCollection<Vehicle>();
            _vehicleTypes = new ObservableCollection<VehicleType>();
            _maintenanceTypes = new ObservableCollection<MaintenanceCheckType>();
        }

        internal ObservableCollection<VehicleType> GetVehicleTypes()
        {
            return _vehicleTypes;
        }

        internal void SetVehicleTypes(List<VehicleType> vehicleTypes)
        {
            _vehicleTypes = new ObservableCollection<VehicleType>(vehicleTypes);
        }

        internal ObservableCollection<MaintenanceCheckType> GetMaintenanceTypes()
        {
            return _maintenanceTypes;
        }

        internal void SetMaintenanceTypes(List<MaintenanceCheckType> maintenanceCheckType)
        {
            _maintenanceTypes = new ObservableCollection<MaintenanceCheckType>(maintenanceCheckType);
        }

        internal void SetVehicles(List<Vehicle> vehicles)
        {
            _vehicles = new ObservableCollection<Vehicle>(vehicles);
        }

        internal ObservableCollection<Vehicle> GetVehicles()
        {
            return _vehicles;
        }

        internal void AddNewVehicle(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
        }
    }
}   //ImprezGarage.Infrastructure.Model namespace 