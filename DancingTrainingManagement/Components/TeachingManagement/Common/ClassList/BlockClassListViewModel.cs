using BLL.ClassManagement;
using Common;
using DancingTrainingManagement.UICore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.TeachingManagement.Common.ClassList
{
    class BlockClassListViewModel : ClassListViewModel
    {
        private DelegateCommand _addClass;
        public DelegateCommand AddClass
        {
            get
            {
                _addClass = _addClass ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        _bussiness.RaiseChangeBlockClassEvent(OperationType.Add, null);
                    }));
                return _addClass;
            }
            set
            {
                _addClass = value;
                RaisePropertyChanged("AddClass");
            }
        }

        private BlockClassManagement _bussiness;
        public BlockClassListViewModel(BlockClassManagement bussiness) : base()
        {
            _bussiness = bussiness;
            if (_bussiness.BlockClassCollection != null)
                _bussiness.BlockClassCollection.ForEach(c =>
                {
                    Classes.Add(new ClassViewModel(c));
                    Classes.Last().ClassOperatedEvent += OnClassOperated;
                });
            _bussiness.BlockClassChangedEvent += OnBlockClassChanged;
        }

        private void OnBlockClassChanged(OperationType operation, BlockClassModel model, int newIndex)
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
            _bussiness.RaiseChangeBlockClassEvent(operation, (model as BlockClassModel));
        }
    }
}
