using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.CommonComponent.Calender
{
    class CalenderViewModel : ViewModelBase
    {
        private ObservableCollection<CalenderItem> _dayCollection;

        public ObservableCollection<CalenderItem> DayCollection
        {
            get { return _dayCollection; }
            set
            {
                _dayCollection = value;
                RaisePropertyChanged("DayCollection");
            }
        }

        public CalenderViewModel()
        {
            Init();
        }

        private void Init()
        {
            DayCollection = new ObservableCollection<CalenderItem>();
            for (int i = 1; i < 49; i++)
            {
                DayCollection.Add(new CalenderItem(i));
            }
        }
    }
}
