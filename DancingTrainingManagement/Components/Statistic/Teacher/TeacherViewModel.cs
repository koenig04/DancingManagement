using BLL.StatisticManagement.TeachingStatistic;
using Common;
using DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary;
using DancingTrainingManagement.UICore;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.Statistic.Teacher
{
    class TeacherViewModel : ConcreteStatisticViewModelBase
    {
        private StatisticHeaderViewModel _head;

        public StatisticHeaderViewModel Head
        {
            get { return _head; }
            set
            {
                _head = value;
                RaisePropertyChanged("Head");
            }
        }
        private SeriesCollection _columns;

        public SeriesCollection ColumnColletcion
        {
            get { return _columns; }
            set
            {
                _columns = value;
                RaisePropertyChanged("ColumnColletcion");
            }
        }

        private List<string> _labels;

        public List<string> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                RaisePropertyChanged("Labels");
            }
        }

        private Visibility _chartVis;

        public Visibility ChartVis
        {
            get { return _chartVis; }
            set
            {
                _chartVis = value;
                RaisePropertyChanged("ChartVis");
            }
        }

        private TeacherSummaryViewModel _summary;

        public TeacherSummaryViewModel Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                RaisePropertyChanged("Summary");
            }
        }

        private TeachingStatisticBussiness _bussiness;
        private int _colorIndex = 0;
        public TeacherViewModel(TeachingStatisticBussiness bussiness)
        {
            _bussiness = bussiness;
            Head = new StatisticHeaderViewModel(StatisticFuncEnum.Teacher);
            Head.DoSearchingForTeacherEvent += (statistics, startDate, endDate, isSortedByMonth) =>
            {
                if (CheckValidity(statistics, startDate, endDate))
                {
                    if (statistics.Contains(TeachingStatisticTypeEnum.ClassCount))
                    {
                        _bussiness.SearchTeachingCount(startDate, endDate, isSortedByMonth);
                    }
                }
            };
            Summary = new TeacherSummaryViewModel(bussiness);
            ColumnColletcion = new SeriesCollection();
            Labels = new List<string>();
            ChartVis = Visibility.Hidden;
            _bussiness.TeacherCountInfoChangedEvent += OnTeacherCountInfoChanged;
        }

        private void OnTeacherCountInfoChanged(List<TeachingCountGroup> teachingCountGroups, bool isSortedByMonth)
        {
            ChartVis = Visibility.Visible;
            Labels.Clear();
            ColumnColletcion.Clear();
            Summary.ClearSummary();

            FillLabels(teachingCountGroups[0].ClassCountGroup, isSortedByMonth);
            Summary.CreateSummaries(teachingCountGroups[0].DetailsGroup.Keys.ToArray(), isSortedByMonth);
            foreach (TeachingCountGroup item in teachingCountGroups)
            {
                ColumnSeries col = CreateOneColumn(item.TeacherName, _colorIndex);
                foreach (int amount in item.ClassCountGroup.Values)
                {
                    col.Values.Add((decimal)amount);
                }
                ColumnColletcion.Add(col);
                Summary.AddSummary(item.ClassCountGroup, GlobalVariables.CustomerColorSet[_colorIndex % 12], item.TeacherName, item.TeacherID);
                _colorIndex++;
            }
        }

        private void FillLabels(Dictionary<DateTime, int> groupDetais, bool isSortedByMonth)
        {
            if (Labels.Count == 0)
            {
                foreach (DateTime span in groupDetais.Keys)
                {
                    Labels.Add(isSortedByMonth ? span.ToString("yyyy-MM") : span.ToString("yyyy"));
                }
            }
        }

        private bool CheckValidity(List<TeachingStatisticTypeEnum> statistics, DateTime startDate, DateTime endDate)
        {
            if (statistics == null || statistics.Count == 0)
            {
                return EnableErrMsg(MessageType.SearchingTypeErr);
            }
            return CheckValidity(startDate, endDate);
        }
    }
}
