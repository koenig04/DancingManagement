using Common;
using DancingTrainingManagement.UICore;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Details
{
    class TeachingCountDetailsViewModel : DetailViewModelBase
    {
        private ObservableCollection<TeachingCountViewModel> _collection;

        public ObservableCollection<TeachingCountViewModel> AccountRecordsCollection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                RaisePropertyChanged("AccountRecordsCollection");
            }
        }

        public TeachingCountDetailsViewModel()
        {
            AccountRecordsCollection = new ObservableCollection<TeachingCountViewModel>();
            TotalWidth = 200;
        }

        public void Enable(List<ClassInfoForTeacherModel> details)
        {
            AccountRecordsCollection.Clear();
            foreach (ClassInfoForTeacherModel item in details)
                AccountRecordsCollection.Add(new TeachingCountViewModel(item.ClassName, item.ClassDate));
            PublicMathods.Sort(AccountRecordsCollection);
        }
    }
}
