//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.UnitTests.MyGarage
{
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class AddVehicle
    {
        [TestMethod]
        public void AddCar()
        {
            var newVehicle = new Vehicle
            {
                VehicleType = 1,
                Model = "Subaru",
                Make = "Unit Test",
                Registration = "BD53LRV",
                InsuranceRenewalDate = DateTime.Now,
                TaxExpiryDate = DateTime.Now,
            };

            //DataService service = new DataService();
            //service.AddNewVehicle(newVehicle);
        }

        [TestMethod]
        public void AddMotorbike()
        {
            var newVehicle = new Vehicle
            {
                VehicleType = 2,
                Model = "Yamaha",
                Make = "Bike Unit Test",
                Registration = "BD53LRV",
                InsuranceRenewalDate = DateTime.Now,
                TaxExpiryDate = DateTime.Now,
            };

            //DataService service = new DataService();
            //service.AddNewVehicle(newVehicle);
        }

        [TestMethod]
        public void AddBicycle()
        {
            var newVehicle = new Vehicle
            {
                VehicleType = 3,
            };

            //DataService service = new DataService();
            //service.AddNewVehicle(newVehicle);
        }
    }
}   //ImprezGarage.UnitTests.MyGarage namespace 