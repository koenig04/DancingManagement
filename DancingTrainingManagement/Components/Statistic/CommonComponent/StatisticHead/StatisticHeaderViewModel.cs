using Common;
using DancingTrainingManagement.Components.Statistic.CommonComponent.FilterItems;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead
{
    class StatisticHeaderViewModel : ViewModelBase
    {
        private int _filterType;

        public int FilterType
        {
            get { return _filterType; }
            set
            {
                _filterType = value;
                RaisePropertyChanged("FilterType");
            }
        }

        private FinanceItemsViewModel _finance;

        public FinanceItemsViewModel FinanceFilter
        {
            get { return _finance; }
            set
            {
                _finance = value;
                RaisePropertyChanged("FinanceFilter");
            }
        }

        private TeachingItemsViewModel _teacingFilter;

        public TeachingItemsViewModel TeachingFilter
        {
            get { return _teacingFilter; }
            set
            {
                _teacingFilter = value;
                RaisePropertyChanged("TeachingFilter");
            }
        }

        private DateSelecterViewModel _dateSelecter;

        public DateSelecterViewModel DateSelecter
        {
            get { return _dateSelecter; }
            set
            {
                _dateSelecter = value;
                RaisePropertyChanged("DateSelecter");
            }
        }

        private DelegateCommand _search;

        public DelegateCommand Search
        {
            get
            {
                _search = _search ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        switch (_func)
                        {
                            case StatisticFuncEnum.Finance:
                                DoSearchingEvent?.Invoke(_currentStaticItems, _startDate, _endDate, _isSortedByMonth);
                                break;
                            case StatisticFuncEnum.Teacher:
                                DoSearchingForTeacherEvent?.Invoke(_currentTeachingStatisticItems, _startDate, _endDate, _isSortedByMonth);
                                break;
                        }
                    }));
                return _search;
            }
            set
            {
                _search = value;
                RaisePropertyChanged("Search");
            }
        }

        public delegate void DoSearching(List<StatisticTypeModel> statistics, DateTime startDate, DateTime endDate, bool isSortedByMonth);
        public event DoSearching DoSearchingEvent;
        public delegate void DoSearchingForTeacher(List<TeachingStatisticTypeEnum> statistics, DateTime startDate, DateTime endDate, bool isSortedByMonth);
        public event DoSearchingForTeacher DoSearchingForTeacherEvent;

        private List<StatisticTypeModel> _currentStaticItems;
        private List<TeachingStatisticTypeEnum> _currentTeachingStatisticItems;
        private DateTime _startDate, _endDate;
        private bool _isSortedByMonth;
        private StatisticFuncEnum _func;
        public StatisticHeaderViewModel(StatisticFuncEnum func)
        {
            _func = func;
            DateSelecter = new DateSelecterViewModel();
            DateSelecter.DateSpanChangedEvent += (startDate, endDate, isSortedByMonth) =>
            {
                _startDate = startDate;
                _endDate = endDate;
                _isSortedByMonth = isSortedByMonth;
            };

            FinanceFilter = new FinanceItemsViewModel();
            TeachingFilter = new TeachingItemsViewModel();
            FinanceFilter.OnOperateEnableEvent(true, func == StatisticFuncEnum.Finance ? true : false);
            TeachingFilter.OnOperateEnableEvent(true, func == StatisticFuncEnum.Teacher ? true : false);
            FinanceFilter.FilterItemsChangedEvent += statistics => _currentStaticItems = statistics;
            TeachingFilter.FilterItemsChangedEvent += statistics => _currentTeachingStatisticItems = statistics;
        }
    }
}
