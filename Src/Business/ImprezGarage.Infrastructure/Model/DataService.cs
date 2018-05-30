//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class DataService : IDataService
    {
        #region Cached Data
        private ObservableCollection<Vehicle> cachedVehicles;
        private ObservableCollection<VehicleType> cachedVehicleTypes;
        private List<MaintenanceCheckType> cachedMaintenanceTypes;
        #endregion

        #region Gets
        /// <summary>
        /// This function returns a collection of all the vehicle types from teh database.
        /// </summary>
        /// <returns>An observable collection of all vehicle types</returns>
        public void GetVehicleTypes(Action<ObservableCollection<VehicleType>, Exception> callback, bool refresh = false)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (cachedVehicleTypes == null || refresh)
                    {
                        cachedVehicleTypes = new ObservableCollection<VehicleType>(model.VehicleTypes.OrderBy(o => o.Name));
                    }

                    callback(cachedVehicleTypes, null);
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }            
        }

        /// <summary>
        /// This returns the appropriate vehicle type associated with the pass through Id.
        /// </summary>
        public void GetVehicleType(Action<VehicleType, Exception> callback, int typeId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (cachedVehicleTypes == null)
                    {
                        if (model.VehicleTypes.Any(o => o.Id == typeId))
                        {
                            callback(model.VehicleTypes.First(o => o.Id == typeId),null);
                        }
                        else
                        {
                            callback(null, null);
                        }
                    }
                    else
                    {
                        if (cachedVehicleTypes.Any(o => o.Id == typeId))
                        {
                            callback(cachedVehicleTypes.First(o => o.Id == typeId), null);
                        }
                        else
                        {
                            callback(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        /// <summary>
        /// Retrieve all of the Maintenance Check Types from the database.
        /// </summary>
        public void GetMaintenanceCheckTypes(Action<List<MaintenanceCheckType>, Exception> callback)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if(cachedMaintenanceTypes == null)
                    {
                        cachedMaintenanceTypes = model.MaintenanceCheckTypes.OrderBy(o => o.Type).ToList();
                    }

                    callback(cachedMaintenanceTypes, null);                   
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        /// <summary>
        /// Retrieve a specific Maintenance Check Type by it's Id.
        /// </summary>
        public void GetMaintenanceCheckTypeById(Action<MaintenanceCheckType, Exception> callback, int typeId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (cachedMaintenanceTypes == null)
                    {
                        cachedMaintenanceTypes = model.MaintenanceCheckTypes.OrderBy(o => o.Type).ToList();
                    }

                    if (cachedMaintenanceTypes.Any(o => o.Id == typeId))
                    {
                        callback(cachedMaintenanceTypes.First(o => o.Id == typeId), null);
                    }
                    else
                    {
                        callback(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }
        
        /// <summary>
        /// Retrieve all of the saved vehicles
        /// </summary>
        /// <returns>An observable collection of all vehicles</returns>
        public void GetVehicles(Action<ObservableCollection<Vehicle>, Exception> callback, bool refresh = false)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (cachedVehicles == null || refresh)
                    {
                        cachedVehicles = new ObservableCollection<Vehicle>(model.Vehicles);
                    }

                    callback(cachedVehicles, null);
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        /// <summary>
        /// Retrieve all of the maintenance checks performed on a specific vehicle, based off of it's Id.
        /// </summary>
        public void GetMaintenanceChecksForVehicleByVehicleId(Action<List<MaintenanceCheck>, Exception> callback, int vehicleId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if(model.MaintenanceChecks.Any(o => o.VehicleId == vehicleId))
                    {
                        var list = model.MaintenanceChecks.Where(o => o.VehicleId == vehicleId).ToList();
                        callback(list, null);
                    }
                    else
                    {
                        callback(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetMaintenanceChecksById(Action<MaintenanceCheck, Exception> callback, int maintenanceCheckId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (model.MaintenanceChecks.Any(o => o.Id == maintenanceCheckId))
                    {
                        var maintenanceCheck = model.MaintenanceChecks.First(o => o.Id == maintenanceCheckId);
                        callback(maintenanceCheck, null);
                    }
                    else
                    {
                        callback(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        public void GetVehicleByItsId(Action<Vehicle, Exception> callback, int vehicleId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if(cachedVehicles.Any(o => o.Id == vehicleId))
                    {
                        callback(cachedVehicles.First(o => o.Id == vehicleId), null);                        
                    }

                    callback(null, null);
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        public void GetPetrolExpensesByVehicleId(Action<ObservableCollection<PetrolExpense>, Exception> callback, int vehicleId)
        {
            try
            {
                using (var model = new ImprezGarageEntities())
                {
                    if (model.PetrolExpenses.Any(o => o.VehicleId == vehicleId))
                    {
                        var expenses = new ObservableCollection<PetrolExpense>(model.PetrolExpenses.Where(o => o.VehicleId == vehicleId));
                        callback(expenses, null);
                    }
                }
            }
            catch (Exception ex)
            {
                callback(null, ex);
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
                    callback(null);
                }
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