using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Model;
using DancingTrainingManagement.UICore;
using BLL.ClassManagement;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.ClassList
{
    class RegularClassListViewModel : ClassListViewModel
    {
        private DelegateCommand _addClass;
        public DelegateCommand AddClass
        {
            get
            {
                _addClass = _addClass ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        _bussiness.RaiseChangeRegularClassEvent(OperationType.Add, null);
                    }));
                return _addClass;
            }
            set
            {
                _addClass = value;
                RaisePropertyChanged("AddClass");
            }
        }

        private RegularClassManagement _bussiness;
        public RegularClassListViewModel(RegularClassManagement bussiness) : base()
        {
            _bussiness = bussiness;
            if (_bussiness.RegularClassCollection != null)
                _bussiness.RegularClassCollection.ForEach(c =>
                {
                    Classes.Add(new ClassViewModel(c));
                    Classes.Last().ClassOperatedEvent += OnClassOperated;
                });
            _bussiness.RegularClassChangedEvent += OnRegularClassChanged;
        }

        private void OnRegularClassChanged(OperationType operation, RegularClassModel model, int newIndex)
        {
            if (operation == OperationType.Add || operation == OperationType.Update)
            {
                if (operation == OperationType.Update)
                {
                    Classes.Remove(Classes.Where(c => c.ClassID == model.ClassID).First());
                }
                Classes.Insert(newIndex, new ClassViewModel(model));
                Classes[newIndex].ClassOperatedEvent += OnClassOperated;
            }
        }

        private void OnClassOperated(OperationType operation, ClassModel model)
        {
            if (operation == OperationType.Select)
            {
                foreach (ClassViewModel item in Classes.Where(c => c.ClassID != model.ClassID))
                {
                    item.ChangeToUnselected();
                }
            }
            _bussiness.RaiseChangeRegularClassEvent(operation, (model as RegularClassModel));
        }
    }
}
