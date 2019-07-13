using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.TraineeOperation
{
    class FuzzyTraineeViewModel : ViewModelBase
    {
        private string _traineeClassName;

        public string TraineeClassName
        {
            get { return _traineeClassName; }
            set
            {
                _traineeClassName = value;
                RaisePropertyChanged("TraineeClassName");
            }
        }

        private string _traineeName;

        public string TraineeName
        {
            get { return _traineeName; }
            set
            {
                _traineeName = value;
                RaisePropertyChanged("TraineeName");
            }
        }

        private Color _itemFore;

        public Color ItemFore
        {
            get { return _itemFore; }
            set
            {
                _itemFore = value;
                RaisePropertyChanged("ItemFore");
            }
        }

        private Color _itemBack;

        public Color ItemBack
        {
            get { return _itemBack; }
            set
            {
                _itemBack = value;
                RaisePropertyChanged("ItemBack");
            }
        }

        private DelegateCommand _itemMouseIn;

        public DelegateCommand ItemMouseIn
        {
            get
            {
                _itemMouseIn = _itemMouseIn ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ChangeColor(true);
                    }));
                return _itemMouseIn;
            }
            set { _itemMouseIn = value; }
        }

        private DelegateCommand _itemMouseOut;

        public DelegateCommand ItemMouseOut
        {
            get
            {
                _itemMouseOut = _itemMouseOut ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ChangeColor(false);
                    }));
                return _itemMouseOut;
            }
            set
            {
                _itemMouseOut = value;
                RaisePropertyChanged("ItemMouseOut");
            }
        }

        private DelegateCommand _itemClicked;

        public DelegateCommand ItemClicked
        {
            get
            {
                _itemClicked = _itemClicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        TraineeSelectedEvent?.Invoke(_trainee);
                    }));

                return _itemClicked;
            }
            set { _itemClicked = value; }
        }

        private void ChangeColor(bool isMouseIn)
        {
            ItemBack = isMouseIn ? GlobalVariables.LightMainColor : Colors.White;
            ItemFore = isMouseIn ? Colors.White : GlobalVariables.MainColor;
        }

        public delegate void TraineeSelected(TraineeModel trainee);
        public event TraineeSelected TraineeSelectedEvent;

        TraineeModel _trainee;
        public FuzzyTraineeViewModel(TraineeModel trainee, ClassType classType)
        {
            TraineeClassName = classType == ClassType.Regular ? string.Empty : trainee.RegularClassName;
            TraineeName = trainee.TraineeName;
            _trainee = trainee;
            ChangeColor(false);
        }
    }
}
