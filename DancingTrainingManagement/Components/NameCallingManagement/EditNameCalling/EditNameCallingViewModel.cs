using BLL.NameCallingManagement;
using Common;
using DancingTrainingManagement.Components.NameCallingManagement.NameCalling;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.NameCallingManagement.EditNameCalling
{
    class EditNameCallingViewModel : ViewModelBase
    {
        private DateTime _classDate = DateTime.Now;

        public DateTime ClassDate
        {
            get { return _classDate; }
            set
            {
                _classDate = value;
                _nameCalling.ChangeEditDate(value);
                Calling.ClearAll();
                RaisePropertyChanged("ClassDate");
            }
        }

        private int _selectedCalling;

        public int SelectedCalling
        {
            get { return _selectedCalling; }
            set
            {
                _selectedCalling = value;
                if (value >= 0)
                    Calling.LoadByCalling(_callings[value]);
                RaisePropertyChanged("SelectedCalling");
            }
        }

        private ObservableCollection<string> _callingCollection;

        public ObservableCollection<string> CallingCollection
        {
            get { return _callingCollection; }
            set
            {
                _callingCollection = value;
                RaisePropertyChanged("CallingCollection");
            }
        }

        private NameCallingViewModel _calling;

        public NameCallingViewModel Calling
        {
            get { return _calling; }
            set
            {
                _calling = value;
                RaisePropertyChanged("Calling");
            }
        }

        public delegate void ErrOccured(MessageType msg, MessageLevel level = MessageLevel.Warning);
        public event ErrOccured ErrOccuredEvent;

        private EditNameCallingBussiness _nameCalling;
        private List<NameCallingModel> _callings;
        public EditNameCallingViewModel(EditNameCallingBussiness nameCalling)
        {
            _nameCalling = nameCalling;
            _nameCalling.CallingsChangedEvent += callings =>
            {
                CallingCollection.Clear();
                if (callings != null)
                    callings.ForEach(c => CallingCollection.Add(c.ClassName));
                _callings = callings;
                SelectedCalling = -1;
            };

            CallingCollection = new ObservableCollection<string>();
            Calling = new NameCallingViewModel(nameCalling.Calling, nameCalling.ClassSelecter, false);
            Calling.NameCallingChangedEvent += (op, calling) =>
            {
                _nameCalling.OperateCalling(op, calling);
                _nameCalling.ChangeEditDate(ClassDate);
            };
            Calling.ErrOccuredEvent += (msg, level) => ErrOccuredEvent?.Invoke(msg);
        }
    }
}
