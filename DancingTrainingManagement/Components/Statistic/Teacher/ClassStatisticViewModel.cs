using BLL.StatisticManagement.TeachingStatistic;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.Teacher
{
    class ClassStatisticViewModel : ViewModelBase
    {
        private ObservableCollection<ClassItemViewModel> _classes;

        public ObservableCollection<ClassItemViewModel> Classes
        {
            get { return _classes; }
            set { _classes = value; RaisePropertyChanged("Classes"); }
        }

        private ObservableCollection<CLassCountViewModel> items_;

        public ObservableCollection<CLassCountViewModel> ItemCollection
        {
            get { return items_; }
            set { items_ = value; RaisePropertyChanged("ItemCollection"); }
        }

        private int height_;

        public int TotalClassHeight
        {
            get { return height_; }
            set { height_ = value; RaisePropertyChanged("TotalClassHeight"); }
        }


        public ClassStatisticViewModel()
        {
            Classes = new ObservableCollection<ClassItemViewModel>();
            ItemCollection = new ObservableCollection<CLassCountViewModel>();
            TotalClassHeight = 0;
        }

        private List<TeachingCountByClass> info_;
        public void UpdateClassList(List<TeachingCountByClass> info)
        {
            info_ = info;

            Classes.Clear();
            ItemCollection.Clear();

            if (info != null)
            {
                foreach (var i in info)
                {
                    Classes.Add(new ClassItemViewModel(i.ClassID, i.ClassName));
                    Classes.Last().ClassSelectedEvent += OnClassSelected;
                }
                TotalClassHeight = info.Count * 50+2;
            }
        }

        private void OnClassSelected(string id)
        {
            foreach (var c in Classes)
            {
                c.ChangeToUnselected(id);
            }
            ItemCollection.Clear();
            var countInfo = info_.Where(i => i.ClassID == id).First();
            foreach (var c in countInfo.Info.Keys)
            {
                ItemCollection.Add(new CLassCountViewModel(c.ToString("MM月dd日"), countInfo.Info[c].ToString()));
            }
        }
    }
}
