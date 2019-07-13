using BLL.TeachingManagement;
using Common;
using DancingTrainingManagement.Components.TeachingManagement.BlockTeaching;
using DancingTrainingManagement.Components.TeachingManagement.Others;
using DancingTrainingManagement.Components.TeachingManagement.RegularTeaching;
using DancingTrainingManagement.Components.TeachingManagement.TeacherManagement;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement
{
    /// <summary>
    /// 教学管理的ViewModel
    /// 包含RegularTeaching 
    /// </summary>
    class TeachingManagementViewModel : ViewModelBase
    {
        private int _teachingFunc;

        public int TeachingFunc
        {
            get { return _teachingFunc; }
            set
            {
                _teachingFunc = value;
                Regular.OnOperateEnableEvent(true, value == 0 ? true : false);
                Block.OnOperateEnableEvent(true, value == 1 ? true : false);
                Teacher.OnOperateEnableEvent(true, value == 2 ? true : false);
                Others.OnOperateEnableEvent(true, value == 3 ? true : false);
                RaisePropertyChanged("TeachingFunc");
            }
        }

        private DelegateCommand _changeFunc;

        public DelegateCommand ChangeTeachingFunc
        {
            get
            {
                _changeFunc = _changeFunc ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        TeachingFunc = int.Parse(o.ToString());
                    }));
                return _changeFunc;
            }
            set
            {
                _changeFunc = value;
                RaisePropertyChanged("ChangeTeachingFunc");
            }
        }

        private RegularTeachingViewModel _regular;

        public RegularTeachingViewModel Regular
        {
            get { return _regular; }
            set
            {
                _regular = value;
                RaisePropertyChanged("Regular");
            }
        }

        private BlockTeachingViewModel _block;

        public BlockTeachingViewModel Block
        {
            get { return _block; }
            set
            {
                _block = value;
                RaisePropertyChanged("Block");
            }
        }

        private TeacherManagementViewModel _teacher;

        public TeacherManagementViewModel Teacher
        {
            get { return _teacher; }
            set
            {
                _teacher = value;
                RaisePropertyChanged("Teacher");
            }
        }

        private OthersViewModel _others;

        public OthersViewModel Others
        {
            get { return _others; }
            set
            {
                _others = value;
                RaisePropertyChanged("Others");
            }
        }


        private TeachingManagementBussiness _bussiness;
        public TeachingManagementViewModel(TeachingManagementBussiness bussiness)
        {
            _bussiness = bussiness;
            Regular = new RegularTeachingViewModel(bussiness.Regular);
            Block = new BlockTeachingViewModel(bussiness.Block);
            Teacher = new TeacherManagementViewModel();
            Others = new OthersViewModel(bussiness.Others);
            TeachingFunc = 0;
        }
    }
}
