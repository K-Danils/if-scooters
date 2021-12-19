using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scooter
{
    interface IRentCalculator
    {
        /// calculate rent for the scooter
        /// parameter rentStart, rentEnd - times when the scooter was taken and when it was given back
        /// parameter pricePerMinute - price per minute while renting the scooter
        /// returns - total to be payed for the rent
        decimal CalculateRent(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute);

        /// calculate rent for the scooter across multiple days
        /// parameter rentStart, rentEnd - times when the scooter was taken and when it was given back
        /// parameter pricePerMinute - price per minute while renting the scooter
        /// returns - total to be payed for the rent
        decimal CalculateRentForMultipleDays(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute);
    }
}
