using BLL.PaymentManagement.ClassPayment.PaymentSelecter;
using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.Payment.ClassPayment.ClassFee.PaymentSelecter
{
    class PaymentInCountViewModel : ViewModelBase
    {
        private int _termCount;

        public int TermCount
        {
            get { return _termCount; }
            set
            {
                _termCount = value;
                if (value > 0)
                    TotalCost = (decimal.Parse(CostPerTerm) * value).ToString();
                else
                    TotalCost = string.Empty;
                RaisePropertyChanged("TermCount");
            }
        }

        private DelegateCommand _changeCount;

        public DelegateCommand ChangeTermCount
        {
            get
            {
                _changeCount = _changeCount ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        TermCount = int.Parse(o.ToString());
                    }));
                return _changeCount;
            }
            set
            {
                _changeCount = value;
                RaisePropertyChanged("ChangeTermCount");
            }
        }

        private string _costPerTerm;

        public string CostPerTerm
        {
            get { return _costPerTerm; }
            set
            {
                _costPerTerm = value;
                RaisePropertyChanged("CostPerTerm");
            }
        }

        private string _total;

        public string TotalCost
        {
            get { return _total; }
            set
            {
                _total = value;
                RaisePropertyChanged("TotalCost");
            }
        }

        private Visibility _cisRegular;

        public Visibility VisRegular
        {
            get { return _cisRegular; }
            set
            {
                _cisRegular = value;
                RaisePropertyChanged("VisRegular");
            }
        }

        private PaymentInCountBussiness _bussiness;
        private ClassType _currentClassType;
        public PaymentInCountViewModel(PaymentInCountBussiness bussiness)
        {
            _bussiness = bussiness;
            TermCount = -1;

            _bussiness.ClassTypeChangedEvent += classType =>
            {
                VisRegular = (ClassType)classType == ClassType.Regular ? Visibility.Visible : Visibility.Hidden;
                _currentClassType = (ClassType)classType;
            };

            _bussiness.SelectedClassChangedEvent += model =>
            {
                if (_currentClassType == ClassType.Regular)
                {
                    TermCount = -1;
                    CostPerTerm = model.CostPerTerm.ToString();
                }
                else
                {
                    TotalCost= model.CostPerTerm.ToString();
                }
            };
        }
    }
}
