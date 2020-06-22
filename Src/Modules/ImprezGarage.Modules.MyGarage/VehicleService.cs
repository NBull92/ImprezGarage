
namespace ImprezGarage.Modules.MyGarage
{
    using Infrastructure.Model;
    using Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VehicleService : IVehicleService
    {
        private Guid _Id = Guid.NewGuid();
        private Vehicle _currentVehicle;
        
        public event EventHandler<Vehicle> SelectedVehicleChanged = delegate { };

        public void RaiseSelectedVehicleChanged(Vehicle vehicle)
        {
            _currentVehicle = vehicle;
            OnRaiseSelectedVehicleChanged();
        }

        public void ClearSelectedVehicle()
        {
            _currentVehicle = null;
            OnRaiseSelectedVehicleChanged();
        }

        public Vehicle GetSelectedVehicle()
        {
            return _currentVehicle;
        }

        private void OnRaiseSelectedVehicleChanged()
        {
            //Create list of exception
            var exceptions = new List<Exception>();
            var test = SelectedVehicleChanged.GetInvocationList();
            //Invoke OnSelectedVehicleChanged Action by iterating on all subscribers event handlers
            foreach (var handler in SelectedVehicleChanged.GetInvocationList())
            {
                try
                {
                    //pass sender object and eventArgs while
                    handler.DynamicInvoke(this, _currentVehicle);
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
