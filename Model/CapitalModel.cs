using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CapitalModel : IComparable<CapitalModel>
    {
        public int GeneralYear { get; set; }
        public int GeneralMonth { get; set; }
        public decimal Capital { get; set; }

        public int CompareTo(CapitalModel other)
        {
            if (GeneralYear > other.GeneralYear)
            {
                return 1;
            }
            else if (GeneralYear < other.GeneralYear)
            {
                return -1;
            }
            else
            {
                if (GeneralMonth > other.GeneralMonth)
                {
                    return 1;
                }
                else if (GeneralMonth < other.GeneralMonth)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
