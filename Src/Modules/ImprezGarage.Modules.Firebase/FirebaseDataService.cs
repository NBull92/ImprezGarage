
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
        ///<inheritdoc/>
        public async Task<IEnumerable<Vehicle>> GetVehicles(bool refresh = false)
        {
            return await Task.Run(() => _dataStorage.GetVehiclesAsync(refresh));
        }

        
        ///<inheritdoc/>
        public async Task<IEnumerable<Vehicle>> GetUserVehicles(string userId, bool refresh = false)
        {
            return await Task.Run(() => _dataStorage.GetUserVehiclesAsync(userId, refresh));
        }

        ///<inheritdoc/>
        public Vehicle GetLatestUserVehicle(string userId)
        {
            return _dataStorage.GetLatestUserVehicle(userId);
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<VehicleType>> GetVehicleTypesAsync(bool refresh = false)
        {
            return await Task.Run(() => _dataStorage.GetVehicleTypesAsync());
        }

        ///<inheritdoc/>
        public async Task<VehicleType> GetVehicleTypeAsync(int typeId)
        {
            return await Task.Run(() =>
            {
                var types = _dataStorage.GetVehicleTypesAsync().Result.ToList();
                return types.FirstOrDefault(o => o.Id == typeId);
            });
        }

        ///<inheritdoc/>
        public MaintenanceCheckType GetMaintenanceCheckTypeById(int typeId)
        {
            return _dataStorage.GetMaintenanceTypes(typeId);
        }

        ///<inheritdoc/>
        public IEnumerable<MaintenanceCheck> GetVehicleMaintenanceChecks(int vehicleId)
        {
            return _dataStorage.GetVehicleMaintenanceChecks(vehicleId);
        }

        ///<inheritdoc/>
        public Task<IEnumerable<PetrolExpense>> GetPetrolExpensesByVehicleId(int vehicleId)
        {
            return Task.Run(() => _dataStorage.GetVehiclePetrolExpenses(vehicleId));
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<MaintenanceCheckType>> GetMaintenanceCheckTypesAsync()
        {
            return await Task.Run(() =>
            {
                var types = _dataStorage.GetMaintenanceTypesAsync().Result;
                return types;
            });
        }

        ///<inheritdoc/>
        public Task<IEnumerable<MaintenanceCheckOption>> GetMaintenanceCheckOptionsForType(int typeId)
        {
            return Task.Run(() => _dataStorage.GetMaintenanceCheckOptions(typeId));
        }

        ///<inheritdoc/>
        public async Task<MaintenanceCheck> GetMaintenanceChecksByIdAsync(int maintenanceCheckId)
        {
            return await Task.Run(() => _dataStorage.GetMaintenanceChecksByIdAsync(maintenanceCheckId));
        }

        ///<inheritdoc/>
        public DateTime? LastMaintenanceCheckDateForVehicle(int vehicleId)
        {
            var last = _dataStorage.LastVehicleCheck(vehicleId);
            return last?.DatePerformed;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<PerformedMaintenanceOption>> GetOptionsPerformedAsync(int maintenanceCheckId)
        {
            return await Task.Run(() =>
                {
                    return _dataStorage.GetOptionsPerformedAsync().Result
                        .Where(o => o.MaintenanceCheck == maintenanceCheckId);
                });
        }

        ///<inheritdoc/>
        public bool IsValidVehicle(int vehicleId, string userId)
        {
            return _dataStorage.GetUserVehicle(vehicleId, userId);
        }

        #endregion

        #region Adds
        ///<inheritdoc/>
        public void AddNewVehicle(Vehicle vehicle)
        {
            _dataStorage.AddNewVehicle(vehicle);
        }

        ///<inheritdoc/>
        public async Task<int> SetMaintenanceCheckAsync(MaintenanceCheck maintenanceCheck)
        {
            return await Task.Run(() => _dataStorage.SubmitMaintenanceCheckAsync(maintenanceCheck));
        }

        ///<inheritdoc/>
        public void AddPetrolExpenditure(double amount, DateTime date, int vehicleId)
        {
            var expense = new PetrolExpense {Amount = amount, VehicleId = vehicleId, DateEntered = date};

            _dataStorage.AddPetrolExpenditure(expense);
        }


        ///<inheritdoc/>
        public async Task SetOptionsPerformedAsync(IEnumerable<PerformedMaintenanceOption> maintenanceOptionsPerformed)
        {
            await _dataStorage.SetOptionsPerformedAsync(maintenanceOptionsPerformed);
        }

        ///<inheritdoc/>
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
        ///<inheritdoc/>
        public void DeleteVehicle(Vehicle vehicle)
        {
            _dataStorage.DeleteVehicleMaintenanceChecks(vehicle);
            _dataStorage.DeleteVehiclePetrolExpenditure(vehicle);
            _dataStorage.DeleteVehicle(vehicle);
        }

        ///<inheritdoc/>
        public void DeleteMaintenanceCheck(int maintenanceCheckId)
        {
            _dataStorage.DeleteMaintenanceCheck(maintenanceCheckId);
        }

        ///<inheritdoc/>
        public void DeletePetrolExpense(int petrolExpenseId)
        {
            _dataStorage.DeletePetrolExpenditure(petrolExpenseId);
        }
        #endregion

        #region Updates
        ///<inheritdoc/>
        public void UpdateVehicle(Vehicle vehicle)
        {
            _dataStorage.UpdateVehicle(vehicle);
        }

        ///<inheritdoc/>
        public void UpdateUser(Account user)
        {
            _dataStorage.UpdateUser(user);
        }

        ///<inheritdoc/>
        public Account CreateUser(string userLocalId, string email)
        {
            var newUser = new Account
            {
                UserId = userLocalId,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Name = email.Split('@')[0]
            };
            
            _dataStorage.SubmitNewUser(newUser);
            return newUser;
        }

        ///<inheritdoc/>
        public Account GetUser(string userLocalId)
        {
            return _dataStorage.GetUser(userLocalId);
        }
        #endregion
    }
}
