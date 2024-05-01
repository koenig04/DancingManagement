using BLL.ClassManagement;
using BLL.NameCallingManagement;
using BLL.OverdueManagement;
using BLL.TeachingManagement.RegularTeaching;
using BLL.TraineeManagement;
using Common;
using DAL;
using Model;
using Model.DancingClass;
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

            List<ClassGivingDetailModel> givings = _calling.GetGivingDetail(trainee.TraineeID);

            List<NameCallingModel> callings;
            if (givings != null)
            {
                //giving exist
                int countPerTerm = 20;
                TraineeModel currentTrainee = _calling.GetTrainee(trainee.TraineeID);
                int classCount = 0;
                if (currentTrainee.RemainRegularCount < 0)
                {
                    classCount = countPerTerm - currentTrainee.RemainRegularCount;
                }
                else
                {
                    if (currentTrainee.RemainRegularCount % countPerTerm != 0)
                    {
                        classCount = countPerTerm - currentTrainee.RemainRegularCount % countPerTerm;
                    }
                }

                if (isCurrentTerm)
                {
                    List<NameCallingModel> presence = _calling.GetListByPresentCount(trainee.TraineeID, classCount * 2);
                    List<ClassGivingDetailModel> givingDetails = _calling.GetGivingDetailByTraineeAndCount(trainee.TraineeID, classCount);
                    callings = CombineNamecallingAndGivingForCurrentTerm(presence, givingDetails, classCount, trainee.TraineeID);
                }else
                {
                    classCount += countPerTerm;
                    List<NameCallingModel> presence = _calling.GetListByPresentCount(trainee.TraineeID, classCount * 2);
                    List<ClassGivingDetailModel> givingDetails = _calling.GetGivingDetailByTraineeAndCount(trainee.TraineeID, classCount);
                }
            }
            else
            {
                callings = isCurrentTerm ?
                   _calling.GetListForCurrentTerm(trainee.TraineeID) :
                   _calling.GetListForPreviousTerm(trainee.TraineeID);
            }
            _currentPresenceInfo = new PresenceInfo();
            _currentPresenceInfo.FillPresence(callings, _regular, trainee.TraineeID, givings == null ? true : false);


            //get payment info
            string paymentDate = string.Empty, paymentCount = string.Empty;
            _paymentDal.GetClassPayment(trainee.TraineeID, isCurrentTerm, ref paymentDate, ref paymentCount);
            _currentPresenceInfo.PaymentDate = paymentDate;
            _currentPresenceInfo.PaymentCount = paymentCount;

            PresenceInfoChangedEvent?.Invoke(_currentPresenceInfo);
        }

        private List<NameCallingModel> CombineNamecallingAndGivingForCurrentTerm(List<NameCallingModel> presence, 
            List<ClassGivingDetailModel> givingDetails, int classCount, string traineeID)
        {
            List<NameCallingModel> res = new List<NameCallingModel>();

            int currentCount = 0;
            int currentGivingIndex = 0;

            for (int i = 0; i < presence.Count; i++)
            {
                if (currentGivingIndex < givingDetails.Count &&
                    givingDetails[currentGivingIndex].GivingDate >= presence[i].ClassDate)
                {
                    res.Add(GenAGivingCalling(traineeID, givingDetails[currentGivingIndex].GivingDate, presence[0].ClassID));
                    currentCount--;
                    currentGivingIndex++;
                }

                res.Add(presence[i]);
                if (presence[i].PresenceTrainees.Contains(traineeID))
                {
                    currentCount++;
                }

                if (currentCount == classCount)
                    break;
            }
            return res;
        }

        private List<NameCallingModel> CombineNamecallingAndGivingForPreviousTerm(List<NameCallingModel> presence,
            List<ClassGivingDetailModel> givingDetails, int classCount, string traineeID)
        {
            List<NameCallingModel> res = new List<NameCallingModel>();

            int presenceTotal = (from d in presence
                                 where d.PresenceTrainees.Contains(traineeID)
                                 select d).Count();
            int givingTotal = (from d in givingDetails
                               where d.GivingDate >= presence.Last().ClassDate
                               select d).Count();
            if (presenceTotal - givingTotal < classCount)
                return res;

            int currentClassCount = classCount - Common.GlobalVariables.ClassCountPerTerm;
            int currentCount = 0;
            int currentGivingIndex = 0;
            int currentPresenceIndex = 0;

            //2 stages

            for (int i = 0; i < presence.Count; i++)
            {
                if (currentGivingIndex < givingDetails.Count &&
                    givingDetails[currentGivingIndex].GivingDate >= presence[i].ClassDate)
                {
                    res.Add(GenAGivingCalling(traineeID, givingDetails[currentGivingIndex].GivingDate, presence[0].ClassID));
                    currentCount--;
                    currentGivingIndex++;
                }

                res.Add(presence[i]);
                if (presence[i].PresenceTrainees.Contains(traineeID))
                {
                    currentCount++;
                }

                if (currentCount == currentClassCount)
                {
                    currentPresenceIndex = i;
                    break;
                }
            }
        }

        private NameCallingModel GenAGivingCalling(string traineeID, DateTime classDate, string classID)
        {
            NameCallingModel tempNameCalling = new NameCallingModel();
            tempNameCalling.ClassID = classID;
            tempNameCalling.ClassDate = classDate;
            tempNameCalling.Presence = "";
            tempNameCalling.Leave = "";
            tempNameCalling.Giving = traineeID;
            tempNameCalling.Absence = "";
            return tempNameCalling;
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

        public void FillPresence(List<NameCallingModel> callings, RegularClassManagement regular, string traineeID, bool isASC = true)
        {
            Details = new List<PresenceDetail>();

            if (isASC)
            {
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
            else
            {
                if (callings == null)
                {
                    callings = new List<NameCallingModel>();
                }
                for (int i = callings.Count - 1; i >= 0; i--)
                {
                    PresenceDetail detail = new PresenceDetail();
                    detail.ClassDate = callings[i].ClassDate;
                    detail.ClassID = callings[i].ClassID;
                    detail.ClassName = regular.GetClassNameInHis(callings[i].ClassID, callings[i].ClassDate);
                    if (callings[i].PresenceTrainees.Contains(traineeID))
                    {
                        detail.State = CallingState.Presence;
                    }
                    else if (callings[i].LeaveTrainees.Contains(traineeID))
                    {
                        detail.State = CallingState.Leave;
                    }
                    else if (callings[i].AbsenceTrainees.Contains(traineeID))
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
