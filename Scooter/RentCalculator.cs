using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scooter
{
    public class RentCalculator : IRentCalculator
    {
        private readonly decimal _dailyMaximum = 20;

        public RentCalculator()
        {
        }

        public decimal CalculateRent(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute)
        {
            CheckForExceptions(rentStart, rentEnd);

            decimal total = 0;
            var resetDate = new DateTime(rentStart.Year, rentStart.Month, rentStart.Day + 1, 0, 0, 0);
            TimeSpan timePassed = rentEnd - rentStart;

            // if no days have passed, return the total price of the day
            if (Math.Floor(timePassed.TotalDays) == 0 && rentStart.Day == rentEnd.Day)
            {
                decimal price = (decimal)timePassed.TotalMinutes * pricePerMinute;

                return GetPriceOverflow(price);
            }
            // Calculate price if scooter wasn't given back until the next day, but 24h haven't passed
            else if (Math.Floor(timePassed.TotalDays) == 0)
            {
                TimeSpan priceForToday = resetDate - rentStart;
                TimeSpan priceForTommorow = rentEnd - resetDate;
                decimal firstDayPrice = (decimal)priceForToday.TotalMinutes * pricePerMinute;
                decimal secondDayPrice = (decimal)priceForTommorow.TotalMinutes * pricePerMinute;

                return GetPriceOverflow(firstDayPrice) + GetPriceOverflow(secondDayPrice);
            }
            // calculate price when 1 or more days have passed
            else
            {
                return CalculateRentForMultipleDays(rentStart, rentEnd, pricePerMinute);
            }
        }

        public decimal CalculateRentForMultipleDays(DateTime rentStart, DateTime rentEnd, decimal pricePerMinute)
        {
            CheckForExceptions(rentStart, rentEnd);

            TimeSpan timePassed = rentEnd - rentStart;
            var resetDate = new DateTime(rentEnd.Year, rentEnd.Month, rentEnd.Day, 0, 0, 0);
            var minutesInDay = 1440;
            decimal total = 0;
            decimal daysPassedInMinutes;
            decimal remainingMinutes;
            decimal price;
            double days;

            // in case price per day never reaches daily maximum return total minutes passed * price per minute
            if (minutesInDay * pricePerMinute < _dailyMaximum) { return (decimal)timePassed.TotalMinutes * pricePerMinute; }

            // check the rent end is the next day of the rent start.
            // This is done because it sometimes incorrectly calculates the amount of days passed, 
            // usually having 1 day less than actually has passed.
            // So below is a workaround to ensure that it always works correctly.

            days = rentEnd.Day == rentStart.Day + 1 ? days = Math.Floor(timePassed.TotalDays) : days = Math.Floor(timePassed.TotalDays) + 1;

            // since a day passed means and daily maximum was reached, multiply days by daily maximum
            total += _dailyMaximum * (decimal)days;

            // get the rent price of the first day, since the price for the first day can be below dailyMaximum
            TimeSpan timePassedDuringFirstDay = new DateTime(rentStart.Year, rentStart.Month, rentStart.Day + 1, 0, 0, 0) - rentStart;
            var priceForFirstDay = (decimal)timePassedDuringFirstDay.TotalMinutes * pricePerMinute;
            total += GetPriceOverflow(priceForFirstDay);

            // set rentStart to the resetTime, which is last day of the rent 00:00 am and calculate the remaining minutes during the last day of the rent
            rentStart = resetDate;
            timePassed = rentEnd - rentStart;
            daysPassedInMinutes = (decimal)timePassed.Days * 24 * 60;
            remainingMinutes = (decimal)Math.Round(timePassed.TotalMinutes) - daysPassedInMinutes;
            price = remainingMinutes * pricePerMinute;
            total += GetPriceOverflow(price);

            return total - _dailyMaximum;
        }

        private static void CheckForExceptions(DateTime rentStart, DateTime rentEnd)
        {
            if (rentStart == null || rentEnd == null)
            {
                throw new Exception("Given dates were null");
            }

            if (rentEnd < rentStart)
            {
                throw new Exception("Given time for rent end is impossible.\n" +
            "Rent end date time is before rent start?");
            }
        }

        private decimal GetPriceOverflow(decimal price)
        {
            // get price overflow if price is higher than daily maximum, then return daily maximum
            // if price is lower than daily maximum, then return price
            return price > _dailyMaximum ? _dailyMaximum : price;
        }
    }
}
