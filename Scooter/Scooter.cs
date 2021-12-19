using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scooter
{
    public class Scooter
    {
        public Scooter(string id, decimal pricePerMinute)
        {
            Id = id;
            PricePerMinute = pricePerMinute;
        }

        public string Id { get; }

        public decimal PricePerMinute { get; }

        public bool IsRented { get; set; }

        public DateTime TimeRented { get; set; }
    }
}
