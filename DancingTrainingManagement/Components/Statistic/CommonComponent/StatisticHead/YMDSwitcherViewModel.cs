using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead
{
    class YMDSwitcherViewModel : ViewModelBase
    {
        private int _ymd;

        public int YMDState
        {
            get { return _ymd; }
            set
            {
                _ymd = value;
                RaisePropertyChanged("YMDState");
            }
        }

        private DelegateCommand _changeYMD;

        public DelegateCommand ChangeYMDState
        {
            get
            {
                _changeYMD = _changeYMD ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        YMDState = int.Parse(o.ToString());
                        YMDStateChangedEvent?.Invoke((YMDType)YMDState);
                    }));
                return _changeYMD;
            }
            set
            {
                _changeYMD = value;
                RaisePropertyChanged("ChangeYMDState");
            }
        }

        public delegate void YMDStateChanged(YMDType ymd);
        public event YMDStateChanged YMDStateChangedEvent;

        public YMDSwitcherViewModel()
        {
            YMDState = 1;
        }
    }
}
