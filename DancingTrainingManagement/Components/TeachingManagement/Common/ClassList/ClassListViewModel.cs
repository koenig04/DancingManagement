using DancingTrainingManagement.UICore;
using Model;
using System.Collections.ObjectModel;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.ClassList
{
    class ClassListViewModel : ViewModelBase
    {
        private ObservableCollection<ClassViewModel> _classes;

        public ObservableCollection<ClassViewModel> Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }        

        public ClassListViewModel()
        {
            Classes = new ObservableCollection<ClassViewModel>();
           
        }        
    }
}
