using Common;
using DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead.FilterItems;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.FilterItems
{
    class FinanceItemsViewModel : ViewModelBase
    {
        private FinanceFilterItemViewModel _allIncome;

        public FinanceFilterItemViewModel AllIncome
        {
            get { return _allIncome; }
            set
            {
                _allIncome = value;
                RaisePropertyChanged("AllIncome");
            }
        }

        private FinanceFilterItemViewModel _allCoutcome;

        public FinanceFilterItemViewModel AllOutcome
        {
            get { return _allCoutcome; }
            set
            {
                _allCoutcome = value;
                RaisePropertyChanged("AllOutcome");
            }
        }

        private FinanceFilterItemViewModel _diff;

        public FinanceFilterItemViewModel Diff
        {
            get { return _diff; }
            set
            {
                _diff = value;
                RaisePropertyChanged("Diff");
            }
        }

        private FinanceFilterItemViewModel _capital;

        public FinanceFilterItemViewModel Capital
        {
            get { return _capital; }
            set { _capital = value;
                RaisePropertyChanged("Capital");
            }
        }


        private ObservableCollection<FinanceFilterItemViewModel> _incomes;

        public ObservableCollection<FinanceFilterItemViewModel> IncomeItems
        {
            get { return _incomes; }
            set
            {
                _incomes = value;
                RaisePropertyChanged("IncomeItems");
            }
        }

        private ObservableCollection<FinanceFilterItemViewModel> _outcomes;

        public ObservableCollection<FinanceFilterItemViewModel> OutcomeItems
        {
            get { return _outcomes; }
            set
            {
                _outcomes = value;
                RaisePropertyChanged("OutcomeItems");
            }
        }

        public delegate void FilterItemsChanged(List<StatisticTypeModel> statisticType);
        public event FilterItemsChanged FilterItemsChangedEvent;

        private List<StatisticTypeEnum> _allCollection;

        private List<StatisticTypeModel> _allStatistic;
        private List<StatisticTypeModel> _detailStatistic;
        public FinanceItemsViewModel()
        {
            AllIncome = new FinanceFilterItemViewModel(StatisticTypeEnum.AllIncome);
            AllOutcome = new FinanceFilterItemViewModel(StatisticTypeEnum.AllOutcome);
            Diff = new FinanceFilterItemViewModel(StatisticTypeEnum.Diff);
            Capital = new FinanceFilterItemViewModel(StatisticTypeEnum.Capital);
            AllIncome.FilterItemClickedEvent += OnFilterItemClickedEvent;
            AllOutcome.FilterItemClickedEvent += OnFilterItemClickedEvent;
            Diff.FilterItemClickedEvent += OnFilterItemClickedEvent;
            Capital.FilterItemClickedEvent += OnFilterItemClickedEvent;

            List<StatisticTypeEnum> incomeItems = new List<StatisticTypeEnum>() { StatisticTypeEnum.ClassFee, StatisticTypeEnum.Exam,StatisticTypeEnum.ThingIncome,
                StatisticTypeEnum.Investment, StatisticTypeEnum.OtherIncome };
            List<StatisticTypeEnum> outcomeItems = new List<StatisticTypeEnum>() { StatisticTypeEnum.TeacherFee, StatisticTypeEnum.Salary, StatisticTypeEnum.Bonus,
                 StatisticTypeEnum.Water, StatisticTypeEnum.Elec, StatisticTypeEnum.Internet, StatisticTypeEnum.ThingCost,StatisticTypeEnum.Rent, StatisticTypeEnum.OtherOutcome };
            IncomeItems = new ObservableCollection<FinanceFilterItemViewModel>();
            OutcomeItems = new ObservableCollection<FinanceFilterItemViewModel>();
            foreach (StatisticTypeEnum item in incomeItems)
            {
                IncomeItems.Add(new FinanceFilterItemViewModel(item));
                IncomeItems.Last().FilterItemClickedEvent += OnFilterItemClickedEvent;
            }
            foreach (StatisticTypeEnum item in outcomeItems)
            {
                OutcomeItems.Add(new FinanceFilterItemViewModel(item));
                OutcomeItems.Last().FilterItemClickedEvent += OnFilterItemClickedEvent;
            }
            _allCollection = new List<StatisticTypeEnum>() { StatisticTypeEnum.AllIncome, StatisticTypeEnum.AllOutcome, StatisticTypeEnum.Diff, StatisticTypeEnum.Capital};

            _allStatistic = new List<StatisticTypeModel>();
            _detailStatistic = new List<StatisticTypeModel>();
        }

        private void OnFilterItemClickedEvent(StatisticTypeModel statisticType, bool isSelected)
        {
            if (isSelected)
            {
                if (_allCollection.Contains(statisticType.StatisticType))
                {
                    _detailStatistic.Clear();
                    if(statisticType.StatisticType== StatisticTypeEnum.Capital)
                    {
                        AllIncome.ChangeSelectionState(false);
                        AllOutcome.ChangeSelectionState(false);
                        Diff.ChangeSelectionState(false);
                        _allStatistic.Clear();
                    }
                    else
                    {
                        Capital.ChangeSelectionState(false);
                        int capitalIndex = _allStatistic.IndexOf(_allStatistic.Where(s => s.StatisticType == StatisticTypeEnum.Capital).FirstOrDefault());
                        if(capitalIndex>=0)
                        {
                            _allStatistic.RemoveAt(capitalIndex);
                        }
                    }

                    _allStatistic.Add(statisticType);

                    foreach (FilterItemViewModel item in IncomeItems)
                    {
                        item.ChangeSelectionState(false);
                    }
                    foreach (FilterItemViewModel item in OutcomeItems)
                    {
                        item.ChangeSelectionState(false);
                    }

                }
                else
                {
                    _allStatistic.Clear();
                    _detailStatistic.RemoveAll(s => s.Catogery != statisticType.Catogery);
                    _detailStatistic.Add(statisticType);

                    AllIncome.ChangeSelectionState(false);
                    AllOutcome.ChangeSelectionState(false);
                    Diff.ChangeSelectionState(false);
                    Capital.ChangeSelectionState(false);
                    foreach (FilterItemViewModel item in (statisticType.Catogery == StatisticCategoryEnum.Income ? OutcomeItems : IncomeItems))
                    {
                        item.ChangeSelectionState(false);
                    }
                }
            }
            else
            {
                if (_allCollection.Contains(statisticType.StatisticType))
                {
                    _allStatistic.Remove(statisticType);
                }
                else
                {
                    _detailStatistic.Remove(statisticType);
                }
            }
            RaiseFilterItemsChangedEvent();
        }

        private void RaiseFilterItemsChangedEvent()
        {
            if (_allStatistic.Count > 0)
            {
                FilterItemsChangedEvent?.Invoke(_allStatistic);
            }
            else if (_detailStatistic.Count > 0)
            {
                FilterItemsChangedEvent?.Invoke(_detailStatistic);
            }
            else
            {
                FilterItemsChangedEvent?.Invoke(null);
            }
        }
    }
}
