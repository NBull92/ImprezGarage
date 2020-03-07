
namespace ImprezGarage.Modules.MyGarage
{
    using Infrastructure.Model;
    using Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VehicleService : IVehicleService
    {
        public event EventHandler<Vehicle> SelectedVehicleChanged = delegate { };

        public void RaiseSelectedVehicleChanged(Vehicle vehicle)
        {
            OnRaiseSelectedVehicleChanged(vehicle);
        }

        public void ClearSelectedVehicle()
        {
            OnRaiseSelectedVehicleChanged();
        }

        private void OnRaiseSelectedVehicleChanged(Vehicle vehicle = null)
        {
            //Create list of exception
            var exceptions = new List<Exception>();

            //Invoke OnSelectedVehicleChanged Action by iterating on all subscribers event handlers
            foreach (var handler in SelectedVehicleChanged.GetInvocationList())
            {
                try
                {
                    //pass sender object and eventArgs while
                    handler.DynamicInvoke(this, vehicle);
                }
                catch (Exception e)
                {
                    //Add exception in exception list if occured any
                    exceptions.Add(e);
                }
            }

            //Check if any exception occured while 
            //invoking the subscribers event handlers
            if (exceptions.Any())
            {
                //Throw aggregate exception of all exceptions 
                //occured while invoking subscribers event handlers
                throw new AggregateException(exceptions);
            }
        }
    }
}
