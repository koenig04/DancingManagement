using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.StatisticHead.FilterItems
{
    abstract class FilterItemViewModel : ViewModelBase
    {
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

        private Color _borderColor;

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                RaisePropertyChanged("BorderColor");
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
       
        protected bool IsSelected;
        protected void Init()
        {
            ChangeSelectionState(false);
            IsSelected = false;
        }
        public abstract void ChangeSelectionState(bool isSelected);        
    }
}
