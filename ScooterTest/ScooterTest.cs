using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scooter;
using System.Collections.Generic;

namespace ScooterTest
{
    [TestClass]
    public class ScooterTest
    {
        [TestMethod]
        public void Update_And_Retrieve_Scooter_Inventory()
        {
            RentalCompany rc = new RentalCompany("Name of the company");

            rc.ScooterService.AddScooter("1", 0.1m);
            rc.ScooterService.AddScooter("2", 0.1m);
            rc.ScooterService.AddScooter("3", 0.1m);
            rc.ScooterService.AddScooter("4", 0.1m);

            rc.StartRent("4");
            rc.EndRent("4");
            rc.StartRent("3");

            var expectedInventory = new List<Scooter.Scooter>()
            {
                rc.ScooterService.GetScooterById("1"),
                rc.ScooterService.GetScooterById("2"),
                rc.ScooterService.GetScooterById("4")
            };

            CollectionAssert.AreEqual(expectedInventory, rc.ScooterService.GetScooters());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No scooter found by id: 5")]
        public void Accessing_Non_Existant_Scooter()
        {
            RentalCompany rc = new RentalCompany("Name of the company");

            rc.ScooterService.AddScooter("1", 0.1m);
            rc.StartRent("5");
        }

        [TestMethod]
        public void Removing_Scooter()
        {
            RentalCompany rc = new RentalCompany("Name of the company");

            rc.ScooterService.AddScooter("1", 0.1m);
            rc.ScooterService.AddScooter("2", 0.1m);
            rc.ScooterService.AddScooter("3", 0.1m);
            rc.ScooterService.AddScooter("4", 0.1m);

            rc.ScooterService.RemoveScooter("4");

            var expectedInventory = new List<Scooter.Scooter>()
            {
                rc.ScooterService.GetScooterById("1"),
                rc.ScooterService.GetScooterById("2"),
                rc.ScooterService.GetScooterById("3")
            };

            CollectionAssert.AreEqual(expectedInventory, rc.ScooterService.GetScooters());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "ID already taken")]
        public void Adding_Duplicate_Id()
        {
            RentalCompany rc = new RentalCompany("Name of the company");
            rc.ScooterService.AddScooter("3", 0.1m);
            rc.ScooterService.AddScooter("3", 0.1m);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Empty name was given.")]
        public void Adding_Empty_Name()
        {
            RentalCompany rc = new RentalCompany("");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), 
            "Given time for rent end is impossible.\n" +
            "Rent end date time is before rent start?")]
        public void Adding_Impossible_RentEnd_Time()
        {
            var rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            var rentEnd = new DateTime(2021, 12, 18, 12, 0, 0);
            var rent = new RentCalculator();

            rent.CalculateRent(rentStart, rentEnd, 1m);
        }

        [TestMethod]
        public void Calculate_Rent_Price_Without_Exceeding_Limit()
        {
            var rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            var rentEnd = new DateTime(2021, 12, 18, 13, 15, 0);

            var rent = new RentCalculator();

            decimal expectedPrice = 15;

            Assert.AreEqual(expectedPrice, rent.CalculateRent(rentStart, rentEnd, 1m));
        }

        [TestMethod]
        public void Calculate_Rent_Price_With_Exceeding_Daily_Limit()
        {
            var rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            var rentEnd = new DateTime(2021, 12, 18, 13, 30, 0);

            var rent = new RentCalculator();

            decimal expectedPrice = 20;

            Assert.AreEqual(expectedPrice, rent.CalculateRent(rentStart, rentEnd, 1m));
        }

        [TestMethod]
        public void Calculate_Rent_Price_Across_Multiple_Days()
        {
            var rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            var rentEnd = new DateTime(2021, 12, 19, 13, 15, 0);

            var rent = new RentCalculator();

            decimal expectedPrice = 40;

            Assert.AreEqual(expectedPrice, rent.CalculateRent(rentStart, rentEnd, 1m));

            rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            rentEnd = new DateTime(2021, 12, 20, 0, 15, 0);

            expectedPrice = 55;

            Assert.AreEqual(expectedPrice, rent.CalculateRent(rentStart, rentEnd, 1m));

            rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            rentEnd = new DateTime(2022, 1, 1, 0, 15, 0);

            expectedPrice = 295;

            Assert.AreEqual(expectedPrice, rent.CalculateRent(rentStart, rentEnd, 1m));

            rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            rentEnd = new DateTime(2022, 1, 1, 0, 15, 0);

            expectedPrice = 193.95m;

            Assert.AreEqual(expectedPrice, rent.CalculateRent(rentStart, rentEnd, 0.01m));

            rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            rentEnd = new DateTime(2021, 12, 19, 14, 15, 0);

            expectedPrice = 37.82m;

            Assert.AreEqual(expectedPrice, rent.CalculateRent(rentStart, rentEnd, 0.027m));
        }
    }
}
