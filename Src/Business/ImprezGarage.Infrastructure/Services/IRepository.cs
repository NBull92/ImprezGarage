
namespace ImprezGarage.Infrastructure.Services
{
    using Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository
    {
        /// <summary>
        /// return the list of vehicle types.
        /// </summary>
        Task<IEnumerable<VehicleType>> GetVehicleTypesAsync();
        
        /// <summary>
        /// Return the collection of maintenance types.
        /// </summary>
        Task<IEnumerable<MaintenanceCheckType>> GetMaintenanceTypesAsync();

        
        /// <summary>
        /// Return the collection of vehicles.
        /// </summary>
        Task<IEnumerable<Vehicle>> GetVehiclesAsync(bool refresh = false);

        /// <summary>
        /// Add a new vehicle to the collection of vehicles.
        /// </summary>
        void AddNewVehicle(Vehicle vehicle);
    }

    /*//FUTURE IMPLEMENTATION AFTER GETTING FIREBASE TO WORK
     Based on https://www.youtube.com/watch?v=rtXpYpZdOzM
     public interface IRepository<T> where T : class
     {
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
     }
    */
}