using BLL.TeachingManagement.BlockTeaching;
using DancingTrainingManagement.Components.TeachingManagement.Common.ClassList;
using DancingTrainingManagement.Components.TeachingManagement.Common.TraineeList;
using DancingTrainingManagement.Components.TeachingManagement.Common.TraineeOperation;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement.BlockTeaching
{
    class BlockTeachingViewModel : ViewModelBase
    {
        private BlockClassListViewModel _classList;
        /// <summary>
        /// 左侧班级列表
        /// </summary>
        public BlockClassListViewModel ClassList
        {
            get { return _classList; }
            set
            {
                _classList = value;
                RaisePropertyChanged("ClassList");
            }
        }

        private BlockClassOperationViewModel _classOperation;
        /// <summary>
        /// 点击左侧班级列表弹出的班级编辑页面
        /// </summary>
        public BlockClassOperationViewModel ClassOperation
        {
            get { return _classOperation; }
            set
            {
                _classOperation = value;
                RaisePropertyChanged("ClassOperation");
            }
        }

        private BlockTraineeListViewModel _trainees;

        public BlockTraineeListViewModel TraineeList
        {
            get { return _trainees; }
            set
            {
                _trainees = value;
                RaisePropertyChanged("TraineeList");
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


        private BlockTeachingBussiness _bussiness;
        public BlockTeachingViewModel(BlockTeachingBussiness bussiness)
        {
            _bussiness = bussiness;
            ClassList = new BlockClassListViewModel(bussiness.BlockClasses);
            ClassOperation = new BlockClassOperationViewModel(bussiness.BlockClassOperation);
            TraineeList = new BlockTraineeListViewModel(bussiness.BlockTrainee);
            TraineeOperation = new BlockTraineeOperationViewModel(bussiness.TraineeManagement, bussiness.BlockTrainee.TraineeOperation, bussiness.BlockClasses);
        }
    }
}
