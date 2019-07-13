using BLL.PaymentManagement.ClassPayment;
using BLL.TeacherManagement;
using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Payment.ClassPayment.TeacherFee
{
    class TeacherFeeViewModel : ViewModelBase
    {
        private string _selectYear;

        public string SelectYear
        {
            get { return _selectYear; }
            set
            {
                _selectYear = value;
                QuerryClassCount();
                RaisePropertyChanged("SelectYear");
            }
        }

        private List<string> _years;

        public List<string> YearCollection
        {
            get { return _years; }
            set
            {
                _years = value;
                RaisePropertyChanged("YearCollection");
            }
        }

        private string _selectMonth;

        public string SelectMonth
        {
            get { return _selectMonth; }
            set
            {
                _selectMonth = value;
                QuerryClassCount();
                RaisePropertyChanged("SelectMonth");
            }
        }

        private List<string> _months;

        public List<string> MonthCollection
        {
            get { return _months; }
            set
            {
                _months = value;
                RaisePropertyChanged("MonthCollection");
            }
        }

        private string _selectedTeacher;

        public string SelectedTeacher
        {
            get { return _selectedTeacher; }
            set
            {
                _selectedTeacher = value;
                if (!string.IsNullOrEmpty(value))
                {
                    TeacherFee = TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherName == SelectedTeacher).First().TeacherFee.ToString();
                }
                QuerryClassCount();
                RaisePropertyChanged("SelectedTeacher");
            }
        }

        private List<string> _teachers;

        public List<string> TeacherCollection
        {
            get { return _teachers; }
            set
            {
                _teachers = value;
                RaisePropertyChanged("TeacherCollection");
            }
        }

        private string _classCount;

        public string ClassCount
        {
            get { return _classCount; }
            set
            {
                _classCount = value;
                RaisePropertyChanged("ClassCount");
            }
        }

        private string _teacherFee;

        public string TeacherFee
        {
            get { return _teacherFee; }
            set
            {
                _teacherFee = value;
                RaisePropertyChanged("TeacherFee");
            }
        }

        private string _teeTotal;

        public string TeacherFeeTotal
        {
            get { return _teeTotal; }
            set
            {
                _teeTotal = value;
                RaisePropertyChanged("TeacherFeeTotal");
            }
        }

        private DelegateCommand _confirm;

        public DelegateCommand Confirm
        {
            get
            {
                _confirm = _confirm ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (CheckValidity())
                        {
                            if (_bussiness.AddTeacherFee(TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherName == SelectedTeacher).First().TeacherID,
                                int.Parse(SelectYear),
                                int.Parse(SelectMonth),
                                decimal.Parse(TeacherFeeTotal)))
                            {
                                RaiseErrOccuredEvent(MessageType.TeacherFeeDupliPaidErr);
                            }
                            else
                            {
                                ShowInfoEvent?.Invoke(MessageType.TeacherFeePaidSuccess);
                            }
                        }
                    }));
                return _confirm;
            }
            set
            {
                _confirm = value;
                RaisePropertyChanged("Confirm");
            }
        }

        public delegate void ErrOccured(MessageType msg, MessageLevel level = MessageLevel.Warning);
        public event ErrOccured ErrOccuredEvent;
        public delegate void ShowInfo(MessageType msg, MessageLevel level = MessageLevel.Info);
        public event ShowInfo ShowInfoEvent;

        private TeacherFeeBussiness _bussiness;
        public TeacherFeeViewModel(TeacherFeeBussiness bussiness)
        {
            _bussiness = bussiness;
            _bussiness.ClassCountChangedEvent += count =>
            {
                ClassCount = count.ToString();
                if (!string.IsNullOrEmpty(TeacherFee))
                {
                    TeacherFeeTotal = (count * decimal.Parse(TeacherFee)).ToString();
                }
            };

            SelectYear = string.Empty;
            SelectMonth = string.Empty;
            SelectedTeacher = string.Empty;
            MonthCollection = new List<string>();
            YearCollection = new List<string>();
            TeacherCollection = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                MonthCollection.Add(i.ToString());
            }
            for (int i = 2019; i < (2019 + 21); i++)
            {
                YearCollection.Add(i.ToString());
            }

            TeacherManagementBussiness.Instance.Teachers.ForEach(t => TeacherCollection.Add(t.TeacherName));
            TeacherManagementBussiness.Instance.TeacherChangedEvent += (operation, teacher, previousTeacher) =>
            {
                if (operation == OperationType.Update)
                {
                    TeacherCollection.Remove(previousTeacher.TeacherName);
                }
                TeacherCollection.Add(teacher.TeacherName);
            };
        }

        private void QuerryClassCount()
        {
            if (!string.IsNullOrEmpty(SelectYear) && !string.IsNullOrEmpty(SelectMonth) && !string.IsNullOrEmpty(SelectedTeacher))
            {
                _bussiness.QuerryClassCount(TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherName == SelectedTeacher).First().TeacherID,
                    DateTime.Parse(string.Format("{0}-{1}-1", SelectYear, SelectMonth)),
                    DateTime.Parse(string.Format("{0}-{1}-1", SelectYear, SelectMonth)).AddMonths(1).AddDays(-1));
            }
        }

        private bool CheckValidity()
        {

            if (string.IsNullOrEmpty(SelectMonth))
            {
                return RaiseErrOccuredEvent(MessageType.MonthErr);
            }
            if (string.IsNullOrEmpty(SelectYear))
            {
                return RaiseErrOccuredEvent(MessageType.YearErr);
            }
            if (string.IsNullOrEmpty(SelectedTeacher))
            {
                return RaiseErrOccuredEvent(MessageType.TeacherErr);
            }
            return true;
        }

        private bool RaiseErrOccuredEvent(MessageType msg)
        {
            ErrOccuredEvent?.Invoke(msg, MessageLevel.Warning);
            return false;
        }
    }
}
