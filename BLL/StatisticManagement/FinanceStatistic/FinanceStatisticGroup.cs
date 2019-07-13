using Common;
using Model;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.FinanceStatistic
{
    public class FinanceStatisticGroup<T>
    {
        public StatisticTypeEnum StatisticType { get; private set; }
        public string GroupName { get; private set; }
        public Dictionary<DateTime, decimal> GroupDetais { get; private set; }
        public Dictionary<DateTime, List<T>> FinanceDetails { get; private set; }

        public FinanceStatisticGroup(StatisticTypeEnum statisticType, string groupName, List<AccountInfoModel> details, DateTime startDate, DateTime endDate, bool isSortByMonth)
        {
            Init(statisticType, groupName, startDate, endDate, isSortByMonth);
            DateTime[] spanKeys = GroupDetais.Keys.ToArray();
            foreach (DateTime span in spanKeys)
            {
                GroupDetais[span] = details == null ? 0 :
                    details.Where(d => d.AccountDate >= span && d.AccountDate < (isSortByMonth ? span.AddMonths(1) : span.AddYears(1)))
                    .Sum(d => d.AccountAmount);
                FinanceDetails[span] = details == null ? null : (List<T>)Convert.ChangeType(
                    details.Where(d => d.AccountDate >= span && d.AccountDate < (isSortByMonth ? span.AddMonths(1) : span.AddYears(1))).ToList(), typeof(List<T>));
            }
        }

        public FinanceStatisticGroup(StatisticTypeEnum statisticType, string groupName, List<ClassPaymentModel> details, DateTime startDate, DateTime endDate, bool isSortByMonth)
        {
            Init(statisticType, groupName, startDate, endDate, isSortByMonth);
            DateTime[] spanKeys = GroupDetais.Keys.ToArray();
            foreach (DateTime span in spanKeys)
            {
                GroupDetais[span] = details == null ? 0 : details.Where(d => d.PaymentDate >= span && d.PaymentDate < (isSortByMonth ? span.AddMonths(1) : span.AddYears(1)))
                    .Sum(d => d.TotalCost);
                FinanceDetails[span] = details == null ? null : (List<T>)Convert.ChangeType(
                    details.Where(d => d.PaymentDate >= span && d.PaymentDate < (isSortByMonth ? span.AddMonths(1) : span.AddYears(1))).ToList(), typeof(List<T>));
            }
        }

        public FinanceStatisticGroup(StatisticTypeEnum statisticType, string groupName, List<TeacherFeeModel> details, DateTime startDate, DateTime endDate, bool isSortByMonth)
        {
            Init(statisticType, groupName, startDate, endDate, isSortByMonth);
            DateTime[] spanKeys = GroupDetais.Keys.ToArray();
            foreach (DateTime span in spanKeys)
            {
                GroupDetais[span] = details == null ? 0 : details.Where(d => d.PaymentDate >= span
                && d.PaymentDate< (isSortByMonth ? span.AddMonths(1) : span.AddYears(1)))
                    .Sum(d => d.Amount);
                FinanceDetails[span] = details == null ? null : (List<T>)Convert.ChangeType(
                    details.Where(d => d.PaymentDate >= span
                    && d.PaymentDate < (isSortByMonth ? span.AddMonths(1) : span.AddYears(1))).ToList(), typeof(List<T>));
            }
        }

        public FinanceStatisticGroup(StatisticTypeEnum statisticType, string groupName, List<CapitalModel> details, DateTime startDate, DateTime endDate, bool isSortByMonth)
        {
            Init(statisticType, groupName, startDate, endDate, isSortByMonth);
            if (details != null)
                details.Sort();
            DateTime[] spanKeys = GroupDetais.Keys.ToArray();
            foreach (DateTime span in spanKeys)
            {
                if (details == null
                    || details.Count == 0
                    || span < DateTime.Parse(string.Format("{0}-{1}-1", details[0].GeneralYear, details[0].GeneralMonth))
                    || span > DateTime.Parse(string.Format("{0}-{1}-1", details[details.Count - 1].GeneralYear, details[details.Count - 1].GeneralMonth)))
                {
                    GroupDetais[span] = 0;
                }
                else
                {
                    if (isSortByMonth)
                    {
                        GroupDetais[span] = details.Where(d => DateTime.Parse(string.Format("{0}-{1}-1", d.GeneralYear, d.GeneralMonth)) <= span).Sum(d => d.Capital);
                    }
                    else
                    {
                        GroupDetais[span] = details.Where(d => DateTime.Parse(string.Format("{0}-{1}-1", d.GeneralYear, d.GeneralMonth)) < span.AddYears(1).AddDays(-1)).Sum(d => d.Capital);
                    }
                }
            }
        }

        private void Init(StatisticTypeEnum statisticType, string groupName, DateTime startDate, DateTime endDate, bool isSortByMonth)
        {
            GroupName = groupName;
            StatisticType = statisticType;
            GroupDetais = new Dictionary<DateTime, decimal>();
            FinanceDetails = new Dictionary<DateTime, List<T>>();

            GenerateDateInterval(startDate, endDate, isSortByMonth);
        }

        private void GenerateDateInterval(DateTime startDate, DateTime endDate, bool isSortByMonth)
        {
            int datespanCount = 0;
            if (isSortByMonth)
            {
                datespanCount = (endDate.Month - startDate.Month + 1) + 12 * (endDate.Year - startDate.Year);
            }
            else
            {
                datespanCount = endDate.Year - startDate.Year + 1;
            }
            for (int i = 0; i < datespanCount; i++)
            {
                GroupDetais.Add(isSortByMonth ? startDate.AddMonths(i) : startDate.AddYears(i), 0);
                FinanceDetails.Add(isSortByMonth ? startDate.AddMonths(i) : startDate.AddYears(i), null);
            }
        }
    }

}
