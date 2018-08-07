﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model
{
    using ImprezGarage.Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataService : IDataService
    {
        #region Cached Data
        private DataStorage _dataStorage;
        #endregion

        public DataService()
        {
            _dataStorage = new DataStorage();
        }

        #region Gets
        /// <summary>
        /// Retrieve all of the saved vehicles
        /// </summary>
        /// <returns>An observable collection of all vehicles</returns>
        public Task<ObservableCollection<Vehicle>> GetVehicles(bool refresh = false)
        {
            RetrieveVehicles(refresh);
            return Task.Run(() => _dataStorage.GetVehicles());
        }

        public Task<Vehicle> GetVehicleByItsId(int vehicleId)
        {
            RetrieveVehicles();

            if (_dataStorage.GetVehicles().Any(o => o.Id == vehicleId))
            {
                return Task.Run(() => _dataStorage.GetVehicles().First(o => o.Id == vehicleId));
            }
            return null;
        }

        private void RetrieveVehicles(bool refresh = false)
        {
            using (var model = new ImprezGarageEntities())
            {
                if (_dataStorage.GetVehicles() == null || refresh)
                {
                    _dataStorage.SetVehicles(model.Vehicles.ToList());
                }
            }
        }

        /// <summary>
        /// This function returns a collection of all the vehicle types from teh database.
        /// </summary>
        /// <returns>An observable collection of all vehicle types</returns>
        public Task<ObservableCollection<VehicleType>> GetVehicleTypes(bool refresh = false)
        {
            RetrieveVehicleTypes(refresh);
            return Task.Run(() => _dataStorage.GetVehicleTypes());
        }

        /// <summary>
        /// This returns the appropriate vehicle type associated with the pass through Id.
        /// </summary>
        public Task<VehicleType> GetVehicleType(int typeId)
        {
            RetrieveVehicleTypes();

            if (_dataStorage.GetVehicleTypes().Any(o => o.Id == typeId))
            {
                return Task.Run(() => _dataStorage.GetVehicleTypes().First(o => o.Id == typeId));
            }
            return null;
        }

        private void RetrieveVehicleTypes(bool refresh = false)
        {
            using (var model = new ImprezGarageEntities())
            {
                if (!_dataStorage.GetVehicleTypes().Any() || refresh)
                {
                    _dataStorage.SetVehicleTypes(model.VehicleTypes.OrderBy(o => o.Name).ToList());
                }
            }
        }

        /// <summary>
        /// Retrieve all of the Maintenance Check Types from the database.
        /// </summary>
        public Task<ObservableCollection<MaintenanceCheckType>> GetMaintenanceCheckTypes()
        {
            RetrieveMaintenanceTypes();
            return Task.Run(() => _dataStorage.GetMaintenanceTypes());
        }

        /// <summary>
        /// Retrieve a specific Maintenance Check Type by it's Id.
        /// </summary>
        public Task<MaintenanceCheckType> GetMaintenanceCheckTypeById(int typeId)
        {
            RetrieveMaintenanceTypes();

            if (_dataStorage.GetMaintenanceTypes().Any(o => o.Id == typeId))
            {
                return Task.Run(() => _dataStorage.GetMaintenanceTypes().First(o => o.Id == typeId));
            }
            return null;
        }

        private void RetrieveMaintenanceTypes()
        {
            using (var model = new ImprezGarageEntities())
            {
                if (!_dataStorage.GetMaintenanceTypes().Any())
                {
                    _dataStorage.SetMaintenanceTypes(model.MaintenanceCheckTypes.OrderBy(o => o.Type).ToList());
                }
            }
        }

        /// <summary>
        /// Retrieve all of the maintenance checks performed on a specific vehicle, based off of it's Id.
        /// </summary>
        public Task<List<MaintenanceCheck>> GetMaintenanceChecksForVehicleByVehicleId(int vehicleId)
        {
            using (var model = new ImprezGarageEntities())
            {
                if (model.MaintenanceChecks.Any(o => o.VehicleId == vehicleId))
                {
                    var list = model.MaintenanceChecks.Where(o => o.VehicleId == vehicleId).ToList();
                    return Task.Run(() => list);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Task<MaintenanceCheck> GetMaintenanceChecksById(int maintenanceCheckId)
        {
            using (var model = new ImprezGarageEntities())
            {
                if (model.MaintenanceChecks.Any(o => o.Id == maintenanceCheckId))
                {
                    var maintenanceCheck = model.MaintenanceChecks.First(o => o.Id == maintenanceCheckId);
                    return Task.Run(() => maintenanceCheck);
                }
                return null;
            }
        }

        public Task<ObservableCollection<PetrolExpense>> GetPetrolExpensesByVehicleId(int vehicleId)
        {
            using (var model = new ImprezGarageEntities())
            {
                if (model.PetrolExpenses.Any(o => o.VehicleId == vehicleId))
                {
                    var expenses = new ObservableCollection<PetrolExpense>(model.PetrolExpenses.Where(o => o.VehicleId == vehicleId));
                    return Task.Run(() => expenses);
                }
                return null;
            }
        }

        public Task<DateTime?> GetLastMaintenanceCheckDateForVehicleByVehicleId(int vehicleId)
        {
            using (var model = new ImprezGarageEntities())
            {
                if (model.MaintenanceChecks.Any(o => o.VehicleId == vehicleId))
                {
                    if (model.MaintenanceChecks.Count(o => o.VehicleId == vehicleId) == 1)
                    {
                        var last = model.MaintenanceChecks.First(o => o.VehicleId == vehicleId);
                        return Task.Run(() => last.DatePerformed);
                    }
                    else
                    {
                        var last = model.MaintenanceChecks.Last(o => o.VehicleId == vehicleId);
                        return Task.Run(() => last.DatePerformed);
                    }
                }
                return null;
            }
        }
        #endregion

        #region Adds
        /// <summary>
        /// Add new vehicle to the database
        /// </summary>
        public void AddNewVehicle(Action<Exception> callback, Vehicle vehicle)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    model.Vehicles.Add(vehicle);
                    model.SaveChanges();
                    _dataStorage.AddNewVehicle(model.Vehicles.Last());
                }
                callback(null);
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }

        /// <summary>
        /// Save the passed through maintenance check to the database.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="maintenanceCheck"></param>
        public void SubmitMaintenanceCheck(Action<Exception> callback, MaintenanceCheck maintenanceCheck)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    model.MaintenanceChecks.Add(maintenanceCheck);
                    model.SaveChanges();
                    callback(null);
                }
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }

        public void AddPetrolExpenditure(Action<Exception> callback, double amount, int vehicleId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    model.PetrolExpenses.Add(new PetrolExpense
                    {
                        Amount = amount,
                        DateEntered = DateTime.Now,
                        VehicleId = vehicleId
                    });
                    model.SaveChanges();
                    callback(null);
                }
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }
        #endregion

        #region Deletes
        /// <summary>
        /// Deletes the passed through object.
        /// </summary>
        /// <param name="callback">sends back whether there was an error during the delete process.</param>
        public void DeleteVehicle(Action<Exception> callback, Vehicle vehicle)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (model.Vehicles.Any(o => o.Id == vehicle.Id))
                    {
                        model.Vehicles.Remove(model.Vehicles.First(o => o.Id == vehicle.Id));
                        model.SaveChanges();
                        callback(null);
                    }
                }
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }


        public void DeleteMaintenanceCheck(Action<Exception> callback, int maintenanceCheckId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (model.MaintenanceChecks.Any(o => o.Id == maintenanceCheckId))
                    {
                        var check = model.MaintenanceChecks.First(o => o.Id == maintenanceCheckId);
                        model.MaintenanceChecks.Remove(check);
                        model.SaveChanges();
                        callback(null);
                    }
                }
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }


        public void DeletePetrolExpense(Action<Exception> callback, int petrolExpenseId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (model.PetrolExpenses.Any(o => o.Id == petrolExpenseId))
                    {
                        var expense = model.PetrolExpenses.First(o => o.Id == petrolExpenseId);
                        model.PetrolExpenses.Remove(expense);
                        model.SaveChanges();
                        callback(null);
                    }
                }
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }
        #endregion

        #region Updates
        /// <summary>
        /// This function will take the data from the passed through vehicle and update it's entity in the database.
        /// </summary>
        /// <param name="callback">sends back whether there was an error during the save or not.</param>
        public void UpdateVehicle(Action<Exception> callback, Vehicle vehicle)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (model.Vehicles.Any(o => o.Id == vehicle.Id))
                    {
                        var updateVehicle = model.Vehicles.First(o => o.Id == vehicle.Id);
                        updateVehicle.DateModified = DateTime.Now;
                        updateVehicle.FriendlyName = vehicle.FriendlyName;
                        updateVehicle.HasInsurance = vehicle.HasInsurance;
                        updateVehicle.HasValidTax = vehicle.HasValidTax;
                        updateVehicle.InsuranceRenewalDate = vehicle.InsuranceRenewalDate;
                        updateVehicle.Make = vehicle.Make;
                        updateVehicle.Model = vehicle.Model;
                        updateVehicle.Registration = vehicle.Registration;
                        updateVehicle.TaxExpiryDate = vehicle.TaxExpiryDate;
                        updateVehicle.VehicleType = vehicle.VehicleType;
                        model.SaveChanges();
                        callback(null);
                    }
                    else
                    {
                        callback(new Exception("This vehicle does not exist."));
                    }
                }
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }

        public void UpdateMaintenanceCheck(Action<Exception> callback, MaintenanceCheck maintenanceCheck)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (model.MaintenanceChecks.Any(o => o.Id == maintenanceCheck.Id))
                    {
                        var check = model.MaintenanceChecks.First(o => o.Id == maintenanceCheck.Id);
                        check.AddedAutoTransmissionFluid = check.AddedAutoTransmissionFluid;
                        check.AirFilterNotes = check.AirFilterNotes;
                        check.AutoTransmissionFluidNotes = check.AutoTransmissionFluidNotes;
                        check.BatteryNotes = check.BatteryNotes;
                        check.ChangeFanBelt = check.ChangeFanBelt;
                        check.CheckAutoTransmissionFluid = check.CheckAutoTransmissionFluid;
                        check.CheckCoolantLevels = check.CheckCoolantLevels;
                        check.CheckedAirFilter = check.CheckedAirFilter;
                        check.CheckedBattery = check.CheckedBattery;
                        check.CheckedOilLevels = check.CheckedOilLevels;
                        check.CheckPowerSteeringFluidLevels = check.CheckPowerSteeringFluidLevels;
                        check.CoolantNotes = check.CoolantNotes;
                        check.FlushedSystemAndChangeCoolant = check.FlushedSystemAndChangeCoolant;
                        check.OilLevelNotes = check.OilLevelNotes;
                        check.PerformedBy = check.PerformedBy;
                        check.PowerSteeringNotes = check.PowerSteeringNotes;
                        check.ReplacedAirFilter = check.ReplacedAirFilter;
                        check.ReplacedOilFilter = check.ReplacedOilFilter;

                        model.SaveChanges();
                    }

                    callback(null);
                }
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }
        #endregion
    }
}   // ImprezGarage.Infrastructure namespace 