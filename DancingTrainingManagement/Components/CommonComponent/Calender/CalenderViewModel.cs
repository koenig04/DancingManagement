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

        public void UpdateDateInfo(int year, int month)
        {
            DayCollection.Clear();

            DateTime firstDay = Convert.ToDateTime(year.ToString() + "-" + month.ToString() + "-1");
            int index = Convert.ToInt32(firstDay.DayOfWeek);
            index--;
            if (index < 0)
            {
                index = 6;
            }

            for (int i = 0; i < index; i++)
            {
                //add blank
                DayCollection.Add(new CalenderItem(year, month, -1));
            }

            int monthDayCount = (int)(firstDay.AddMonths(1) - firstDay).TotalDays;
            for (int i = 1; i <= monthDayCount; i++)
            {
                DayCollection.Add(new CalenderItem(year, month, i));
            }
        }

        public void UpdateInfo(List<CalenderItemInfoModel> infos)
        {
            foreach(var info in infos)
            {
                var calenderDay = DayCollection.Where(d => d.Day == info.Day).FirstOrDefault();
                calenderDay.UpdataContents(info.Content);
            }
        }

        private void Init()
        {
            DayCollection = new ObservableCollection<CalenderItem>();
        }
    }
}
