
namespace ImprezGarage.Modules.Firebase
{
    using Infrastructure.Model;
    using Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class FirebaseDataService : IDataService
    {
        #region Attributes
        /// <summary>
        /// Store for the retrieved data from the database.
        /// </summary>
        private readonly FirebaseDataStorage _dataStorage;
        #endregion

        /// <summary>
        /// Construct the data service class and instantiate the data storage.
        /// </summary>
        public FirebaseDataService()
        {
            var connection = new Connection();
            _dataStorage = new FirebaseDataStorage(connection);
        }

        #region Gets
        public async Task<IEnumerable<Vehicle>> GetVehicles(bool refresh = false)
        {
            return await Task.Run(() => _dataStorage.GetVehiclesAsync(refresh));
        }
        
        public async Task<IEnumerable<VehicleType>> GetVehicleTypesAsync(bool refresh = false)
        {
            return await Task.Run(() => _dataStorage.GetVehicleTypesAsync());
        }

        public async Task<VehicleType> GetVehicleTypeAsync(int typeId)
        {
            return await Task.Run(() =>
            {
                var types = _dataStorage.GetVehicleTypesAsync().Result.ToList();
                return types.FirstOrDefault(o => o.Id == typeId);
            });
        }

        public MaintenanceCheckType GetMaintenanceCheckTypeById(int typeId)
        {
            return _dataStorage.GetMaintenanceTypes(typeId);
        }

        public IEnumerable<MaintenanceCheck> GetVehicleMaintenanceChecks(int vehicleId)
        {
            return _dataStorage.GetVehicleMaintenanceChecks(vehicleId);
        }

        public Task<IEnumerable<PetrolExpense>> GetPetrolExpensesByVehicleId(int vehicleId)
        {
            return Task.Run(() => _dataStorage.GetVehiclePetrolExpenses(vehicleId));
        }

        public async Task<IEnumerable<MaintenanceCheckType>> GetMaintenanceCheckTypesAsync()
        {
            return await Task.Run(() =>
            {
                var types = _dataStorage.GetMaintenanceTypesAsync().Result;
                return types;
            });
        }

        public Task<IEnumerable<MaintenanceCheckOption>> GetMaintenanceCheckOptionsForType(int typeId)
        {
            return Task.Run(() => _dataStorage.GetMaintenanceCheckOptions(typeId));
        }

        public async Task<MaintenanceCheck> GetMaintenanceChecksByIdAsync(int maintenanceCheckId)
        {
            return await Task.Run(() => _dataStorage.GetMaintenanceChecksByIdAsync(maintenanceCheckId));
        }

        public DateTime? LastMaintenanceCheckDateForVehicle(int vehicleId)
        {
            var last = _dataStorage.LastVehicleCheck(vehicleId);
            return last?.DatePerformed;
        }

        public async Task<IEnumerable<PerformedMaintenanceOption>> GetOptionsPerformedAsync(int maintenanceCheckId)
        {
            return await Task.Run(() =>
                {
                    return _dataStorage.GetOptionsPerformedAsync().Result
                        .Where(o => o.MaintenanceCheck == maintenanceCheckId);
                });
        }

        #endregion

        #region Adds
        public void AddNewVehicle(Vehicle vehicle)
        {
            _dataStorage.AddNewVehicle(vehicle);
        }

        public async Task<int> SetMaintenanceCheckAsync(MaintenanceCheck maintenanceCheck)
        {
            return await Task.Run(() => _dataStorage.SubmitMaintenanceCheckAsync(maintenanceCheck));
        }

        public void AddPetrolExpenditure(double amount, DateTime date, int vehicleId)
        {
            var expense = new PetrolExpense {Amount = amount, VehicleId = vehicleId, DateEntered = date};

            _dataStorage.AddPetrolExpenditure(expense);
        }


        public async Task SetOptionsPerformedAsync(IEnumerable<PerformedMaintenanceOption> maintenanceOptionsPerformed)
        {
            await _dataStorage.SetOptionsPerformedAsync(maintenanceOptionsPerformed);
        }

        public void AddRepairReport(string partReplaced, string replacedWith, double price, int vehicleId)
        {
            var repair = new PartsReplacementRecord
            {
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                PartReplaced = partReplaced,
                ReplacedWith = replacedWith,
                Price = price,
                VehicleId= vehicleId,
            };

            _dataStorage.AddRepair(repair);
        }

        #endregion

        #region Deletes
        public void DeleteVehicle(Vehicle vehicle)
        {
            _dataStorage.DeleteVehicle(vehicle);
        }

        public void DeleteMaintenanceCheck(int maintenanceCheckId)
        {
            _dataStorage.DeletePerformedMaintenanceOptions(maintenanceCheckId);
            _dataStorage.DeleteMaintenanceCheck(maintenanceCheckId);
        }

        public void DeletePetrolExpense(int petrolExpenseId)
        {
            _dataStorage.DeletePetrolExpenditure(petrolExpenseId);
        }
        #endregion

        #region Updates
        public void UpdateVehicle(Vehicle vehicle)
        {
            _dataStorage.UpdateVehicle(vehicle);
        }
        #endregion
    }
}
