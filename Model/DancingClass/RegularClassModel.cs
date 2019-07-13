using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class RegularClassModel : ClassModel, IComparable<RegularClassModel>
    {
        public override string ClassName
        {
            get
            {
                return RegularClassType.Instance.RegularClassTypeCollection.Where(r => r.Level == ClassType).First().Name
                    + ClassSerial.ToString()
                    + "班";
            }
        }
        

        public int CompareTo(RegularClassModel other)
        {
            if (this.ClassType > other.ClassType)
            {
                return 1;
            }
            else if (this.ClassType < other.ClassType)
            {
                return -1;
            }
            else
            {
                if (this.ClassSerial > other.ClassSerial)
                {
                    return 1;
                }
                else if (this.ClassSerial < other.ClassSerial)
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
