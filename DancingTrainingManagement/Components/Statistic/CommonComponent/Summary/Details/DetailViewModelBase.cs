using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Details
{
    class DetailViewModelBase : ViewModelBase
    {
        private int _totalWidth;

        public int TotalWidth
        {
            get { return _totalWidth; }
            set
            {
                _totalWidth = value;
                ScrollWidth = value - 10;
                RaisePropertyChanged("TotalWidth");
            }
        }

        private int _scrollWidth;

        public int ScrollWidth
        {
            get { return _scrollWidth; }
            set
            {
                _scrollWidth = value;
                RaisePropertyChanged("ScrollWidth");
            }
        }

    }
}
