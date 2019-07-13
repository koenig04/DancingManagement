using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Item
{
    class SummaryItemViewModel : ViewModelBase
    {
        private string _itemTitle;

        public string ItemTitle
        {
            get { return _itemTitle; }
            set
            {
                _itemTitle = value;
                RaisePropertyChanged("ItemTitle");
            }
        }

        private string _amount;

        public string ItemAmount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaisePropertyChanged("ItemAmount");
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

        private Color _bkgColor;

        public Color BkgColor
        {
            get { return _bkgColor; }
            set
            {
                _bkgColor = value;
                RaisePropertyChanged("BkgColor");
            }
        }

        private Color _titleColor;

        public Color TitleColor
        {
            get { return _titleColor; }
            set
            {
                _titleColor = value;
                RaisePropertyChanged("TitleColor");
            }
        }

        public Color SummaryItemColor
        {
            get
            {
                return _color;
            }
        }
        private Color _color;
        public SummaryItemViewModel(string itemName, Color color)
        {
            ItemTitle = itemName;
            ItemColor = color;
            BkgColor = GlobalVariables.MainBackColor;
            TitleColor = Colors.Black;
            _color = color;
        }

        public void ChangeSelectState(bool state)
        {
            ItemColor = state ? Colors.White : _color;
            BkgColor = state ? _color : GlobalVariables.MainBackColor;
            TitleColor = state ? Colors.White : Colors.Black;
        }
    }
}
