using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scooter
{
    public class RentalCompany : IRentalCompany
    {
        public ScooterService ScooterService = new ScooterService();
        // Dictionary containing income of the company. Keys are years and value is total income
        private Dictionary<int, decimal> _incomeByYear = new Dictionary<int, decimal>();
        // Dictionary to keep track of rented scooters. Keys are years and value is a List of rented scooter id's 
        private Dictionary<int, List<string>> _rentedScooters = new Dictionary<int, List<string>>();
        RentCalculator rentCalculator = new RentCalculator();

        public string Name { get; }

        public RentalCompany(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("Empty name was given.");
            }
            else
            {
                Name = name;
            }
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            if (year < 0 || year > 9999)
            {
                throw new Exception("Given year is out of range.");
            }

            decimal total = 0;
            
            // return income from all years
            if (year == null)
            {
                // add income from currently rented scooters
                if (includeNotCompletedRentals)
                {
                    total += CalculateRentedScooterPrice(year);
                }

                // add return income from returned scooters from all years
                foreach (var scooter in _incomeByYear)
                {
                    total += scooter.Value;
                }

                return total;
            }
            // return income from specified year
            else
            {
                // add income from rentedScooters
                if (includeNotCompletedRentals)
                {
                    total += CalculateRentedScooterPrice(year);
                }

                // add income from returned scooters within a specified year
                foreach (var scooter in _incomeByYear)
                {
                    if (scooter.Key == year)
                    {
                        total += scooter.Value;
                    }
                }
                return total;
            }
        }

        private decimal CalculateRentedScooterPrice(int? year)
        {
            if (year < 0 || year > 9999)
            {
                throw new Exception("Given year is out of range.");
            }

            decimal total = 0;
            Scooter scooter;

            // return income from all years
            if (year == null)
            {
                foreach (var scooters in _rentedScooters)
                {
                    foreach (var id in scooters.Value)
                    {
                        scooter = ScooterService.GetScooterById(id);
                        total += rentCalculator.CalculateRent(scooter.TimeRented, DateTime.Now, scooter.PricePerMinute);
                    }
                }
            }
            // return income from specified year
            else
            {
                foreach (var scooters in _rentedScooters)
                {
                    if (scooters.Key == year)
                    {
                        foreach (var id in scooters.Value)
                        {
                            scooter = ScooterService.GetScooterById(id);
                            total += rentCalculator.CalculateRent(scooter.TimeRented, DateTime.Now, scooter.PricePerMinute);
                        }
                        
                    }
                }
            }

            return total;
        }

        public decimal EndRent(string id)
        {
            Scooter scooter = ScooterService.GetScooterById(id);
            int year = DateTime.Now.Year;

            scooter.IsRented = false;

            decimal price = rentCalculator.CalculateRent(scooter.TimeRented, DateTime.Now, scooter.PricePerMinute);

            // remove rented scooter from the Dictionary of rented scooters
            foreach (var years in _rentedScooters)
            {
                years.Value.Remove(id);
            }

            // add income from the scooter to current year
            if (_incomeByYear.ContainsKey(year)) { _incomeByYear[year] += price; }
            else { _incomeByYear.Add(year, price); }

            return price;
        }

        public void StartRent(string id)
        {
            var scooter = ScooterService.GetScooterById(id);
            var today = DateTime.Now;

            if (scooter.IsRented)
            {
                Console.WriteLine("Scooter already rented");
                return;
            }

            scooter.TimeRented = today;
            scooter.IsRented = true;

            // add scooter to rented scooter dictionary
            if (_rentedScooters.ContainsKey(today.Year))
            {
                _rentedScooters[today.Year].Add(id);
            }
            else
            {
                _rentedScooters.Add(today.Year, new List<string>() { id });
            }
        }
    }
}
