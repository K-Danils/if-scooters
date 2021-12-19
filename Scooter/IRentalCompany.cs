using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scooter
{
    interface IRentalCompany
    {
        /// Name of the company.
        string Name { get; }

        /// Start the rent of the scooter.
        /// parameter id - ID of the scooter
        void StartRent(string id);

        /// End the rent of the scooter.
        /// parameter id - ID of the scooter
        /// returns - The total price of rental. It has to calculated taking into account for how long time scooter was rented.
        /// If total amount per day reaches 20 EUR than timer must be stopped and restarted at beginning of next day at 0:00 am.
        decimal EndRent(string id);

        /// Income report.
        /// paramter year - Year of the report. Sum all years if value is not set.
        /// parameter includeNotCompletedRentals - Include income from the scooters that are rented out (rental has not ended yet) and
        /// calculate rental price as if the rental would end at the time when this report was requested.
        /// returns - The total price of all rentals filtered by year if given.
        decimal CalculateIncome(int? year, bool includeNotCompletedRentals);
    }
}
