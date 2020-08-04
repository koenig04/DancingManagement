using BLL.ClassManagement;
using BLL.CommonBussiness;
using BLL.TeacherManagement;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.CommonComponent.ClassSelecter
{
    class ClassSelecterViewModel : ViewModelBase
    {
        private int _classType;

        public int ClassType
        {
            get { return _classType; }
            set
            {
                _classType = value;
                _bussiness.ChangeClassType(value);
                RaisePropertyChanged("ClassType");
            }
        }

        private int _classWidth = 100;

        public int ClassComboWidth
        {
            get { return _classWidth; }
            set
            {
                _classWidth = value;
                RaisePropertyChanged("ClassComboWidth");
            }
        }


        private string _selectedClass;

        public string SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                _selectedClass = value;
                _bussiness.ChangeClass(value);
                RaisePropertyChanged("SelectedClass");
            }
        }

        private ObservableCollection<string> _classes;

        public ObservableCollection<string> ClassCollection
        {
            get { return _classes; }
            set
            {
                _classes = value;
                RaisePropertyChanged("ClassCollection");
            }
        }


        private DelegateCommand _changeClassType;

        public DelegateCommand ChangeClassType
        {
            get
            {
                _changeClassType = _changeClassType ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ClassType = int.Parse(o.ToString());
                    }));
                return _changeClassType;
            }
            set
            {
                _changeClassType = value;
                RaisePropertyChanged("ChangeClassType");
            }
        }

        private string _selectedTeacher;

        public string SelectedTeacher
        {
            get { return _selectedTeacher; }
            set
            {
                _selectedTeacher = value;
                _bussiness.ChangeTeacher(value);
                RaisePropertyChanged("SelectedTeacher");
            }
        }

        private ObservableCollection<string> _teachers;

        public ObservableCollection<string> TeacherCollection
        {
            get { return _teachers; }
            set
            {
                _teachers = value;
                RaisePropertyChanged("TeacherCollection");
            }
        }

        private Visibility _visTeacher;

        public Visibility VisTeacher
        {
            get { return _visTeacher; }
            set
            {
                _visTeacher = value;
                RaisePropertyChanged("VisTeacher");
            }
        }

        private string _selectedTrainee;

        public string SelectedTrainee
        {
            get { return _selectedTrainee; }
            set
            {
                _selectedTrainee = value;
                RaisePropertyChanged("SelectedTrainee");
            }
        }

        private ObservableCollection<string> _trainees;

        public ObservableCollection<string> TraineeCollection
        {
            get { return _trainees; }
            set
            {
                _trainees = value;
                RaisePropertyChanged("TraineeCollection");
            }
        }

        private Visibility _visTrainee;

        public Visibility VisTrainee
        {
            get { return _visTrainee; }
            set
            {
                _visTrainee = value;
                RaisePropertyChanged("VisTrainee");
            }
        }


        public void ResetClassType()
        {
            ClassType = -1;
        }

        public delegate void TraineesChanged(List<TraineeModel> trainees);
        public event TraineesChanged TraineesChangedEvent;

        private ClassSelecterBussiness _bussiness;
        private List<TraineeModel> _traineeCollection;

        public ClassSelecterViewModel(ClassSelecterBussiness bussiness, bool isTeacherShown, bool enableTraineeFunc = true)
        {
            _bussiness = bussiness;

            ClassCollection = new ObservableCollection<string>();
            TeacherCollection = new ObservableCollection<string>();
            TraineeCollection = new ObservableCollection<string>();
            TeacherManagementBussiness.Instance.Teachers.ForEach(t => TeacherCollection.Add(t.TeacherName));
            TeacherManagementBussiness.Instance.TeacherChangedEvent += (operation, teacher, previousTeacher) =>
            {
                if (operation == OperationType.Update)
                {
                    TeacherCollection.Remove(previousTeacher.TeacherName);
                }
                TeacherCollection.Add(teacher.TeacherName);
            };
            VisTeacher = isTeacherShown ? Visibility.Visible : Visibility.Collapsed;
            VisTrainee = enableTraineeFunc ? Visibility.Visible : Visibility.Collapsed;
            ClassType = -1;

            _bussiness.ClassCollectionChangedEvent += classes =>
            {
                SelectedClass = string.Empty;
                ClassCollection.Clear();
                classes.ForEach(c => ClassCollection.Add(c.ClassName));
                ClassComboWidth = (Common.ClassType)_bussiness.CurrentClassType == Common.ClassType.Regular ? 100 : 200;
            };

            _bussiness.TeacherChangedEvent += teacher => SelectedTeacher = teacher;

            if (enableTraineeFunc)
            {
                _bussiness.TraineeCollectionChangedEvent += trainees =>
                  {
                      TraineeCollection.Clear();
                      if (trainees != null)
                      {
                          trainees.ForEach(t => TraineeCollection.Add(t.TraineeName));
                          _traineeCollection = trainees;
                      }
                  };
            }
            else
            {
                _bussiness.TraineeCollectionChangedEvent += trainees => TraineesChangedEvent?.Invoke(trainees.Where(t => t.State == 0).ToList());
            }
        }

        public string GetCurrentClassID()
        {
            return _bussiness.CurrentClassID;
        }

        public string GetCurrentTeacherID()
        {
            return _bussiness.CurrentTeacherID;
        }

        public string GetCurrentTraineeID()
        {
            return _traineeCollection.Where(t => t.TraineeName == SelectedTrainee).First().TraineeID;
        }

        public void ClearAll()
        {
            SelectedClass = string.Empty;
            SelectedTeacher = string.Empty;
            SelectedTrainee = string.Empty;
            ClassType = -1;
        }

        public void LoadByCalling(NameCallingModel calling)
        {
            ClassType = calling.ClassType;
            SelectedClass = calling.ClassName;
            SelectedTeacher = calling.TeacherName;
        }
    }
}
