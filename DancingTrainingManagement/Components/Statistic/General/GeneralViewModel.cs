using BLL.StatisticManagement.GeneralAndExport;
using Common;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.General
{
    class GeneralViewModel : ViewModelBase
    {
        private string _traineeCount;

        public string TraineeCount
        {
            get { return _traineeCount; }
            set
            {
                _traineeCount = value;
                RaisePropertyChanged("TraineeCount");
            }
        }

        private string _currentCapital;

        public string CurrentCapital
        {
            get { return _currentCapital; }
            set
            {
                _currentCapital = value;
                RaisePropertyChanged("CurrentCapital");
            }
        }

        private Color _capitalColor;

        public Color CapitalColor
        {
            get { return _capitalColor; }
            set
            {
                _capitalColor = value;
                RaisePropertyChanged("CapitalColor");
            }
        }

        private string _selectedYear;

        public string SelectBillYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                RaisePropertyChanged("SelectBillYear");
            }
        }

        private string _selectedMonth;

        public string SelectBillMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                RaisePropertyChanged("SelectBillMonth");
            }
        }

        private List<string> _years;

        public List<string> YearCollection
        {
            get { return _years; }
            set
            {
                _years = value;
                RaisePropertyChanged("YearCollection");
            }
        }

        private List<string> _months;

        public List<string> MonthCollection
        {
            get { return _months; }
            set
            {
                _months = value;
                RaisePropertyChanged("MonthCollection");
            }
        }

        private DelegateCommand _exportBill;

        public DelegateCommand ExportBill
        {
            get
            {
                _exportBill = _exportBill ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (_bussiness.ExportBills(int.Parse(SelectBillYear), int.Parse(SelectBillMonth)))
                        {
                            Msg.Enable(Common.MessageType.ExportSuccess, Common.MessageLevel.Info);
                        }
                        else
                        {
                            Msg.Enable(Common.MessageType.ExportFailed, Common.MessageLevel.Warning);
                        }
                    }));
                return _exportBill;
            }
            set
            {
                _exportBill = value;
                RaisePropertyChanged("ExportBill");
            }
        }

        private DelegateCommand _exportDB;

        public DelegateCommand ExportDB
        {
            get
            {
                _exportDB = _exportDB ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (_bussiness.ExportDB())
                        {
                            Msg.Enable(Common.MessageType.ExportSuccess, Common.MessageLevel.Info);
                        }
                        else
                        {
                            Msg.Enable(Common.MessageType.ExportFailed, Common.MessageLevel.Warning);
                        }
                    }));
                return _exportDB;
            }
            set
            {
                _exportDB = value;
                RaisePropertyChanged("ExportDB");
            }
        }


        private MessageViewModel _msg;

        public MessageViewModel Msg
        {
            get { return _msg; }
            set
            {
                _msg = value;
                RaisePropertyChanged("Msg");
            }
        }

        private GeneralAndExportBussiness _bussiness;

        public GeneralViewModel(GeneralAndExportBussiness bussiness)
        {
            _bussiness = bussiness;
            _bussiness.CurrentCapitalChangedEvent += capital =>
            {
                CurrentCapital = capital.ToString();
                CapitalColor = capital > 0 ? GlobalVariables.IncomeColor : GlobalVariables.ExpenseColor;
            };
            _bussiness.TraineeCountChangedEvent += count => TraineeCount = count.ToString();
            _bussiness.Init();

            YearCollection = new List<string>();
            for (int i = 2019; i < 2035; i++)
            {
                YearCollection.Add(i.ToString());
            }
            MonthCollection = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                MonthCollection.Add(i.ToString());
            }
            SelectBillYear = DateTime.Now.Year.ToString();
            SelectBillMonth = DateTime.Now.Month.ToString();

            Msg = new MessageViewModel(false);
            Msg.OnOperateEnableEvent(false, false);
        }
    }
}
