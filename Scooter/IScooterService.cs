using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scooter
{
    interface IScooterService
    {
        /// Add scooter.
        /// parameter id - Unique ID of the scooter
        /// parameter pricePerMinute - Rental price of the scooter per one minute
        void AddScooter(string id, decimal pricePerMinute);

        /// Remove scooter. This action is not allowed for scooters if the rental is in progress.
        /// parameter id - Unique ID of the scooter
        void RemoveScooter(string id);

        /// List of scooters that belong to the company.
        /// returns - Return a list of available scooters.
        List<Scooter> GetScooters();

        /// Get particular scooter by ID.
        /// parameter scooterId - Unique ID of the scooter.
        /// returns - Return a particular scooter.
        Scooter GetScooterById(string scooterId);
    }
}
