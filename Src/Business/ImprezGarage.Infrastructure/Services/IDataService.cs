//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using ImprezGarage.Infrastructure.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public interface IDataService
    {
        #region Gets
        /// <summary>
        /// Retrieve all of the saved vehicles
        /// </summary>
        /// <returns>An observable collection of all vehicles</returns>
        Task<ObservableCollection<Vehicle>> GetVehicles(bool refresh = false);

        /// <summary>
        /// Retrieve the vehicles from the database and find the vehicle with the Id passed through.
        /// </summary>
        Task<Vehicle> GetVehicleByItsId(int vehicleId);

        /// <summary>
        /// This function returns a collection of all the vehicle types from teh database.
        /// </summary>
        /// <returns>An observable collection of all vehicle types</returns>
        Task<ObservableCollection<VehicleType>> GetVehicleTypes(bool refresh = false);

        /// <summary>
        /// This returns the appropriate vehicle type associated with the pass through Id.
        /// </summary>
        Task<VehicleType> GetVehicleType(int typeId);

        /// <summary>
        /// Retrieve all of the Maintenance Check Types from the database.
        /// </summary>
        Task<ObservableCollection<MaintenanceCheckType>> GetMaintenanceCheckTypes();

        /// <summary>
        /// Retrieve a specific Maintenance Check Type by it's Id.
        /// </summary>
        Task<MaintenanceCheckType> GetMaintenanceCheckTypeById(int typeId);

        /// <summary>
        /// Retrieve all of the maintenance checks performed on a specific vehicle, based off of it's Id.
        /// </summary>
        Task<List<MaintenanceCheck>> GetMaintenanceChecksForVehicleByVehicleId(int vehicleId);

        /// <summary>
        /// Retrieve a specific maintenance check.
        /// </summary>
        Task<MaintenanceCheck> GetMaintenanceChecksById(int maintenanceCheckId);

        /// <summary>
        /// Return a collection of all fo the petrol expenses for a specific vehicle.
        /// </summary>
        Task<ObservableCollection<PetrolExpense>> GetPetrolExpensesByVehicleId(int vehicleId);

        /// <summary>
        /// Get the last time a specific vehicle had a maintenance check.
        /// </summary>
        Task<DateTime?> GetLastMaintenanceCheckDateForVehicleByVehicleId(int vehicleId);
        #endregion

        #region Adds
        /// <summary>
        /// Add new vehicle to the database
        /// </summary>
        void AddNewVehicle(Action<Exception> callback, Vehicle vehicle);

        /// <summary>
        /// Save the passed through maintenance check to the database.
        /// </summary>
        void SubmitMaintenanceCheck(Action<Exception> callback, MaintenanceCheck maintenanceCheck);

        /// <summary>
        /// Add a new petrol expense to the database.
        /// </summary>
        void AddPetrolExpenditure(Action<Exception> callback, double amount, int vehicleId);
        #endregion

        #region Deletes
        /// <summary>
        /// Deletes the passed through object.
        /// </summary>
        void DeleteVehicle(Action<Exception> callback, Vehicle vehicle);

        /// <summary>
        /// Delete a maintenance check from the database
        /// </summary>
        void DeleteMaintenanceCheck(Action<Exception> callback, int maintenanceCheckId);

        /// <summary>
        /// Delete a petrol expense from the database.
        /// </summary>
        void DeletePetrolExpense(Action<Exception> callback, int petrolExpenseId);
        #endregion

        #region Updates
        /// <summary>
        /// This function will take the data from the passed through vehicle and update it's entity in the database.
        /// </summary>
        /// <param name="callback">sends back whether there was an error during the save or not.</param>
        void UpdateVehicle(Action<Exception> callback, Vehicle vehicle);

        /// <summary>
        /// Update a maintenance check in the database with new information.
        /// </summary>
        void UpdateMaintenanceCheck(Action<Exception> callback, MaintenanceCheck maintenanceCheck);
        #endregion
    }
}   //ImprezGarage.Infrastructure.Model namespace 