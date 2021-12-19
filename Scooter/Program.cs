using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scooter
{
    class Program
    {
        static void Main(string[] args)
        {

            var rentStart = new DateTime(2021, 12, 18, 13, 0, 0);
            var rentEnd = new DateTime(2021, 12, 19, 13, 15, 0);

            var rent = new RentCalculator();

            decimal expectedPrice = 40;

            Console.WriteLine(expectedPrice + " " + rent.CalculateRent(rentStart, rentEnd, 1m));

            Console.ReadKey();
        }
    }
}
