using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TraineeModel : IComparable<TraineeModel>, ICloneable
    {
        public string TraineeID { get; set; }
        public string TraineeIDForShown
        {
            get
            {
                return TraineeID.Substring(2, TraineeID.Length - 2).PadLeft(4, '0');
            }
        }
        public string TraineeName { get; set; }
        public string RegularClassID { get; set; }
        public string RegularClassName { get; set; }
        public int RemainRegularCount { get; set; }
        public int InitialRemainRegularCount { get; set; }
        public int State { get; set; }
        public string CurrentBlockID { get; set; }
        public bool IsNew { get; set; }
        public DateTime CreateDate { get; set; }
        public int CallingState { get; set; }


        public int CompareTo(TraineeModel other)
        {
            if (this.State > other.State)
            {
                return 1;
            }
            else if (this.State < other.State)
            {
                return -1;
            }
            else
            {
                if(int.Parse(this.TraineeIDForShown)>int.Parse(other.TraineeIDForShown))
                {
                    return 1;
                }
                else if(int.Parse(this.TraineeIDForShown) < int.Parse(other.TraineeIDForShown))
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
