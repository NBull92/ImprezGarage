namespace ImprezGarage.Modules.Firebase
{
    using Infrastructure.Model;
    using Infrastructure.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FirebaseDataStorage : IRepository
    {
        #region Attributes
        private readonly Connection _connection;

        /// <summary>
        /// Store all of the vehicles
        /// </summary>
        private List<Vehicle> _vehicles;
        /// <summary>
        /// Store all of the vehicle types
        /// </summary>
        private List<VehicleType> _vehicleTypes;

        /// <summary>
        /// Store all of the maintenance types
        /// </summary>
        private List<MaintenanceCheckType> _maintenanceTypes;
        #endregion

        #region Methods
        public FirebaseDataStorage(Connection connection)
        {
            _connection = connection;
            _vehicles = new List<Vehicle>();
            _vehicleTypes = new List<VehicleType>();
            _maintenanceTypes = new List<MaintenanceCheckType>();
        }

        public async Task<IEnumerable<MaintenanceCheckType>> GetMaintenanceTypesAsync()
        {
            if (_maintenanceTypes.Count().Equals(0))
            {
                await Task.Run(() =>
                {
                    _maintenanceTypes = _connection.GetAsync<MaintenanceCheckType>().Result;
                });
            }

            return _maintenanceTypes;
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync(bool refresh = false)
        {
            if (_vehicles.Count().Equals(0) || refresh)
            {
                await Task.Run(() =>
                {
                    _vehicles = _connection.GetAsync<Vehicle>().Result;
                });
            }

            return _vehicles;
        }
        
        public async Task<IEnumerable<Vehicle>> GetUserVehiclesAsync(string userId, bool refresh)
        {
            if (_vehicles.Count().Equals(0) || refresh)
            {
                await Task.Run(() =>
                {
                    _vehicles = _connection.GetAsync<Vehicle>().Result;
                });
            }

            return _vehicles.Where(o => o.UserId == userId).OrderByDescending(o => o.DateCreated);
        }


        public Vehicle GetLatestUserVehicle(string userId)
        {
            return _vehicles.LastOrDefault(o => o.UserId == userId);
        }

        public async Task<IEnumerable<VehicleType>> GetVehicleTypesAsync()
        {
            if (_vehicleTypes.Count().Equals(0))
            {
                await Task.Run(() =>
                {
                    _vehicleTypes = _connection.GetAsync<VehicleType>().Result;
                });
            }

            return _vehicleTypes;
        }

        public IEnumerable<PetrolExpense> GetVehiclePetrolExpenses(int vehicleId)
        {
            return _connection.Get<PetrolExpense>().Where(o => o.VehicleId != null && o.VehicleId == vehicleId);
        }

        public IEnumerable<MaintenanceCheck> GetVehicleMaintenanceChecks(int vehicleId)
        {
            return _connection.Get<MaintenanceCheck>().Where(o => o.VehicleId == vehicleId);
        }

        public IEnumerable<MaintenanceCheckOption> GetMaintenanceCheckOptions(int typeId)
        {
            return _connection.Get<MaintenanceCheckOption>().Where(o => o.MaintenanceType == typeId);
        }

        public MaintenanceCheckType GetMaintenanceTypes(int typeId)
        {
            return _connection.Get<MaintenanceCheckType>().FirstOrDefault(o => o.Id == typeId);
        }

        public MaintenanceCheck LastVehicleCheck(int vehicleId)
        {
            return _connection.Get<MaintenanceCheck>().LastOrDefault(o => o.VehicleId == vehicleId);
        }

        public async Task<int> SubmitMaintenanceCheckAsync(MaintenanceCheck maintenanceCheck)
        {
            var last = _connection.Get<MaintenanceCheck>().LastOrDefault();
            maintenanceCheck.Id = last != null ? ++last.Id : 0;

            await Task.Run(() =>
            {
                _connection.Submit(maintenanceCheck, maintenanceCheck.Id);
            });

            return maintenanceCheck.Id;
        }

        public async Task SetOptionsPerformedAsync(IEnumerable<PerformedMaintenanceOption> maintenanceOptionsPerformed)
        {
            await Task.Run(() =>
            {
                var count = _connection.GetAsync<PerformedMaintenanceOption>().Result.Count;

                foreach (var option in maintenanceOptionsPerformed)
                {
                    option.Id = count++;
                    _connection.Submit(option, option.Id);
                }
            });
        }

        public async Task<MaintenanceCheck> GetMaintenanceChecksByIdAsync(int maintenanceCheckId)
        {
            return await Task.Run(() =>
            {
                var results = _connection.GetAsync<MaintenanceCheck>().Result;
                var check = results?.FirstOrDefault(o => o.Id == maintenanceCheckId);
                return check;
            });
        }

        public async Task<IEnumerable<PerformedMaintenanceOption>> GetOptionsPerformedAsync()
        {
            return await Task.Run(() => _connection.GetAsync<PerformedMaintenanceOption>());
        }

        public void AddNewVehicle(Vehicle vehicle)
        {
            vehicle.Id = _vehicles.Count();
            _vehicles.Add(vehicle);
            _connection.Submit(vehicle, vehicle.Id);
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            _connection.Update(vehicle, vehicle.Id);
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            _vehicles.Remove(vehicle);
            _connection.Delete<Vehicle>(vehicle.Id);
        }

        public void DeleteVehiclePetrolExpenditure(Vehicle vehicle)
        {
            var expenses = GetVehiclePetrolExpenses(vehicle.Id).ToList();
            expenses.ForEach(o => DeletePetrolExpenditure(o.Id));
        }

        public void DeletePetrolExpenditure(int expenseId)
        {
            _connection.Delete<PetrolExpense>(expenseId);
        }
        
        public void AddPetrolExpenditure(PetrolExpense expense)
        {
            expense.Id = _connection.Get<PetrolExpense>().Count;
            _connection.Submit(expense, expense.Id);
        }

        public void DeletePerformedMaintenanceOptions(int maintenanceCheckId)
        {
            var list = _connection.Get<PerformedMaintenanceOption>();

            var filtered = list?.Where(o => o.MaintenanceCheck == maintenanceCheckId);

            if (filtered == null)
                return;

            foreach (var option in filtered)
            {
                _connection.Delete<PerformedMaintenanceOption>(option.Id);
            }
        }

        public void DeleteVehicleMaintenanceChecks(Vehicle vehicle)
        {
            var checks = GetVehicleMaintenanceChecks(vehicle.Id).ToList();
            checks.ForEach(o => DeleteMaintenanceCheck(o.Id));
        }

        public void DeleteMaintenanceCheck(int maintenanceCheckId)
        {
            DeletePerformedMaintenanceOptions(maintenanceCheckId);
            
            _connection.Delete<MaintenanceCheck>(maintenanceCheckId);
        }

        public void AddRepair(PartsReplacementRecord repair)
        {
            repair.Id = _connection.Get<PartsReplacementRecord>().Count;
            _connection.Submit(repair, repair.Id);
        }

        public void SubmitNewUser(Account account)
        {
            account.Id = _connection.Get<Account>().Count;
            _connection.Submit(account, account.Id);
        }

        public Account GetUser(string userLocalId)
        {
            return _connection.Get<Account>().FirstOrDefault(O => O.UserId == userLocalId);
        }

        public void UpdateUser(Account user)
        {
            _connection.Update(user, user.Id);
        }

        public bool GetUserVehicle(int vehicleId, string userId)
        {
           return _vehicles.Any(o => o.Id.Equals(vehicleId) && o.UserId.Equals(userId));
        }
        #endregion
    }
}