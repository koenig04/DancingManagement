using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DancingTrainingManagement.Components.Payment.NormalPayment
{
    class AccountItemViewModel:ViewModelBase
    {
        private BitmapImage _ItemImg;

        public BitmapImage ItemImg
        {
            get
            {
                return _ItemImg;
            }
            set
            {
                _ItemImg = value;
                RaisePropertyChanged("ItemImg");
            }
        }

        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                _ItemName = value;
                RaisePropertyChanged("ItemName");
            }
        }

        private DelegateCommand _itemClicked;

        public DelegateCommand ItemClicked
        {
            get
            {
                if (_itemClicked == null)
                {
                    _itemClicked = new DelegateCommand(new Action<object>(
                        o =>
                        {
                            ItemSelectedEvent?.Invoke(_item);
                            ChangeSelecState(true);
                        }));
                }
                return _itemClicked;
            }
            set
            {
                _itemClicked = value;
                RaisePropertyChanged("ItemClicked");
            }
        }

        private Color _itemColor;

        public Color ItemColor
        {
            get { return _itemColor; }
            set
            {
                _itemColor = value;
                RaisePropertyChanged("ItemColor");
            }
        }

        private Color _itemForeColor;

        public Color ItemForeColor
        {
            get { return _itemForeColor; }
            set
            {
                _itemForeColor = value;
                RaisePropertyChanged("ItemForeColor");
            }
        }

        public string ItemID
        {
            get
            {
                return _item.ItemID;
            }
        }

        public delegate void ItemSelected(ItemInfoModel item);
        public event ItemSelected ItemSelectedEvent;
        private ItemInfoModel _item;

        public AccountItemViewModel(ItemInfoModel item)
        {
            _item = item;
            ItemImg = PublicMathods.GetImage(item.IconName);
            ItemName = item.ItemName;
            ChangeSelecState(false);            
        }

        public void ChangeSelecState(bool isSelected)
        {
            ItemColor = isSelected ? (_item.IsIncome ? GlobalVariables.IncomeColor : GlobalVariables.ExpenseColor) : GlobalVariables.MainBackColor;
            ItemForeColor = isSelected ? GlobalVariables.MainBackColor : (_item.IsIncome ? GlobalVariables.IncomeColor : GlobalVariables.ExpenseColor);
            ItemImg = PublicMathods.GetImage(isSelected ? _item.IconNamePressed : _item.IconName);
        }
    }
}
