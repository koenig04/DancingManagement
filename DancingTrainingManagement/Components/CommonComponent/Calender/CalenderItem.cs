using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.CommonComponent.Calender
{
    class CalenderItem : ViewModelBase
    {
        private string _day;

        public string Day
        {
            get { return _day; }
            set
            {
                _day = value;
                RaisePropertyChanged("Day");
            }
        }

        private Color color_;

        public Color DayColor
        {
            get { return color_; }
            set { color_ = value; RaisePropertyChanged("DayColor"); }
        }


        private ObservableCollection<ItemContent> _itemContents;

        public ObservableCollection<ItemContent> ItemContents
        {
            get { return _itemContents; }
            set
            {
                _itemContents = value;
                RaisePropertyChanged("ItemContents");
            }
        }

        const int TOTAL_HEIGHT = 80;

        public CalenderItem(int year,int month, int day)
        {
            if (day > 0)
            {
                Day = day.ToString();
                DateTime currentDay = Convert.ToDateTime(year.ToString() + "-" + month.ToString() + "-" + day.ToString());
                var dayOfWeek = currentDay.DayOfWeek;
                if(dayOfWeek== DayOfWeek.Saturday||dayOfWeek== DayOfWeek.Sunday)
                {
                    DayColor = Common.GlobalVariables.ExpenseColor;
                }else
                {
                    DayColor = Common.GlobalVariables.MainColor;
                }

                ItemContents = new ObservableCollection<ItemContent>();

                AdjustHeight();
            }
        }

        public void UpdataContents(List<string> contents)
        {
            ItemContents.Clear();
            foreach (string content in contents)
            {
                ItemContents.Add(new ItemContent(content));
            }
            AdjustHeight();
        }

        private void AdjustHeight()
        {
            foreach (ItemContent content in ItemContents)
            {
                content.ContentHeight = TOTAL_HEIGHT / ItemContents.Count;
            }
        }
    }
}
