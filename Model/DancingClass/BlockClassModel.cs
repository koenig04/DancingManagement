using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BlockClassModel : ClassModel, IComparable<BlockClassModel>
    {
        public int ClassYear { get; set; }
        public int ClassSeason { get; set; }
        public string Trainee { get; set; }
        public override string ClassName
        {
            get
            {
                return ClassYear.ToString() +
                    BlockClassType.Instance.BlockClassSeasonColletion.Where(b => b.Type == ClassSeason).First().Name +
                    "独舞" +
                    BlockClassType.Instance.BlockClassTypeCollection.Where(b => b.Type == ClassType).First().Name +
                    ClassSerial +
                    "班";
            }
        }

        public int CompareTo(BlockClassModel other)
        {
            if (ClassYear > other.ClassYear)
            {
                return 1;
            }
            else if (ClassYear < other.ClassYear)
            {
                return -1;
            }
            else
            {
                if (ClassType > other.ClassType)
                {
                    return 1;
                }
                else if (ClassType < other.ClassType)
                {
                    return -1;
                }
                else
                {
                    if (ClassSerial > other.ClassSerial)
                    {
                        return 1;
                    }
                    else if (ClassSerial < other.ClassSerial)
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
}
