//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public interface IDataService
    {
        #region Gets
        Task<ObservableCollection<Vehicle>> GetVehicles(bool refresh = false);
        Task<Vehicle> GetVehicleByItsId(int vehicleId);
        Task<ObservableCollection<VehicleType>> GetVehicleTypes(bool refresh = false);
        Task<VehicleType> GetVehicleType(int typeId);
        Task<ObservableCollection<MaintenanceCheckType>> GetMaintenanceCheckTypes();
        Task<MaintenanceCheckType> GetMaintenanceCheckTypeById(int typeId);
        Task<List<MaintenanceCheck>> GetMaintenanceChecksForVehicleByVehicleId(int vehicleId);
        Task<MaintenanceCheck> GetMaintenanceChecksById(int maintenanceCheckId);
        Task<ObservableCollection<PetrolExpense>> GetPetrolExpensesByVehicleId(int vehicleId);
        Task<DateTime?> GetLastMaintenanceCheckDateForVehicleByVehicleId(int vehicleId);
        #endregion

        #region Adds
        void AddNewVehicle(Action<Exception> callback, Vehicle vehicle);
        void SubmitMaintenanceCheck(Action<Exception> callback, MaintenanceCheck maintenanceCheck);
        void AddPetrolExpenditure(Action<Exception> callback, double amount, int vehicleId);
        #endregion

        #region Deletes
        void DeleteVehicle(Action<Exception> callback, Vehicle vehicle);
        void DeleteMaintenanceCheck(Action<Exception> callback, int maintenanceCheckId);
        void DeletePetrolExpense(Action<Exception> callback, int petrolExpenseId);
        #endregion

        #region Updates
        void UpdateVehicle(Action<Exception> callback, Vehicle editVehicle);
        void UpdateMaintenanceCheck(Action<Exception> callback, MaintenanceCheck maintenanceCheck);
        #endregion
    }
}   //ImprezGarage.Infrastructure.Model namespace 