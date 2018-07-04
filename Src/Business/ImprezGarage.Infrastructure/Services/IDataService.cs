//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IDataService
    {
        #region Gets
        void GetVehicleTypes(Action<ObservableCollection<VehicleType>, Exception> callback, bool refresh = false);
        void GetVehicleType(Action<VehicleType, Exception> callback, int typeId);
        void GetVehicles(Action<ObservableCollection<Vehicle>, Exception> callback, bool refresh = false);
        void GetVehicleByItsId(Action<Vehicle,Exception> callback, int vehicleId);
        void GetMaintenanceCheckTypes(Action<List<MaintenanceCheckType>, Exception> callback);
        void GetMaintenanceCheckTypeById(Action<MaintenanceCheckType, Exception> callback, int typeId);
        void GetMaintenanceChecksForVehicleByVehicleId(Action<List<MaintenanceCheck>, Exception> callback, int vehicleId);
        void GetMaintenanceChecksById(Action<MaintenanceCheck, Exception> callback, int maintenanceCheckId);
        void GetPetrolExpensesByVehicleId(Action<ObservableCollection<PetrolExpense>, Exception> callback, int vehicleId);
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