using BLL.TeachingManagement.RegularTeaching;
using DancingTrainingManagement.Components.CommonComponent.CheckBox;
using DancingTrainingManagement.UICore;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement.RegularTeaching
{
    class RegularClassGivingOperationViewModel : ViewModelBase
    {
        private DateTime startDate_;

        public DateTime StartDate
        {
            get { return startDate_; }
            set { startDate_ = value; RaisePropertyChanged("StartDate"); }
        }

        private DateTime endDate_;

        public DateTime EndDate
        {
            get { return endDate_; }
            set { endDate_ = value; RaisePropertyChanged("EndDate"); }
        }

        private bool _endDateEnable;

        public bool EndDateEnable
        {
            get { return _endDateEnable; }
            set { _endDateEnable = value; RaisePropertyChanged("EndDateEnable"); }
        }

        private CheckBoxViewModel endDateChb_;

        public CheckBoxViewModel EndDataChb
        {
            get { return endDateChb_; }
            set { endDateChb_ = value; RaisePropertyChanged("EndDataChb"); }
        }

        private string interval_;

        public string GivingInterval
        {
            get { return interval_; }
            set { interval_ = value; RaisePropertyChanged("GivingInterval"); }
        }

        private string count_;

        public string GivingCount
        {
            get { return count_; }
            set { count_ = value; RaisePropertyChanged("GivingCount"); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private DelegateCommand cancel_;

        public DelegateCommand Cancel
        {
            get
            {
                return cancel_ = cancel_ ?? new DelegateCommand(new Action<object>(
                  o =>
                  {
                      Vis = System.Windows.Visibility.Hidden;
                  }));
            }
            set { cancel_ = value; RaisePropertyChanged("Cancel"); }
        }

        private DelegateCommand confirm_;

        public DelegateCommand Confirm
        {
            get
            {
                return confirm_ = confirm_ ?? new DelegateCommand(new Action<object>(
                o =>
                {
                    if (CheckValidation())
                    {
                        ErrVis = System.Windows.Visibility.Collapsed;
                        ClassGivingInfoModel model = new ClassGivingInfoModel();
                        model.ClassID = currentClassID_;
                        model.StartDate = StartDate;
                        if (EndDateEnable)
                            model.EndDate = EndDate;
                        model.GivingInterval = int.Parse(GivingInterval);
                        model.GivingCount = int.Parse(GivingCount);
                        model.EndDataEnable = EndDateEnable;
                        _bussiness.AddNewGiving(model);
                    }
                    else
                    {
                        ErrVis = System.Windows.Visibility.Visible;
                    }
                }));
            }
            set { confirm_ = value; }
        }

        private System.Windows.Visibility errVis_;

        public System.Windows.Visibility ErrVis
        {
            get { return errVis_; }
            set { errVis_ = value; RaisePropertyChanged("ErrVis"); }
        }


        private RegularClassOperationBussiness _bussiness;
        private string currentClassID_;

        public RegularClassGivingOperationViewModel(RegularClassOperationBussiness bussiness) : base()
        {
            _bussiness = bussiness;
            EndDataChb.CheckboxStatusChangedEvent += (enable) => EndDateEnable = enable;
            Vis = System.Windows.Visibility.Hidden;
        }

        public void AddNewGiving(string classID)
        {
            currentClassID_ = classID;
            EndDataChb.resetStatus();
            GivingCount = string.Empty;
            GivingInterval = string.Empty;
            Title = "新增送课";
            ErrVis = System.Windows.Visibility.Collapsed;
            StartDate = DateTime.Now;
            Vis = System.Windows.Visibility.Visible;

        }

        private bool CheckValidation()
        {
            if (EndDateEnable && EndDate <= StartDate)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(GivingInterval) || string.IsNullOrWhiteSpace(GivingCount))
            {
                return false;
            }

            int parseRes;
            if ((!int.TryParse(GivingInterval, out parseRes)) || (!int.TryParse(GivingCount, out parseRes)))
            {
                return false;
            }

            return true;
        }
    }
}
