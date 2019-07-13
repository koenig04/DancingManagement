using BLL.ClassManagement;
using BLL.TeacherManagement;
using BLL.TraineeManagement;
using Common;
using DAL;
using Model;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.FinanceStatistic
{
    public class FinanceStatisticBussiness
    {
        public delegate void FinanceStatisticChanged(List<StatisticTypeModel> statistics, List<FinanceStatisticGroup<AccountInfoModel>> normalStatistic,
            FinanceStatisticGroup<TeacherFeeModel> teacherFeeStatistic, FinanceStatisticGroup<ClassPaymentModel> classpaymentStatistic,
            Dictionary<DateTime, decimal> diffStatisticGroup, bool isSortedByMonth);
        public event FinanceStatisticChanged FinanceStatisticChangedEvent;
        public delegate void ShowDetails(List<AccountInfoModel> normalStatistic,
            List<TeacherFeeModel> teacherFeeStatistic, List<ClassPaymentModel> classpaymentStatistic);
        public event ShowDetails ShowDetailsEvent;
        public delegate void CapitalChanged(FinanceStatisticGroup<CapitalModel> capitals, bool isSortedByMonth);
        public event CapitalChanged CapitalChangedEvent;

        public event EventHandler GeneralCapitalChanged;

        private PaymentInfo _paymentDal;
        private TraineeManagementBussiness _trainees;
        private BlockClassManagement _blockClasses;
        private RegularClassManagement _regularClasses;
        private GeneralInfo _generalDal;

        private List<FinanceStatisticGroup<AccountInfoModel>> _normalStatisticGroup;
        private FinanceStatisticGroup<TeacherFeeModel> _teacherFeeStatistcGroup;
        private FinanceStatisticGroup<ClassPaymentModel> _classPaymentStatisticGroup;
        private FinanceStatisticGroup<CapitalModel> _capitalStatisticGroup;
        private Dictionary<DateTime, decimal> _diffStatisticGroup;

        private List<StatisticTypeModel> _storageStatistics;
        private DateTime _storageStartDate;
        private DateTime _storageEndDate;
        private bool _storageIsSortedByMonth;
        public FinanceStatisticBussiness(PaymentInfo paymentDal, TraineeManagementBussiness trainees, BlockClassManagement blocks, RegularClassManagement regulars,
            GeneralInfo generalDal)
        {
            _paymentDal = paymentDal;
            _trainees = trainees;
            _blockClasses = blocks;
            _regularClasses = regulars;
            _generalDal = generalDal;
            _normalStatisticGroup = new List<FinanceStatisticGroup<AccountInfoModel>>();
        }

        public void Refresh()
        {
            Search(_storageStatistics, _storageStartDate, _storageEndDate, _storageIsSortedByMonth);
        }

        public void Search(List<StatisticTypeModel> statistics, DateTime startDate, DateTime endDate, bool isSortedByMonth)
        {
            _storageStatistics = statistics;
            _storageStartDate = startDate;
            _storageEndDate = endDate;
            _storageIsSortedByMonth = isSortedByMonth;

            _normalStatisticGroup.Clear();
            _teacherFeeStatistcGroup = null;
            _classPaymentStatisticGroup = null;
            _diffStatisticGroup = null;
            foreach (StatisticTypeModel statistic in statistics)
            {
                if (statistic.StatisticType == StatisticTypeEnum.Capital)
                {
                    _capitalStatisticGroup = new FinanceStatisticGroup<CapitalModel>(StatisticTypeEnum.Capital,
                         StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.Capital).First().ShownText,
                         _generalDal.GetCapitals(startDate, endDate),
                         startDate,
                         endDate,
                         isSortedByMonth);
                    CapitalChangedEvent?.Invoke(_capitalStatisticGroup, isSortedByMonth);
                    return;
                }
                else if (statistic.StatisticType == StatisticTypeEnum.ClassFee)
                {
                    _classPaymentStatisticGroup = new FinanceStatisticGroup<ClassPaymentModel>(StatisticTypeEnum.ClassFee,
                        StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.ClassFee).First().ShownText,
                        _paymentDal.GetClassPaymentList(startDate, endDate),
                        startDate,
                        endDate,
                        isSortedByMonth);
                }
                else if (statistic.StatisticType == StatisticTypeEnum.TeacherFee)
                {
                    _teacherFeeStatistcGroup = new FinanceStatisticGroup<TeacherFeeModel>(StatisticTypeEnum.TeacherFee,
                       StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.TeacherFee).First().ShownText,
                       _paymentDal.GetTeacherFeeList(startDate, endDate),
                       startDate,
                       endDate,
                       isSortedByMonth);
                }
                else if (statistic.StatisticType == StatisticTypeEnum.AllIncome)
                {
                    _normalStatisticGroup.Add(new FinanceStatisticGroup<AccountInfoModel>(statistic.StatisticType,
                       StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == statistic.StatisticType).First().ShownText,
                       _paymentDal.GetAccountListByAll(true, startDate, endDate),
                       startDate,
                       endDate,
                       isSortedByMonth));
                    if (_classPaymentStatisticGroup == null)
                    {
                        _classPaymentStatisticGroup = new FinanceStatisticGroup<ClassPaymentModel>(StatisticTypeEnum.ClassFee,
                       StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.ClassFee).First().ShownText,
                       _paymentDal.GetClassPaymentList(startDate, endDate),
                       startDate,
                       endDate,
                       isSortedByMonth);
                    }
                }
                else if (statistic.StatisticType == StatisticTypeEnum.AllOutcome)
                {
                    _normalStatisticGroup.Add(new FinanceStatisticGroup<AccountInfoModel>(statistic.StatisticType,
                      StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == statistic.StatisticType).First().ShownText,
                      _paymentDal.GetAccountListByAll(false, startDate, endDate),
                      startDate,
                      endDate,
                      isSortedByMonth));
                    if (_teacherFeeStatistcGroup == null)
                    {
                        _teacherFeeStatistcGroup = new FinanceStatisticGroup<TeacherFeeModel>(StatisticTypeEnum.TeacherFee,
                      StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.TeacherFee).First().ShownText,
                      _paymentDal.GetTeacherFeeList(startDate, endDate),
                      startDate,
                      endDate,
                      isSortedByMonth);
                    }
                }
                else if (statistic.StatisticType != StatisticTypeEnum.Diff)
                {
                    _normalStatisticGroup.Add(new FinanceStatisticGroup<AccountInfoModel>(statistic.StatisticType,
                      StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == statistic.StatisticType).First().ShownText,
                      _paymentDal.GetAccountListByItem(statistic.ID, startDate, endDate),
                      startDate,
                      endDate,
                      isSortedByMonth));
                }
            }

            if (statistics.Where(s => s.StatisticType == StatisticTypeEnum.Diff).FirstOrDefault() != null)
            {
                _diffStatisticGroup = new Dictionary<DateTime, decimal>();
                if (_classPaymentStatisticGroup == null)
                {
                    _classPaymentStatisticGroup = new FinanceStatisticGroup<ClassPaymentModel>(StatisticTypeEnum.ClassFee,
                   StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.ClassFee).First().ShownText,
                   _paymentDal.GetClassPaymentList(startDate, endDate),
                   startDate,
                   endDate,
                   isSortedByMonth);
                }
                if (_teacherFeeStatistcGroup == null)
                {
                    _teacherFeeStatistcGroup = new FinanceStatisticGroup<TeacherFeeModel>(StatisticTypeEnum.TeacherFee,
                  StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.TeacherFee).First().ShownText,
                  _paymentDal.GetTeacherFeeList(startDate, endDate),
                  startDate,
                  endDate,
                  isSortedByMonth);
                }
                if (_normalStatisticGroup.Where(s => s.StatisticType == StatisticTypeEnum.AllIncome).FirstOrDefault() == null)
                {
                    _normalStatisticGroup.Add(new FinanceStatisticGroup<AccountInfoModel>(StatisticTypeEnum.AllIncome,
                      StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.AllIncome).First().ShownText,
                      _paymentDal.GetAccountListByAll(true, startDate, endDate),
                      startDate,
                      endDate,
                      isSortedByMonth));
                }
                if (_normalStatisticGroup.Where(s => s.StatisticType == StatisticTypeEnum.AllOutcome).FirstOrDefault() == null)
                {
                    _normalStatisticGroup.Add(new FinanceStatisticGroup<AccountInfoModel>(StatisticTypeEnum.AllOutcome,
                     StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.AllOutcome).First().ShownText,
                     _paymentDal.GetAccountListByAll(false, startDate, endDate),
                     startDate,
                     endDate,
                     isSortedByMonth));
                }
                Dictionary<DateTime, decimal> IncomeGroup = _normalStatisticGroup.Where(s => s.StatisticType == StatisticTypeEnum.AllIncome).First().GroupDetais;
                Dictionary<DateTime, decimal> OutcomeGroup = _normalStatisticGroup.Where(s => s.StatisticType == StatisticTypeEnum.AllOutcome).First().GroupDetais;
                foreach (DateTime span in IncomeGroup.Keys)
                {
                    _diffStatisticGroup.Add(span, IncomeGroup[span] + _classPaymentStatisticGroup.GroupDetais[span]
                        - OutcomeGroup[span] - _teacherFeeStatistcGroup.GroupDetais[span]);
                }
            }
            FinanceStatisticChangedEvent?.Invoke(statistics, _normalStatisticGroup, _teacherFeeStatistcGroup, _classPaymentStatisticGroup, _diffStatisticGroup, isSortedByMonth);
        }

        public string GetItemName(TeacherFeeModel teacherFee)
        {
            return StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.TeacherFee).First().ShownText
                + "-" + TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherID == teacherFee.TeacherID).First().TeacherName
                + "-" + teacherFee.FeeYear + "年" + teacherFee.FeeMonth + "月";
        }

        public string GetItemName(ClassPaymentModel classPayment)
        {
            return StatisticTypeManagement.Instance.StatisticTypeCollection.Where(s => s.StatisticType == StatisticTypeEnum.ClassFee).First().ShownText
                + "-" + _trainees.SearchTraineeName(classPayment.TraineeID)
                + "-" + (classPayment.ClassType == 0 ? _regularClasses.RegularClassCollection.Where(c => c.ClassID == classPayment.ClassID).First().ClassName
                : _blockClasses.BlockClassCollection.Where(c => c.ClassID == classPayment.ClassID).First().ClassName);
        }

        public void GetDetails(StatisticTypeEnum statisticType, DateTime summaryDate, bool isTotal)
        {
            List<AccountInfoModel> normalStatistic;
            List<TeacherFeeModel> teacherFeeStatistic;
            List<ClassPaymentModel> classpaymentStatistic;
            normalStatistic = new List<AccountInfoModel>();
            teacherFeeStatistic = new List<TeacherFeeModel>();
            classpaymentStatistic = new List<ClassPaymentModel>();
            if (isTotal)
            {
                if (statisticType == StatisticTypeEnum.AllIncome)
                {
                    foreach (List<AccountInfoModel> item in _normalStatisticGroup.Where(g => g.StatisticType == StatisticTypeEnum.AllIncome).First().FinanceDetails.Values)
                    {
                        if (item != null)
                            normalStatistic.AddRange(item);
                    }
                    if (_classPaymentStatisticGroup != null)
                    {
                        foreach (List<ClassPaymentModel> item in _classPaymentStatisticGroup.FinanceDetails.Values)
                        {
                            classpaymentStatistic.AddRange(item);
                        }
                    }
                }
                else if (statisticType == StatisticTypeEnum.AllOutcome)
                {
                    foreach (List<AccountInfoModel> item in _normalStatisticGroup.Where(g => g.StatisticType == StatisticTypeEnum.AllOutcome).First().FinanceDetails.Values)
                    {
                        if (item != null)
                            normalStatistic.AddRange(item);
                    }
                    if (_teacherFeeStatistcGroup != null)
                    {
                        foreach (List<TeacherFeeModel> item in _teacherFeeStatistcGroup.FinanceDetails.Values)
                        {
                            teacherFeeStatistic.AddRange(item);
                        }
                    }
                }
                else if (statisticType == StatisticTypeEnum.ClassFee)
                {
                    if (_classPaymentStatisticGroup != null)
                    {
                        foreach (List<ClassPaymentModel> item in _classPaymentStatisticGroup.FinanceDetails.Values)
                        {
                            classpaymentStatistic.AddRange(item);
                        }
                    }
                }
                else if (statisticType == StatisticTypeEnum.TeacherFee)
                {
                    if (_teacherFeeStatistcGroup != null)
                    {
                        foreach (List<TeacherFeeModel> item in _teacherFeeStatistcGroup.FinanceDetails.Values)
                        {
                            teacherFeeStatistic.AddRange(item);
                        }
                    }
                }
                else
                {
                    foreach (List<AccountInfoModel> item in _normalStatisticGroup.Where(g => g.StatisticType == statisticType).First().FinanceDetails.Values)
                    {
                        if (item != null)
                            normalStatistic.AddRange(item);
                    }
                }
            }
            else
            {
                if (statisticType == StatisticTypeEnum.AllIncome)
                {
                    normalStatistic = _normalStatisticGroup.Where(s => s.StatisticType == StatisticTypeEnum.AllIncome).First().FinanceDetails[summaryDate];
                    if (_classPaymentStatisticGroup != null)
                        classpaymentStatistic = _classPaymentStatisticGroup.FinanceDetails[summaryDate];
                }
                else if (statisticType == StatisticTypeEnum.AllOutcome)
                {
                    normalStatistic = _normalStatisticGroup.Where(s => s.StatisticType == StatisticTypeEnum.AllOutcome).First().FinanceDetails[summaryDate];
                    if (_teacherFeeStatistcGroup != null)
                        teacherFeeStatistic = _teacherFeeStatistcGroup.FinanceDetails[summaryDate];
                }
                else if (statisticType == StatisticTypeEnum.ClassFee)
                {
                    if (_classPaymentStatisticGroup != null)
                        classpaymentStatistic = _classPaymentStatisticGroup.FinanceDetails[summaryDate];
                }
                else if (statisticType == StatisticTypeEnum.TeacherFee)
                {
                    if (_teacherFeeStatistcGroup != null)
                        teacherFeeStatistic = _teacherFeeStatistcGroup.FinanceDetails[summaryDate];
                }
                else
                {
                    normalStatistic = _normalStatisticGroup.Where(s => s.StatisticType == statisticType).First().FinanceDetails[summaryDate];
                }
            }

            ShowDetailsEvent?.Invoke(normalStatistic, teacherFeeStatistic, classpaymentStatistic);
        }

        public void DeletePayment(StatisticTypeEnum deleteType, string deleteID)
        {
            switch (deleteType)
            {
                case StatisticTypeEnum.ClassFee:
                    _paymentDal.DelClassPayment(deleteID);
                    break;
                case StatisticTypeEnum.TeacherFee:
                    _paymentDal.DelTeacherFee(deleteID);
                    break;
                default:
                    _paymentDal.DelAccountInfo(deleteID);
                    break;
            }
            GeneralCapitalChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
