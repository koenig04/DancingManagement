using BLL.ClassManagement;
using BLL.NameCallingManagement;
using BLL.OverdueManagement;
using BLL.TeachingManagement.RegularTeaching;
using BLL.TraineeManagement;
using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.TraineeStatistic
{
    public class PresenceBussiness
    {
        public delegate void RegularClassChanged();
        public event RegularClassChanged RegularClassChangedEvent;
        public delegate void TraineeChanged(List<TraineeModel> trainees);
        public event TraineeChanged TraineeChangedEvent;
        public delegate void PresenceInfoChanged(PresenceInfo info);
        public event PresenceInfoChanged PresenceInfoChangedEvent;

        private RegularClassManagement _regular;
        private TraineeManagementBussiness _trainees;
        private RegularTraineeBussiness _regularTrainees;
        private OverdueManagementBussiness _overdue;
        private NameCallingBussiness _calling;
        private PaymentInfo _paymentDal;

        private List<TraineeModel> _currentTrainees;
        private PresenceInfo _currentPresenceInfo;
        public PresenceBussiness(TraineeManagementBussiness trainees, RegularClassManagement regular,
            RegularTraineeBussiness regularTrainees, OverdueManagementBussiness overdue, NameCallingBussiness calling,
             PaymentInfo paymentDal)
        {
            _regular = regular;
            _trainees = trainees;
            _overdue = overdue;
            _calling = calling;
            _regularTrainees = regularTrainees;
            _regular.RegularClassChangedEvent += (op, model, index) => RegularClassChangedEvent?.Invoke();
            _regularTrainees.LoadTraineesEvent += traineeCollection =>
            {
                _currentTrainees = traineeCollection;
                TraineeChangedEvent?.Invoke(_currentTrainees);
            };
            _paymentDal = paymentDal;
        }

        public List<RegularClassModel> GetRegularClasses()
        {
            return _regular.RegularClassCollection;
        }

        public void GetTrainees(string className)
        {
            _regularTrainees.GetTrainees(_regular.RegularClassCollection.Where(c => c.ClassName == className).First().ClassID);
        }

        public void GetRegularPresence(string traineeName, bool isCurrentTerm)
        {
            //get trainee info
            TraineeModel trainee = _currentTrainees.Where(t => t.TraineeName == traineeName).First();

            List<NameCallingModel> callings = isCurrentTerm ?
                _calling.GetListForCurrentTerm(trainee.TraineeID) :
                _calling.GetListForPreviousTerm(trainee.TraineeID);
            _currentPresenceInfo = new PresenceInfo();
            _currentPresenceInfo.FillPresence(callings, _regular, trainee.TraineeID);

            //get payment info
            string paymentDate = string.Empty, paymentCount = string.Empty;
            _paymentDal.GetClassPayment(trainee.TraineeID, isCurrentTerm, ref paymentDate, ref paymentCount);
            _currentPresenceInfo.PaymentDate = paymentDate;
            _currentPresenceInfo.PaymentCount = paymentCount;

            PresenceInfoChangedEvent?.Invoke(_currentPresenceInfo);
        }
    }

    public class PresenceInfo
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PresenceDetail> Details { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentCount { get; set; }
        public int TotalCount
        {
            get
            {
                return Details.Count();
            }
        }
        public int PresenceCount
        {
            get
            {
                return Details.Count(d => d.State == CallingState.Presence);
            }
        }
        public int AbsenceCount
        {
            get
            {
                return Details.Count(d => d.State == CallingState.Absence);
            }
        }
        public int GivingCount
        {
            get
            {
                return Details.Count(d => d.State == CallingState.Giving);
            }
        }
        public int LeaveCount
        {
            get
            {
                return Details.Count(d => d.State == CallingState.Leave);
            }
        }

        public void FillPresence(List<NameCallingModel> callings, RegularClassManagement regular, string traineeID)
        {
            Details = new List<PresenceDetail>();
            foreach (NameCallingModel item in callings ?? new List<NameCallingModel>())
            {
                PresenceDetail detail = new PresenceDetail();
                detail.ClassDate = item.ClassDate;
                detail.ClassID = item.ClassID;
                detail.ClassName = regular.GetClassNameInHis(item.ClassID, item.ClassDate);
                if (item.PresenceTrainees.Contains(traineeID))
                {
                    detail.State = CallingState.Presence;
                }
                else if (item.LeaveTrainees.Contains(traineeID))
                {
                    detail.State = CallingState.Leave;
                }
                else if (item.AbsenceTrainees.Contains(traineeID))
                {
                    detail.State = CallingState.Absence;
                }
                else
                {
                    detail.State = CallingState.Giving;
                }
                Details.Add(detail);
            }
        }
    }

    public class PresenceDetail : IComparable<PresenceDetail>
    {
        public DateTime ClassDate { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public CallingState State { get; set; }

        public int CompareTo(PresenceDetail other)
        {
            if (ClassDate > other.ClassDate)
            {
                return 1;
            }
            else if (ClassDate < other.ClassDate)
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
