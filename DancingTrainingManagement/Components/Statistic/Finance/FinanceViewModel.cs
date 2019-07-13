using BLL.StatisticManagement.FinanceStatistic;
using Common;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.UICore;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Payment;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Windows;
using DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary;

namespace DancingTrainingManagement.Components.Statistic.Finance
{
    class FinanceViewModel : ConcreteStatisticViewModelBase
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

        private OperationMessageViewModel _operation;

        public OperationMessageViewModel OperationMsg
        {
            get { return _operation; }
            set
            {
                _operation = value;
                RaisePropertyChanged("OperationMsg");
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

        private FinanceSummaryViewModel _summary;

        public FinanceSummaryViewModel Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                RaisePropertyChanged("Summary");
            }
        }

        private FinanceStatisticBussiness _bussiness;
        private StatisticTypeEnum _deletedType;
        private string _deletedID;
        private int _colorIndex = 0;
        public FinanceViewModel(FinanceStatisticBussiness bussiness) : base()
        {
            _bussiness = bussiness;
            Head = new StatisticHeaderViewModel(StatisticFuncEnum.Finance);
            Head.DoSearchingEvent += (statistics, startDate, endDate, isSortedByMonth) =>
            {
                if (CheckValidity(statistics, startDate, endDate))
                    _bussiness.Search(statistics, startDate, endDate, isSortedByMonth);
            };
            Summary = new FinanceSummaryViewModel(bussiness);
            Summary.DeleteRecordEvent += (statisticType, accountID, accountDate, accountItem, accountAmount, note) =>
              {
                  _deletedID = accountID;
                  _deletedType = statisticType;
                  OperationMsg.Enable(accountDate, accountItem, accountAmount, note);
              };

            OperationMsg = new OperationMessageViewModel(false, "删除");
            OperationMsg.OnOperateEnableEvent(false, false);
            OperationMsg.ConfirmOperationEvent += OnConfirmOperation;

            ColumnColletcion = new SeriesCollection();
            Labels = new List<string>();
            ChartVis = Visibility.Hidden;

            _bussiness.FinanceStatisticChangedEvent += OnFinanceStatisticChanged;
            _bussiness.CapitalChangedEvent += OnCapitalChanged;
        }

        private void OnConfirmOperation()
        {
            _bussiness.DeletePayment(_deletedType, _deletedID);
            _bussiness.Refresh();
        }

        private void OnCapitalChanged(FinanceStatisticGroup<CapitalModel> capitals, bool isSortedByMonth)
        {
            ChartVis = Visibility.Visible;
            Labels.Clear();
            ColumnColletcion.Clear();
            Summary.ClearSummary();

            FillLabels(capitals.GroupDetais, isSortedByMonth);
            Summary.CreateSummaries(capitals.GroupDetais.Keys.ToArray(), isSortedByMonth);
            LineSeries line = new LineSeries
            {
                Title = capitals.GroupName,
                DataLabels = true,
                Stroke = new SolidColorBrush(GlobalVariables.MainColor),
                LineSmoothness = 0,
                LabelPoint = point => point.Y.ToString("#0.00"),
                FontSize = 16,
                Fill = new SolidColorBrush(Colors.Transparent),
                Values = new ChartValues<decimal>()
            };
            foreach (decimal amount in capitals.GroupDetais.Values)
            {
                line.Values.Add(amount);
            }
            ColumnColletcion.Add(line);
            Summary.AddSummary(capitals.GroupDetais, GlobalVariables.MainColor,
                capitals.GroupName, capitals.StatisticType);
        }

