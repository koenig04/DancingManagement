using BLL.StatisticManagement.FinanceStatistic;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Details
{
    class FinanceRecordDetailsViewModel : DetailViewModelBase
    {
        private ObservableCollection<FinanceRecordViewModel> _collection;

        public ObservableCollection<FinanceRecordViewModel> AccountRecordsCollection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                RaisePropertyChanged("AccountRecordsCollection");
            }
        }
        public delegate void DeleteAccountRecord(StatisticTypeEnum statisticType, string accountID, string accountDate, string accountItem, string accountAmount, string note);
        public event DeleteAccountRecord DeleteRecordEvent;

        private FinanceStatisticBussiness _bussiness;
        public FinanceRecordDetailsViewModel(FinanceStatisticBussiness bussiness)
        {
            AccountRecordsCollection = new ObservableCollection<FinanceRecordViewModel>();
            _bussiness = bussiness;
            TotalWidth = 400;
        }

        public void Enable(List<AccountInfoModel> normalStatistic,
            List<TeacherFeeModel> teacherFeeStatistic, List<ClassPaymentModel> classpaymentStatistic, Color statisticColor)
        {
            AccountRecordsCollection.Clear();
            if (normalStatistic != null)
            {
                foreach (AccountInfoModel item in normalStatistic)
                {
                    AccountRecordsCollection.Add(new FinanceRecordViewModel(item, statisticColor));
                    AccountRecordsCollection.Last().FinanceRecordSelectedEvent += OnFinanceRecordSelected;
                    AccountRecordsCollection.Last().DeleteRecordEvent += OnDeleteRecord;
                }
            }

            if (teacherFeeStatistic != null)
            {
                foreach (TeacherFeeModel item in teacherFeeStatistic)
                {
                    AccountRecordsCollection.Add(new FinanceRecordViewModel(item, statisticColor, _bussiness));
                    AccountRecordsCollection.Last().FinanceRecordSelectedEvent += OnFinanceRecordSelected;
                    AccountRecordsCollection.Last().DeleteRecordEvent += OnDeleteRecord;
                }
            }

            if (classpaymentStatistic != null)
            {
                foreach (ClassPaymentModel item in classpaymentStatistic)
                {
                    AccountRecordsCollection.Add(new FinanceRecordViewModel(item, statisticColor, _bussiness));
                    AccountRecordsCollection.Last().FinanceRecordSelectedEvent += OnFinanceRecordSelected;
                    AccountRecordsCollection.Last().DeleteRecordEvent += OnDeleteRecord;
                }
            }

            PublicMathods.Sort(AccountRecordsCollection);
        }

        private void OnDeleteRecord(StatisticTypeEnum statisticType, string accountID, string accountDate, string accountItem, string accountAmount, string note)
        {
            DeleteRecordEvent?.Invoke(statisticType, accountID, accountDate, accountItem, accountAmount, note);
        }

        private void OnFinanceRecordSelected(string accountID)
        {
            foreach (FinanceRecordViewModel item in AccountRecordsCollection.Where(a => a.AccountID != accountID))
            {
                item.ChangeSelectState(false);
            }
        }
    }
}
