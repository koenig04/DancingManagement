using Common;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.UICore;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic
{
    class ConcreteStatisticViewModelBase : ViewModelBase
    {
        private MessageViewModel _msg;

        public MessageViewModel Msg
        {
            get { return _msg; }
            set
            {
                _msg = value;
                RaisePropertyChanged("Msg");
            }
        }

        public ConcreteStatisticViewModelBase()
        {
            Msg = new MessageViewModel(false);
            Msg.OnOperateEnableEvent(false, false);
        }

        protected bool EnableErrMsg(MessageType msg)
        {
            Msg.Enable(msg, MessageLevel.Warning);
            return false;
        }

        protected ColumnSeries CreateOneColumn(string title, int colorIndex)
        {
            return new ColumnSeries()
            {
                Title = title,
                DataLabels = true,
                LabelPoint = point => point.Y.ToString("#0"),
                FontSize = 16,
                Fill = new SolidColorBrush(GlobalVariables.CustomerColorSet[colorIndex % 12]),
                Values = new ChartValues<decimal>()
            };
        }

        protected ColumnSeries CreateOneColumn(string title, Color color)
        {
            return new ColumnSeries()
            {
                Title = title,
                DataLabels = true,
                LabelPoint = point => point.Y.ToString("#0"),
                FontSize = 16,
                Fill = new SolidColorBrush(color),
                Values = new ChartValues<decimal>()
            };
        }

        protected bool CheckValidity(DateTime startDate, DateTime endDate)
        {
            if (startDate.Year < 2015)
            {
                return EnableErrMsg(MessageType.StartDateErr);
            }
            if (endDate.Year < 2015)
            {
                return EnableErrMsg(MessageType.EndDateErr);
            }
            return true;
        }
    }
}
