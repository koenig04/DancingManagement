using BLL.TeacherManagement;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace DancingTrainingManagement.Components.TeachingManagement.TeacherManagement
{
    class TeacherManagementViewModel : ViewModelBase
    {
        private int _detailState;

        public int DetailState
        {
            get { return _detailState; }
            set
            {
                _detailState = value;
                RaisePropertyChanged("DetailState");
            }
        }

        private Visibility _visDetail;

        public Visibility VisDetail
        {
            get { return _visDetail; }
            set
            {
                _visDetail = value;
                RaisePropertyChanged("VisDetail");
            }
        }

        private Visibility _errName;

        public Visibility ErrNameVis
        {
            get { return _errName; }
            set
            {
                _errName = value;
                RaisePropertyChanged("ErrNameVis");
            }
        }

        private Visibility _errFee;

        public Visibility ErrFeeVis
        {
            get { return _errFee; }
            set
            {
                _errFee = value;
                RaisePropertyChanged("ErrFeeVis");
            }
        }


        private TeacherListViewModel _list;

        public TeacherListViewModel TeacherList
        {
            get { return _list; }
            set
            {
                _list = value;
                RaisePropertyChanged("TeacherList");
            }
        }

        private string _name;

        public string TeacherName
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("TeacherName");
            }
        }

        private string _fee;

        public string TeacherFee
        {
            get { return _fee; }
            set
            {
                _fee = value;
                RaisePropertyChanged("TeacherFee");
            }
        }

        private DelegateCommand _add;

        public DelegateCommand AddTeacher
        {
            get
            {
                _add = _add ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (CheckValidity())
                        {
                            TeacherManagementBussiness.Instance.AddTeacher(new TeacherModel()
                            {
                                TeacherName = TeacherName,
                                TeacherFee = decimal.Parse(TeacherFee)
                            });
                            VisDetail = Visibility.Hidden;
                        }
                    }
               ));
                return _add;
            }
            set
            {
                _add = value;
                RaisePropertyChanged("AddTeacher");
            }
        }

        private DelegateCommand _update;

        public DelegateCommand UpdateTeacher
        {
            get
            {
                _update = _update ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (CheckValidity())
                        {
                            _teacher.TeacherName = TeacherName;
                            _teacher.TeacherFee = decimal.Parse(TeacherFee);
                            TeacherManagementBussiness.Instance.UpdateTeacher(_teacher);
                            VisDetail = Visibility.Hidden;
                        }
                    }));
                return _update;
            }
            set
            {
                _update = value;
                RaisePropertyChanged("UpdateTeacher");
            }
        }

        private TeacherModel _teacher;
        public TeacherManagementViewModel()
        {
            TeacherList = new TeacherListViewModel();
            TeacherList.OperateTeacherEvent += (operation, model) =>
             {
                 EnableDetail(operation, model);
             };
            VisDetail = Visibility.Hidden;
            ErrFeeVis = Visibility.Hidden;
            ErrNameVis = Visibility.Hidden;
        }

        private void EnableDetail(OperationType operation, TeacherModel teacher)
        {
            DetailState = operation == OperationType.Add ? 0 : 1;
            TeacherName = operation == OperationType.Add ? string.Empty : teacher.TeacherName;
            TeacherFee = operation == OperationType.Add ? string.Empty : teacher.TeacherFee.ToString();
            _teacher = teacher;
            VisDetail = Visibility.Visible;
        }

        private bool CheckValidity()
        {
            ErrNameVis = Visibility.Hidden;
            ErrFeeVis = Visibility.Hidden;
            if (string.IsNullOrEmpty(TeacherName))
            {
                ErrNameVis = Visibility.Visible;
                return false;
            }
            if (string.IsNullOrEmpty(TeacherFee) || !Regex.IsMatch(TeacherFee, @"^[+-]?\d*[.]?\d*$"))
            {
                ErrFeeVis = Visibility.Visible;
                return false;
            }
            return true;
        }
    }
}
