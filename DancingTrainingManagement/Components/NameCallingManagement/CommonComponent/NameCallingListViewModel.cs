using BLL.TeachingManagement.BlockTeaching;
using BLL.TeachingManagement.RegularTeaching;
using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Model;

namespace DancingTrainingManagement.Components.NameCallingManagement.CommonComponent
{
    class NameCallingListViewModel : ViewModelBase
    {
        private ObservableCollection<TraineeForCallingViewModel> _trainees;

        public ObservableCollection<TraineeForCallingViewModel> Trainees
        {
            get { return _trainees; }
            set
            {
                _trainees = value;
                RaisePropertyChanged("Trainees");
            }
        }

        private List<List<string>> _callingNames;

        public NameCallingListViewModel()
        {
            Trainees = new ObservableCollection<TraineeForCallingViewModel>();
            _callingNames = new List<List<string>>() { new List<string>(), new List<string>(), new List<string>(), new List<string>() };
        }

        public void OnLoadTrainees(List<TraineeModel> trainees)
        {
            foreach (List<string> item in _callingNames)
            {
                item.Clear();
            }
            Trainees.Clear();
            if (trainees != null)
            {
                foreach (TraineeModel item in trainees)
                {
                    Trainees.Add(new TraineeForCallingViewModel(item));
                    Trainees.Last().CallingStateChangedEvent += OnCallingStateChanged;
                }
            }
        }

        public void LoadTraineesWithState(List<TraineeModel> trainees)
        {
            foreach (List<string> item in _callingNames)
            {
                item.Clear();
            }
            Trainees.Clear();
            if (trainees != null)
            {
                foreach (TraineeModel item in trainees)
                {
                    Trainees.Add(new TraineeForCallingViewModel(item, item.CallingState));
                    Trainees.Last().CallingStateChangedEvent += OnCallingStateChanged;
                    _callingNames[item.CallingState].Add(item.TraineeID);
                }
            }
        }

        private void OnCallingStateChanged(int currentCallingState, int previousCallingState, string traineeID)
        {
            if (previousCallingState != -1)
            {
                _callingNames[previousCallingState].Remove(traineeID);
            }
            _callingNames[currentCallingState].Add(traineeID);
        }

        public void SetAllPresence()
        {
            if (Trainees.Count > 0)
            {
                foreach (TraineeForCallingViewModel item in Trainees)
                {
                    item.CallingState = (int)CallingState.Presence;
                }
            }
        }

        public void GetCallingNames(ref NameCallingModel model)
        {
            model.PresenceTrainees = _callingNames[(int)CallingState.Presence];
            model.LeaveTrainees = _callingNames[(int)CallingState.Leave];
            model.AbsenceTrainees = _callingNames[(int)CallingState.Absence];
            model.GivingTrainees = _callingNames[(int)CallingState.Giving];
        }

        public void ClearAll()
        {
            foreach (List<string> item in _callingNames)
            {
                item.Clear();
            }
            Trainees.Clear();
        }

        public bool CheckValidity()
        {
            if (Trainees.Where(t => t.CallingState == -1).FirstOrDefault() == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
