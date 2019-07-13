using BLL.TeacherManagement;
using BLL.TeachingManagement.BlockTeaching;
using Common;
using DancingTrainingManagement.Components.TeachingManagement.Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace DancingTrainingManagement.Components.TeachingManagement.BlockTeaching
{
    class BlockClassOperationViewModel : ClassOperationViewModel
    {
        private int _classSeason;

        public int ClassSeason
        {
            get { return _classSeason; }
            set
            {
                _classSeason = value;
                RaisePropertyChanged("ClassSeason");
            }
        }

        private string _selectedYear;

        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                RaisePropertyChanged("SelectedYear");
            }
        }

        private Visibility _errYear;

        public Visibility ErrYearVis
        {
            get { return _errYear; }
            set
            {
                _errYear = value;
                RaisePropertyChanged("ErrYearVis");
            }
        }

        private Visibility _errSeason;

        public Visibility ErrSeasonVis
        {
            get { return _errSeason; }
            set { _errSeason = value;
                RaisePropertyChanged("ErrSeasonVis");
            }
        }


        private ObservableCollection<string> _yearCollection;

        public ObservableCollection<string> YearCollection
        {
            get { return _yearCollection; }
            set
            {
                _yearCollection = value;
                RaisePropertyChanged("YearCollection");
            }
        }  

        private DelegateCommand _confirm;

        public DelegateCommand Confirm
        {
            get
            {
                _confirm = _confirm ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (!CheckValidity())
                        {
                            return;
                        }
                        _model.ClassSeason = ClassSeason;
                        _model.ClassType = BlockClassType.Instance.BlockClassTypeCollection.Where(t => t.Name == SelectedClassType).First().Type;
                        _model.TeacherID = TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherName == SelectedTeacher).First().TeacherID;
                        _model.TeacherName = SelectedTeacher;
                        _model.CostPerTerm = decimal.Parse(CostPerTerm);
                        _model.ClassYear = int.Parse(SelectedYear);
                        _bussiness.OperateBlockClass(_operation, _model);
                        Vis = Visibility.Hidden;
                    }));
                return _confirm;
            }
            set
            {
                _confirm = value;
                RaisePropertyChanged("Confirm");
            }
        }

        private OperationType _operation;
        private BlockClassModel _model;
        private BlockClassOperationBussiness _bussiness;
        public BlockClassOperationViewModel(BlockClassOperationBussiness bussiness) : base()
        {
            _bussiness = bussiness;
            _bussiness.OperationEnableEvent += Enable;
            SelectedYear = DateTime.Now.Year.ToString();
            YearCollection = new ObservableCollection<string>();
            for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 3; i++)
            {
                YearCollection.Add(i.ToString());
            }
            BlockClassType.Instance.BlockClassTypeCollection.ForEach(t => ClassTypeCollection.Add(t.Name));
        }

        private void Enable(OperationType operation, BlockClassModel model)
        {
            if (operation == OperationType.Add || operation == OperationType.Update)
            {
                switch (operation)
                {
                    case OperationType.Add:
                        Title = "增加独舞班";
                        SelectedYear = string.Empty;
                        SelectedTeacher = string.Empty;
                        CostPerTerm = string.Empty;
                        SelectedClassType = string.Empty;
                        ClassSeason = -1;
                        break;
                    case OperationType.Update:
                        Title = "修改独舞班";
                        SelectedYear = model.ClassYear.ToString();
                        SelectedTeacher = model.TeacherName;
                        CostPerTerm = model.CostPerTerm.ToString();
                        ClassSeason = model.ClassSeason;
                        SelectedClassType = BlockClassType.Instance.BlockClassTypeCollection.Where(t => t.Type == model.ClassType).First().Name;
                        break;
                }
                _operation = operation;
                _model = model ?? new BlockClassModel();
                HideErr();
                Vis = Visibility.Visible;
            }
        }

        protected override void HideErr()
        {
            base.HideErr();
            ErrSeasonVis = Visibility.Hidden;
            ErrYearVis = Visibility.Hidden;
        }

        private bool CheckValidity()
        {
            bool res = true;
            HideErr();
            if (string.IsNullOrEmpty(CostPerTerm) || !Regex.IsMatch(CostPerTerm, @"^[+-]?\d*[.]?\d*$"))
            {//检查每期课费是否为数字
                ErrVis = Visibility.Visible;
                res = false && res;
            }
            if (string.IsNullOrEmpty(SelectedTeacher))
            {
                ErrTeacherVis = Visibility.Visible;
                res = false && res;
            }
            if (ClassSeason < 0)
            {
                ErrSeasonVis = Visibility.Visible;
                res = false && res;
            }
            if (string.IsNullOrEmpty(SelectedYear))
            {
                ErrYearVis = Visibility.Visible;
                res = false && res;
            }
            if (string.IsNullOrEmpty(SelectedClassType))
            {
                ErrClassTypeVis = Visibility.Visible;
                res = false && res;
            }

            return res;
        }
    }
}
