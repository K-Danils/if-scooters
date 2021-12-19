using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scooter
{
    public class ScooterService : IScooterService
    {
        List<Scooter> scooterInventory = new List<Scooter>();

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new Exception("Empty ID was given.");
            }

            foreach (var scooter in scooterInventory)
            {
                if (scooter.Id == id)
                {
                    throw new Exception("ID already taken");
                }
            }
            
            scooterInventory.Add(new Scooter(id, pricePerMinute));
        }

        public Scooter GetScooterById(string scooterId)
        {
            if (String.IsNullOrEmpty(scooterId))
            {
                throw new Exception("Empty ID was given.");
            }

            foreach (Scooter scooter in scooterInventory)
            {
                if (scooter.Id == scooterId)
                {
                    return scooter;
                }
            }

            throw new Exception("No scooter found by id: " + scooterId);
        }

        public List<Scooter> GetScooters()
        {
            return scooterInventory.Where(scooter => !scooter.IsRented).ToList();
        }

        public void RemoveScooter(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new Exception("Empty ID was given.");
            }

            var foundScooter = GetScooterById(id);

            scooterInventory.Remove(foundScooter);
        }
    }
}
