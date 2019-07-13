using BLL.ItemManagement;
using BLL.StatisticManagement.FinanceStatistic;
using BLL.TeacherManagement;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Details
{
    class FinanceRecordViewModel : ViewModelBase, IComparable<FinanceRecordViewModel>
    {
        private Color _imgBkg;

        public Color ImgBkg
        {
            get { return _imgBkg; }
            set
            {
                _imgBkg = value;
                RaisePropertyChanged("ImgBkg");
            }
        }

        private BitmapImage _img;

        public BitmapImage Img
        {
            get { return _img; }
            set
            {
                _img = value;
                RaisePropertyChanged("Img");
            }
        }

        private string _recordItem;

        public string RecordItem
        {
            get { return _recordItem; }
            set
            {
                _recordItem = value;
                RaisePropertyChanged("RecordItem");
            }
        }

        private string _recordDate;

        public string RecordDate
        {
            get { return _recordDate; }
            set
            {
                _recordDate = value;
                RaisePropertyChanged("RecordDate");
            }
        }

        private string _recordAmount;

        public string RecordAmount
        {
            get { return _recordAmount; }
            set
            {
                _recordAmount = value;
                RaisePropertyChanged("RecordAmount");
            }
        }

        private Color _amountColor;

        public Color AmountColor
        {
            get { return _amountColor; }
            set
            {
                _amountColor = value;
                RaisePropertyChanged("AmountColor");
            }
        }

        private Color _fontColor;

        public Color FontColor
        {
            get { return _fontColor; }
            set
            {
                _fontColor = value;
                RaisePropertyChanged("FontColor");
            }
        }


        private DelegateCommand _recordClicked;

        public DelegateCommand RecordClicked
        {
            get
            {
                _recordClicked = _recordClicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ChangeSelectState(true);
                        if (!string.IsNullOrEmpty(Note))
                        {
                            IsNoteShown = true;
                        }
                        FinanceRecordSelectedEvent?.Invoke(AccountID);
                    }));
                return _recordClicked;
            }
            set
            {
                _recordClicked = value;
                RaisePropertyChanged("RecordClicked");
            }
        }

        private Visibility _visOp;

        public Visibility VisOperation
        {
            get { return _visOp; }
            set
            {
                _visOp = value;
                RaisePropertyChanged("VisOperation");
            }
        }

        private string _note;

        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                RaisePropertyChanged("Note");
            }
        }

        private bool _isNoteShown;

        public bool IsNoteShown
        {
            get { return _isNoteShown; }
            set
            {
                _isNoteShown = value;
                RaisePropertyChanged("IsNoteShown");
            }
        }

        private DelegateCommand _delete;

        public DelegateCommand DeleteRecord
        {
            get
            {
                _delete = _delete ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        DeleteRecordEvent?.Invoke(_statisticType, AccountID, "日期：" + RecordDate, "内容：" + RecordItem, "金额：" + RecordAmount, "备注：" + Note);
                    }));
                return _delete;
            }
            set
            {
                _delete = value;
                RaisePropertyChanged("DeleteRecord");
            }
        }


        public DateTime AccountDate
        {
            get
            {
                return DateTime.Parse(RecordDate);
            }
        }

        public string AccountID { get; private set; }

        public delegate void FinanceRecordSelected(string accountID);
        public event FinanceRecordSelected FinanceRecordSelectedEvent;
        public delegate void DeleteAccountRecord(StatisticTypeEnum statisticType, string accountID, string accountDate, string accountItem, string accountAmount, string note);
        public event DeleteAccountRecord DeleteRecordEvent;

        private StatisticTypeEnum _statisticType;
        private AccountInfoModel _normalAccount;
        private TeacherFeeModel _teacherFee;
        private ClassPaymentModel _classPayment;
        private Color _recordColor;
        public FinanceRecordViewModel(AccountInfoModel normalAccount, Color recordColor)
        {
            _statisticType = StatisticTypeEnum.NormalAccount;
            AccountID = normalAccount.AccountID;
            _normalAccount = normalAccount;
            _recordColor = recordColor;
            RecordItem = ItemManagementBussiness.Instance.Items.Where(i => i.ItemID == _normalAccount.ItemID).First().ItemName;
            RecordDate = _normalAccount.AccountDate.ToString("yyyy年MM月dd日");
            Note = normalAccount.Notice;
            RecordAmount = normalAccount.AccountAmount.ToString();
            ChangeSelectState(false);
        }

        public FinanceRecordViewModel(TeacherFeeModel teacherFee, Color recordColor, FinanceStatisticBussiness bussiness)
        {
            _statisticType = StatisticTypeEnum.TeacherFee;
            AccountID = teacherFee.TeacherFeeID;
            _teacherFee = teacherFee;
            _recordColor = recordColor;
            RecordItem = bussiness.GetItemName(teacherFee);
            RecordDate = _teacherFee.PaymentDate.ToString("yyyy年MM月dd日");
            RecordAmount = teacherFee.Amount.ToString();
            ChangeSelectState(false);
        }

        public FinanceRecordViewModel(ClassPaymentModel classPayment, Color recordColor, FinanceStatisticBussiness bussiness)
        {
            _statisticType = StatisticTypeEnum.ClassFee;
            AccountID = classPayment.PaymentID;
            _classPayment = classPayment;
            _recordColor = recordColor;
            RecordItem = bussiness.GetItemName(classPayment);
            RecordDate = _classPayment.PaymentDate.ToString("yyyy年MM月dd日");
            RecordAmount = classPayment.TotalCost.ToString();
            ChangeSelectState(false);
        }

        public void ChangeSelectState(bool isSelected)
        {
            ImgBkg = isSelected ? _recordColor : GlobalVariables.MainBackColor;
            FontColor = isSelected ? GlobalVariables.MainBackColor : Colors.Black;
            AmountColor = isSelected ? GlobalVariables.MainBackColor : _recordColor;
            string iconPath;
            switch (_statisticType)
            {
                case StatisticTypeEnum.ClassFee:
                    iconPath = isSelected ? GlobalVariables.ClassPaymentIconPressed : GlobalVariables.ClassPaymentIcon;
                    break;
                case StatisticTypeEnum.TeacherFee:
                    iconPath = isSelected ? GlobalVariables.TeacherFeeIconPressed : GlobalVariables.TeacherFeeIcon;
                    break;
                default:
                    iconPath = isSelected ?
                        ItemManagementBussiness.Instance.Items.Where(i => i.ItemID == _normalAccount.ItemID).First().IconNamePressed :
                        ItemManagementBussiness.Instance.Items.Where(i => i.ItemID == _normalAccount.ItemID).First().IconName;
                    break;
            }
            Img = PublicMathods.GetImage(iconPath);
            VisOperation = isSelected ? Visibility.Visible : Visibility.Hidden;
        }

        public int CompareTo(FinanceRecordViewModel other)
        {
            if (AccountDate > other.AccountDate)
            {
                return 1;
            }
            else if (AccountDate < other.AccountDate)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
