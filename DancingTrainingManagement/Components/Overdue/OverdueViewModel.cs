using BLL.OverdueManagement;
using DancingTrainingManagement.Components.CommonComponent.Message;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.Overdue
{
    class OverdueViewModel : ViewModelBase
    {
        private ObservableCollection<OverdueItemViewModel> _overdues;

        public ObservableCollection<OverdueItemViewModel> Overdues
        {
            get { return _overdues; }
            set
            {
                _overdues = value;
                RaisePropertyChanged("Overdues");
            }
        }

        private DelegateCommand _export;

        public DelegateCommand Export
        {
            get
            {
                _export = _export ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        if (_bussiness.Export())
                        {
                            Msg.Enable(Common.MessageType.ExportSuccess, Common.MessageLevel.Info);
                        }
                        else
                        {
                            Msg.Enable(Common.MessageType.ExportFailed, Common.MessageLevel.Warning);
                        }
                    }));
                return _export;
            }
            set
            {
                _export = value;
                RaisePropertyChanged("Export");
            }
        }

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

        private OverdueManagementBussiness _bussiness;
        public OverdueViewModel(OverdueManagementBussiness bussiness)
        {
            _bussiness = bussiness;
            Overdues = new ObservableCollection<OverdueItemViewModel>();
            bussiness.Overdues.ForEach(o => Overdues.Add(new OverdueItemViewModel(o)));
            bussiness.OverdueChangedEvent += () =>
            {
                Overdues.Clear();
                bussiness.Overdues.ForEach(o => Overdues.Add(new OverdueItemViewModel(o)));
            };

            Msg = new MessageViewModel(true);
            Msg.OnOperateEnableEvent(false, false);
        }
    }
}
