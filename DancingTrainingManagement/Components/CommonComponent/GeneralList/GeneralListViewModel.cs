using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DancingTrainingManagement.Components.CommonComponent.GeneralList
{
    class GeneralListViewModel : ViewModelBase
    {
        private string _listName;

        public string ListName
        {
            get { return _listName; }
            set
            {
                _listName = value;
                RaisePropertyChanged("ListName");
            }
        }

        private ObservableCollection<GeneralListItemViewModel> _items;

        public ObservableCollection<GeneralListItemViewModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged("Items");
            }
        }

        private DelegateCommand _addItem;
        public DelegateCommand AddItem
        {
            get
            {
                _addItem = _addItem ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        ItemOperatedEvent.Invoke(OperationType.Add, string.Empty);
                    }));
                return _addItem;
            }
            set
            {
                _addItem = value;
                RaisePropertyChanged("AddItem");
            }
        }

        public delegate void ItemOperated(OperationType operation, string itemID);
        public event ItemOperated ItemOperatedEvent;

        public GeneralListViewModel(string listName, List<GeneralListItemViewModel> items)
        {
            ListName = listName;
            Items = new ObservableCollection<GeneralListItemViewModel>();
            foreach (GeneralListItemViewModel item in items)
            {
                Items.Add(item);
                item.ItemOperatedEvent += (operate, itemID) => ItemOperatedEvent?.Invoke(operate, itemID);
            }
            Vis = Visibility.Hidden;
        }
    }
}
