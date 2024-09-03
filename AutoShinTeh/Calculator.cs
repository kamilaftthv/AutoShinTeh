using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShinTeh
{
    internal class Commission
    {
        public decimal CalculateCommission(decimal dailyRevenue, DateTime hireDate)
        {
            decimal commissionRate;
            if (dailyRevenue < 50000)
            {
                commissionRate = 0.03m;
            }
            else
            {
                commissionRate = 0.06m;
            }

            decimal commission = dailyRevenue * commissionRate;

            int yearsOfWork = DateTime.Now.Year - hireDate.Year;
            if (hireDate > DateTime.Now.AddYears(-yearsOfWork))
            {
                yearsOfWork--;
            }

            if (yearsOfWork > 3)
            {
                commission = commission * 1.05m;
            }

            return commission;
        }

        public int CalculateYearsOfWork(DateTime hireDate)
        {
            int years = DateTime.Now.Year - hireDate.Year;
            if (hireDate > DateTime.Now.AddYears(-years))
            {
                years--;
            }

            return years;
        }
    }
}