        private void OnFinanceStatisticChanged(List<StatisticTypeModel> statistics, List<FinanceStatisticGroup<AccountInfoModel>> normalStatistic,
            FinanceStatisticGroup<TeacherFeeModel> teacherFeeStatistic, FinanceStatisticGroup<ClassPaymentModel> classpaymentStatistic,
            Dictionary<DateTime, decimal> diffStatisticGroup, bool isSortedByMonth)
        {
            ChartVis = Visibility.Visible;
            Labels.Clear();
            ColumnColletcion.Clear();
            Summary.ClearSummary();

            if (statistics.Where(s => s.StatisticType == StatisticTypeEnum.AllIncome).FirstOrDefault() != null)
            {
                //allincome = normalincome+classpayment
                FillLabels(normalStatistic[0].GroupDetais, isSortedByMonth);
                Summary.CreateSummaries(normalStatistic[0].GroupDetais.Keys.ToArray(), isSortedByMonth);
                ColumnSeries col = CreateOneColumn(
                       StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.AllIncome).First().ShownText,
                       GlobalVariables.IncomeColor);
                FinanceStatisticGroup<AccountInfoModel> allincome = normalStatistic.Where(s => s.StatisticType == StatisticTypeEnum.AllIncome).First();
                DateTime[] statisticKeys = allincome.GroupDetais.Keys.ToArray();
                foreach (DateTime span in statisticKeys)
                {
                    col.Values.Add(allincome.GroupDetais[span] + classpaymentStatistic.GroupDetais[span]);
                    allincome.GroupDetais[span] += classpaymentStatistic.GroupDetais[span];
                }
                ColumnColletcion.Add(col);
                Summary.AddSummary(allincome.GroupDetais, GlobalVariables.IncomeColor, allincome.GroupName, allincome.StatisticType);
            }
            if (statistics.Where(s => s.StatisticType == StatisticTypeEnum.AllOutcome).FirstOrDefault() != null)
            {
                //alloutcome = normaloutcome + teacherfee
                FillLabels(normalStatistic[0].GroupDetais, isSortedByMonth);
                Summary.CreateSummaries(normalStatistic[0].GroupDetais.Keys.ToArray(), isSortedByMonth);
                ColumnSeries col = CreateOneColumn(
                       StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.AllOutcome).First().ShownText,
                       GlobalVariables.ExpenseColor);
                FinanceStatisticGroup<AccountInfoModel> alloutcome = normalStatistic.Where(s => s.StatisticType == StatisticTypeEnum.AllOutcome).First();
                DateTime[] statisticKeys = alloutcome.GroupDetais.Keys.ToArray();
                foreach (DateTime span in statisticKeys)
                {
                    col.Values.Add(alloutcome.GroupDetais[span] + teacherFeeStatistic.GroupDetais[span]);
                    alloutcome.GroupDetais[span] += teacherFeeStatistic.GroupDetais[span];
                }
                ColumnColletcion.Add(col);
                Summary.AddSummary(alloutcome.GroupDetais, GlobalVariables.ExpenseColor, alloutcome.GroupName, alloutcome.StatisticType);
            }
            if (statistics.Where(s => s.StatisticType == StatisticTypeEnum.Diff).FirstOrDefault() != null)
            {
                FillLabels(diffStatisticGroup, isSortedByMonth);
                Summary.CreateSummaries(normalStatistic[0].GroupDetais.Keys.ToArray(), isSortedByMonth);
                ColumnSeries col = CreateOneColumn(
                    StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.Diff).First().ShownText,
                    GlobalVariables.SecondaryColor);
                foreach (decimal amount in diffStatisticGroup.Values)
                {
                    col.Values.Add(amount);
                }
                ColumnColletcion.Add(col);
                Summary.AddSummary(diffStatisticGroup, GlobalVariables.SecondaryColor,
                    StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.Diff).First().ShownText, StatisticTypeEnum.Diff);
            }
            if (statistics.Where(s => s.StatisticType == StatisticTypeEnum.TeacherFee).FirstOrDefault() != null)
            {
                FillLabels(teacherFeeStatistic.GroupDetais, isSortedByMonth);
                Summary.CreateSummaries(teacherFeeStatistic.GroupDetais.Keys.ToArray(), isSortedByMonth);
                ColumnSeries col = CreateOneColumn(teacherFeeStatistic.GroupName, _colorIndex);
                foreach (decimal amount in teacherFeeStatistic.GroupDetais.Values)
                {
                    col.Values.Add(amount);
                }
                ColumnColletcion.Add(col);
                Summary.AddSummary(teacherFeeStatistic.GroupDetais, GlobalVariables.CustomerColorSet[_colorIndex % 12],
                    teacherFeeStatistic.GroupName, teacherFeeStatistic.StatisticType);
                _colorIndex++;
            }
            if (statistics.Where(s => s.StatisticType == StatisticTypeEnum.ClassFee).FirstOrDefault() != null)
            {
                FillLabels(classpaymentStatistic.GroupDetais, isSortedByMonth);
                Summary.CreateSummaries(classpaymentStatistic.GroupDetais.Keys.ToArray(), isSortedByMonth);
                ColumnSeries col = CreateOneColumn(classpaymentStatistic.GroupName, _colorIndex);
                foreach (decimal amount in classpaymentStatistic.GroupDetais.Values)
                {
                    col.Values.Add(amount);
                }
                ColumnColletcion.Add(col);
                Summary.AddSummary(classpaymentStatistic.GroupDetais, GlobalVariables.CustomerColorSet[_colorIndex % 12],
                    classpaymentStatistic.GroupName, classpaymentStatistic.StatisticType);
                _colorIndex++;
            }
            if (statistics.Where(s => s.StatisticType != StatisticTypeEnum.AllIncome &&
            s.StatisticType != StatisticTypeEnum.AllOutcome &&
            s.StatisticType != StatisticTypeEnum.Diff &&
            s.StatisticType != StatisticTypeEnum.ClassFee &&
            s.StatisticType != StatisticTypeEnum.TeacherFee).FirstOrDefault() != null)
            {
                FillLabels(normalStatistic[0].GroupDetais, isSortedByMonth);
                Summary.CreateSummaries(normalStatistic[0].GroupDetais.Keys.ToArray(), isSortedByMonth);
                foreach (FinanceStatisticGroup<AccountInfoModel> item in normalStatistic)
                {
                    ColumnSeries col = CreateOneColumn(item.GroupName, _colorIndex);
                    foreach (decimal amount in item.GroupDetais.Values)
                    {
                        col.Values.Add(amount);
                    }
                    ColumnColletcion.Add(col);
                    Summary.AddSummary(item.GroupDetais, GlobalVariables.CustomerColorSet[_colorIndex % 12], item.GroupName, item.StatisticType);
                    _colorIndex++;
                }
            }
        }

        private void FillLabels(Dictionary<DateTime, decimal> groupDetais, bool isSortedByMonth)
        {
            if (Labels.Count == 0)
            {
                foreach (DateTime span in groupDetais.Keys)
                {
                    Labels.Add(isSortedByMonth ? span.ToString("yyyy-MM") : span.ToString("yyyy"));
                }
            }
        }

        private bool CheckValidity(List<StatisticTypeModel> statistics, DateTime startDate, DateTime endDate)
        {
            if (statistics == null || statistics.Count == 0)
            {
                return EnableErrMsg(MessageType.SearchingTypeErr);
            }
            return CheckValidity(startDate, endDate);
        }
    }
}
