using BLL.ClassManagement;
using BLL.NameCallingManagement;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace BLL.StatisticManagement.ClassStatistic
{
    public class AttendenceBussiness
    {
        public delegate void ClassInfoChanged(List<RegularClassModel> info);
        public event ClassInfoChanged ClassInfoChangedEvent;

        private NameCallingBussiness nameCalling_;
        private RegularClassManagement regularClass_;

        public AttendenceBussiness(RegularClassManagement regularClass, NameCallingBussiness calling)
        {
            nameCalling_ = calling;
            regularClass_ = regularClass;

            regularClass_.RegularClassChangedEvent += OnClassInfoChanged;
        }

        private void OnClassInfoChanged(OperationType operation, RegularClassModel model, int newIndex)
        {
            ClassInfoChangedEvent?.Invoke(regularClass_.GetClassList());
        }

        public double GetAttendenceRatio(string classID, DateTime startDate, DateTime endDate, bool isGeneral)
        {
            List<NameCallingModel> records = nameCalling_.GetListByClass(classID, startDate, endDate, isGeneral);
            Int64 presenceCount = 0;
            Int64 levelCount = 0;
            if (records == null)
            {
                return 0;
            }
            else
            {
                foreach (var record in records)
                {
                    presenceCount += record.PresenceTrainees == null ? 0 : record.PresenceTrainees.Count();
                    levelCount += record.LeaveTrainees == null ? 0 : record.LeaveTrainees.Count();
                }

                return (double)presenceCount / (double)(presenceCount + levelCount);
            }
        }

        public List<RegularClassModel> GetClassInfo()
        {
            return regularClass_.GetClassList();
        }
    }
}
