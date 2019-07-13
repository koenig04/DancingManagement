using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.TraineeList
{
    class TraineeListViewModel : ViewModelBase
    {
        private ObservableCollection<TraineeViewModel> _trainees;

        public ObservableCollection<TraineeViewModel> Trainees
        {
            get { return _trainees; }
            set
            {
                _trainees = value;
                RaisePropertyChanged("Trainees");
            }
        }

        public TraineeListViewModel()
        {
            Trainees = new ObservableCollection<TraineeViewModel>();
        }
    }
}
