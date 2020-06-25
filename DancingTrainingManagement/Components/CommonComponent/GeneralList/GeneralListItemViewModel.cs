using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.CommonComponent.GeneralList
{
    class GeneralListItemViewModel : ViewModelBase
    {
        private Color _itemBkg;

        public Color ItemBkg
        {
            get { return _itemBkg; }
            set
            {
                _itemBkg = value;
                RaisePropertyChanged("ItemBkg");
            }
        }

        private Color _itemFore;

        public Color ItemFore
        {
            get { return _itemFore; }
            set
            {
                _itemFore = value;
                RaisePropertyChanged("ItemFore");
            }
        }

        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                RaisePropertyChanged("ItemName");
            }
        }

        private DelegateCommand _itemClicked;

        public DelegateCommand ItemClicked
        {
            get
            {
                _itemClicked = _itemClicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ItemBkg = GlobalVariables.MainColor;
                        ItemFore = GlobalVariables.MainBackColor;
                        VisOperation = Visibility.Visible;
                        ItemOperatedEvent?.Invoke(OperationType.Select, _model.ItemID);
                    }));
                return _itemClicked;
            }
            set
            {
                _itemClicked = value;
                RaisePropertyChanged("ItemClicked");
            }
        }

        private DelegateCommand _itemModified;

        public DelegateCommand ItemModified
        {
            get
            {
                _itemModified = _itemModified ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ItemOperatedEvent?.Invoke(OperationType.Update, _model.ItemID);
                    }));
                return _itemModified;
            }
            set
            {
                _itemModified = value;
                RaisePropertyChanged("ItemModified");
            }
        }

        private Visibility _visoperation;

        public Visibility VisOperation
        {
            get { return _visoperation; }
            set
            {
                _visoperation = value;
                RaisePropertyChanged("VisOperation");
            }
        }


        public string ItemID
        {
            get
            {
                return _model.ItemID;
            }
        }

        public delegate void ItemOperated(OperationType operation, string itemID);
        public event ItemOperated ItemOperatedEvent;

        private GeneralListItemModel _model;

        public GeneralListItemViewModel(GeneralListItemModel model)
        {
            _model = model;
            ItemName = model.ItemName;
            ItemFore = GlobalVariables.MainColor;
            ItemBkg = GlobalVariables.MainBackColor;
            VisOperation = Visibility.Hidden;
        }

        public void ChangeToUnselected()
        {
            ItemFore = GlobalVariables.MainColor;
            ItemBkg = GlobalVariables.MainBackColor;
            VisOperation = Visibility.Hidden;
        }
    }
}
