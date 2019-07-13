using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RegularChangingModel
    {
        public string RegularChangingID { get; set; }
        public DateTime ChangingDate { get; set; }
        public string RegularClassID { get; set; }
        public int PreviousClassType { get; set; }
        public int PreviousClassSerial { get; set; }
        public int PostClassType { get; set; }
        public int PostClassSerial { get; set; }

        public string PreviousName
        {
            get
            {
                return RegularClassType.Instance.RegularClassTypeCollection.Where(r => r.Level == PreviousClassType).First().Name
                    + PreviousClassSerial.ToString()
                    + "班";
            }
        }

        public string PostName
        {
            get
            {
                return RegularClassType.Instance.RegularClassTypeCollection.Where(r => r.Level == PostClassType).First().Name
                    + PostClassSerial.ToString()
                    + "班";
            }
        }
    }
}
