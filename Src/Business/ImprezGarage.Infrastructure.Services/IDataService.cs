//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataService
    {
        #region Gets
        /// <summary>
        /// Retrieve all of the saved vehicles
        /// </summary>
        /// <returns>An observable collection of all vehicles</returns>
        Task<IEnumerable<Vehicle>> GetVehicles(bool refresh = false);
        
        /// <summary>
        /// This function returns a collection of all the vehicle types from teh database.
        /// </summary>
        /// <returns>An observable collection of all vehicle types</returns>
        Task<IEnumerable<VehicleType>> GetVehicleTypesAsync(bool refresh = false);

        /// <summary>
        /// This returns the appropriate vehicle type associated with the pass through Id.
        /// </summary>
        Task<VehicleType> GetVehicleTypeAsync(int typeId);

        /// <summary>
        /// Retrieve all of the Maintenance Check Types from the database.
        /// </summary>
        Task<IEnumerable<MaintenanceCheckType>> GetMaintenanceCheckTypesAsync();

        /// <summary>
        /// Retrieve a specific Maintenance Check Type by it's Id.
        /// </summary>
        MaintenanceCheckType GetMaintenanceCheckTypeById(int typeId);

        /// <summary>
        /// Retrieve all of the maintenance checks performed on a specific vehicle, based off of it's Id.
        /// </summary>
        IEnumerable<MaintenanceCheck> GetVehicleMaintenanceChecks(int vehicleId);

        /// <summary>
        /// Retrieve a specific maintenance check.
        /// </summary>
        Task<MaintenanceCheck> GetMaintenanceChecksByIdAsync(int maintenanceCheckId);

        /// <summary>
        /// Return a collection of all fo the petrol expenses for a specific vehicle.
        /// </summary>
        Task<IEnumerable<PetrolExpense>> GetPetrolExpensesByVehicleId(int vehicleId);

        /// <summary>
        /// Get the last time a specific vehicle had a maintenance check.
        /// </summary>
        DateTime? LastMaintenanceCheckDateForVehicle(int vehicleId);

        /// <summary>
        /// Return a list of all the options that are associated with a certain maintenance check type.
        /// </summary>
        Task<IEnumerable<MaintenanceCheckOption>> GetMaintenanceCheckOptionsForType(int typeId);

        /// <summary>
        /// See if the user had previously chosen this maintenance option during a previous check.
        /// </summary>
        Task<IEnumerable<PerformedMaintenanceOption>> GetOptionsPerformedAsync(int maintenanceCheckId);

        Account GetUser(string userLocalId);
        #endregion

        #region Adds
        /// <summary>
        /// Add new vehicle to the database
        /// </summary>
        void AddNewVehicle(Vehicle vehicle);

        /// <summary>
        /// Save the passed through maintenance check to the database.
        /// </summary>
        Task<int> SetMaintenanceCheckAsync(MaintenanceCheck maintenanceCheck);

        /// <summary>
        /// Add a new petrol expense to the database.
        /// </summary>
        void AddPetrolExpenditure(double amount, DateTime date, int vehicleId);

        /// <summary>
        /// Save the OptionsPerformed to the database. First off check to see if any of them are currently in the database and if so, update their current information.
        /// If they are in the database and now set to not checked then delete them.
        /// If they are not currently in the database and not set to checked, then add the new option.
        /// </summary>
        Task SetOptionsPerformedAsync(IEnumerable<PerformedMaintenanceOption> maintenanceOptionsPerformed);

        void AddRepairReport(string partReplaced, string replacedWith, double price, int vehicleId);

        Account CreateUser(string userLocalId, string email);
        #endregion

        #region Deletes
        /// <summary>
        /// Deletes the passed through object.
        /// </summary>
        void DeleteVehicle(Vehicle vehicle);

        /// <summary>
        /// Delete a maintenance check from the database
        /// </summary>
        void DeleteMaintenanceCheck(int maintenanceCheckId);

        /// <summary>
        /// Delete a petrol expense from the database.
        /// </summary>
        void DeletePetrolExpense(int petrolExpenseId);
        #endregion

        #region Updates
        /// <summary>
        /// This function will take the data from the passed through vehicle and update it's entity in the database.
        /// </summary>
        /// <param name="vehicle"></param>
        void UpdateVehicle(Vehicle vehicle);

        void UpdateUser(Account user);
        #endregion
    }
}   //ImprezGarage.Infrastructure.Model namespace 