using DancingTrainingManagement.UICore;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement.RegularTeaching
{
    class GivingClassViewModel : ViewModelBase
    {
        private string startDate_;

        public string StartDate
        {
            get { return startDate_; }
            set { startDate_ = value; RaisePropertyChanged("StartDate"); }
        }

        private string endDate_;

        public string EndDate
        {
            get { return endDate_; }
            set { endDate_ = value; RaisePropertyChanged("EndDate"); }
        }

        private string givingInterval_;

        public string GivingInterval
        {
            get { return givingInterval_; }
            set { givingInterval_ = value; RaisePropertyChanged("GivingInterval"); }
        }


        private string givingCount_;

        public string GivingCount
        {
            get { return givingCount_; }
            set { givingCount_ = value; RaisePropertyChanged("GivingCount"); }
        }

        private DelegateCommand modify_;

        public DelegateCommand Modify
        {
            get
            {
                modify_ = modify_ ?? new DelegateCommand(new Action<object>(
             o =>
             {
                 GivingModifyEvent?.Invoke(info_);
             }));
                return modify_;
            }
            set
            {
                modify_ = value;
                RaisePropertyChanged("Modify");
            }
        }

        private DelegateCommand del_;

        public DelegateCommand Del
        {
            get
            {
                del_ = del_ ?? new DelegateCommand(new Action<object>(
             o =>
             {
                 GivingDelEvent?.Invoke(info_);
                 Vis = System.Windows.Visibility.Hidden;
             }));
                return del_;
            }
            set
            {
                del_ = value;
                RaisePropertyChanged("Del");
            }
        }        

        public delegate void GivingModify(ClassGivingInfoModel info);
        public event GivingModify GivingModifyEvent;
        public delegate void GivingDel(ClassGivingInfoModel info);
        public event GivingDel GivingDelEvent;

        private ClassGivingInfoModel info_;

        public GivingClassViewModel()
        {
            Vis = System.Windows.Visibility.Hidden;
        }

        public void Active(ClassGivingInfoModel info)
        {

            info_ = info;

            StartDate = info.StartDate.ToShortDateString();
            EndDate = info.EndDateEnable ? info.EndDate.ToShortDateString() : "";
            GivingCount = info.GivingCount.ToString();
            GivingInterval = info.GivingInterval.ToString();
            Vis = System.Windows.Visibility.Visible;
        }
    }
}
