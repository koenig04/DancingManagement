﻿using BLL.TeacherManagement;
using BLL.TeachingManagement.RegularTeaching;
using Common;
using DancingTrainingManagement.Components.TeachingManagement.Common;
using DancingTrainingManagement.UICore;
using Model;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace DancingTrainingManagement.Components.TeachingManagement.RegularTeaching
{
    /// <summary>
    /// 班级编辑页面的ViewModel
    /// </summary>
    class RegularClassOperationViewModel : ClassOperationViewModel
    {
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
                        _model.ClassType = RegularClassType.Instance.RegularClassTypeCollection.Where(c => c.Name == SelectedClassType).First().Level;
                        _model.TeacherID = TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherName == SelectedTeacher).First().TeacherID;
                        _model.TeacherName = SelectedTeacher;
                        _model.CostPerTerm = decimal.Parse(CostPerTerm);
                        _bussiness.OperateRegularClass(_operation, _model);
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

        private DelegateCommand _addClassGiving;

        public DelegateCommand AddClassGiving
        {
            get
            {
                _addClassGiving = _addClassGiving ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (Giving.Vis != Visibility.Visible)
                            AddGivingEvent?.Invoke(_model.ClassID);
                    }));

                return _addClassGiving;
            }
            set
            {
                _addClassGiving = value;
                RaisePropertyChanged("AddClassGiving");
            }
        }

        private GivingClassViewModel giving_;

        public GivingClassViewModel Giving
        {
            get { return giving_; }
            set { giving_ = value; RaisePropertyChanged("Giving"); }
        }


        private bool givingEnable_;

        public bool GivingEnable
        {
            get { return givingEnable_; }
            set { givingEnable_ = value; RaisePropertyChanged("GivingEnable"); }
        }


        public delegate void AddGiving(string classID);
        public event AddGiving AddGivingEvent;
        public delegate void UpdateGiving(ClassGivingInfoModel model);
        public event UpdateGiving UpdateGivingEvent;

        private RegularClassOperationBussiness _bussiness;
        private OperationType _operation;
        private RegularClassModel _model;
        public RegularClassOperationViewModel(RegularClassOperationBussiness bussiness) : base()
        {
            _bussiness = bussiness;
            _bussiness.OperationEnableEnvet += Enable;

            ClassTypeCollection = new ObservableCollection<string>();
            RegularClassType.Instance.RegularClassTypeCollection.ForEach(c => ClassTypeCollection.Add(c.Name));


            Giving = new GivingClassViewModel();
            Giving.GivingModifyEvent += (ClassGivingInfoModel giving) => UpdateGivingEvent?.Invoke(giving);
            Giving.GivingDelEvent += (ClassGivingInfoModel info) =>
            {
                _bussiness.DelGiving(info);
            };
        }

        private void Enable(OperationType operation, RegularClassModel model)
        {
            if (operation == OperationType.Add || operation == OperationType.Update)
            {
                switch (operation)
                {
                    case OperationType.Add:
                        Title = "增加常规班级";
                        SelectedClassType = string.Empty;
                        SelectedTeacher = string.Empty;
                        CostPerTerm = string.Empty;
                        break;
                    case OperationType.Update:
                        Title = "修改常规班级";
                        SelectedClassType = RegularClassType.Instance.RegularClassTypeCollection.Where(c => c.Level == model.ClassType).First().Name;
                        SelectedTeacher = model.TeacherName;
                        CostPerTerm = model.CostPerTerm.ToString();
                        break;
                }
                _operation = operation;
                _model = model ?? new RegularClassModel();

                Giving.Vis = Visibility.Hidden;
                if (operation == OperationType.Update)
                {
                    List<ClassGivingInfoModel> classes = _bussiness.GetGiving(model.ClassID);
                    if (classes != null)
                    {
                        Giving.Active(classes[0]);
                    }
                    GivingEnable = true;
                }
                else
                {
                    GivingEnable = false;
                }
                HideErr();
                Vis = Visibility.Visible;
            }
        }

        public void RefreshGiving()
        {
            Giving.Vis = Visibility.Hidden;
            List<ClassGivingInfoModel> classes = _bussiness.GetGiving(_model.ClassID);
            if (classes != null)
                Giving.Active(classes[0]);
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
            if (string.IsNullOrEmpty(SelectedClassType))
            {
                ErrClassTypeVis = Visibility.Visible;
                res = false && res;
            }
            return res;
        }
    }
}
