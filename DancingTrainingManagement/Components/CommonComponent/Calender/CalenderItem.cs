using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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

        public CalenderItem(int day)
        {
            Day = day.ToString();
            ItemContents = new ObservableCollection<ItemContent>();
            for (int i = 0; i < 3; i++)
            {
                ItemContents.Add(new ItemContent("三级1班  *2"));
            }
            AdjustHeight();
        }

        public void UpdataContents(List<string> contents)
        {
            ItemContents.Clear();
            foreach(string content in contents)
            {
                ItemContents.Add(new ItemContent(content));
            }
            AdjustHeight();
        }

        private void AdjustHeight()
        {
            foreach(ItemContent content in ItemContents)
            {
                content.ContentHeight = TOTAL_HEIGHT / ItemContents.Count;
            }
        }
    }
}
