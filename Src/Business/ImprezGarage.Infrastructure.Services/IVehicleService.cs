
namespace ImprezGarage.Infrastructure.Services
{
    using Model;
    using System;

    public interface IVehicleService
    {
        event EventHandler<Vehicle> SelectedVehicleChanged;
        void RaiseSelectedVehicleChanged(Vehicle vehicle);
        void ClearSelectedVehicle();
    }
}
