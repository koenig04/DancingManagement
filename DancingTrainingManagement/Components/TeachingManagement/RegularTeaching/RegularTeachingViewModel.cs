using BLL.TeachingManagement.RegularTeaching;
using DancingTrainingManagement.Components.TeachingManagement.Common.ClassList;
using DancingTrainingManagement.Components.TeachingManagement.Common.TraineeList;
using DancingTrainingManagement.Components.TeachingManagement.Common.TraineeOperation;
using DancingTrainingManagement.UICore;

namespace DancingTrainingManagement.Components.TeachingManagement.RegularTeaching
{
    /// <summary>
    /// RegularTeaching的ViewModel
    /// </summary>
    class RegularTeachingViewModel : ViewModelBase
    {
        private RegularClassListViewModel _classList;
        /// <summary>
        /// 左侧班级列表
        /// </summary>
        public RegularClassListViewModel ClassList
        {
            get { return _classList; }
            set
            {
                _classList = value;
                RaisePropertyChanged("ClassList");
            }
        }

        private TraineeListViewModel _traineeList;
        /// <summary>
        /// 右侧学员列表
        /// </summary>
        public TraineeListViewModel TraineeList
        {
            get { return _traineeList; }
            set
            {
                _traineeList = value;
                RaisePropertyChanged("TraineeList");
            }
        }


        private RegularClassOperationViewModel _classOperation;
        /// <summary>
        /// 点击左侧班级列表弹出的班级编辑页面
        /// </summary>
        public RegularClassOperationViewModel ClassOperation
        {
            get { return _classOperation; }
            set
            {
                _classOperation = value;
                RaisePropertyChanged("ClassOperation");
            }
        }

        private TraineeOperationViewModel _traineeOperation;
        /// <summary>
        /// 点击右侧学员列表弹出的学员编辑页面
        /// </summary>
        public TraineeOperationViewModel TraineeOperation
        {
            get { return _traineeOperation; }
            set
            {
                _traineeOperation = value;
                RaisePropertyChanged("TraineeOperation");
            }
        }


        private RegularTeachingBussiness _bussiness;
        public RegularTeachingViewModel(RegularTeachingBussiness bussiness)
        {
            _bussiness = bussiness;
            ClassList = new RegularClassListViewModel(bussiness.RegularClasses);
            ClassOperation = new RegularClassOperationViewModel(bussiness.RegularClassOperation);
            TraineeList = new RegularTraineeListViewModel(bussiness.RegularTrainee);
            TraineeOperation = new RegularTraineeOperationViewModel(bussiness.TraineeManagement, bussiness.RegularTrainee.TraineeOperation, bussiness.RegularClasses);
        }
    }
}
