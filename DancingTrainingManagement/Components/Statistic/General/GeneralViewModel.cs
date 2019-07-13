using BLL.StatisticManagement.GeneralAndExport;
using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.General
{
    class GeneralViewModel : ViewModelBase
    {
        private string _traineeCount;

        public string TraineeCount
        {
            get { return _traineeCount; }
            set
            {
                _traineeCount = value;
                RaisePropertyChanged("TraineeCount");
            }
        }

        private string _currentCapital;

        public string CurrentCapital
        {
            get { return _currentCapital; }
            set
            {
                _currentCapital = value;
                RaisePropertyChanged("CurrentCapital");
            }
        }

        private Color _capitalColor;

        public Color CapitalColor
        {
            get { return _capitalColor; }
            set
            {
                _capitalColor = value;
                RaisePropertyChanged("CapitalColor");
            }
        }

        private GeneralAndExportBussiness _bussiness;

        public GeneralViewModel(GeneralAndExportBussiness bussiness)
        {
            _bussiness = bussiness;
            _bussiness.CurrentCapitalChangedEvent += capital =>
            {
                CurrentCapital = capital.ToString();
                CapitalColor = capital > 0 ? GlobalVariables.IncomeColor : GlobalVariables.ExpenseColor;
            };
            _bussiness.TraineeCountChangedEvent += count => TraineeCount = count.ToString();
            _bussiness.Init();
        }
    }
}
