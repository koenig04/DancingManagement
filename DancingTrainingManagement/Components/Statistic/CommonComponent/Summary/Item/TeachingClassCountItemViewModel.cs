using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Item
{
    class TeachingClassCountItemViewModel : SummaryItemViewModel
    {
        private DelegateCommand _clicked;

        public DelegateCommand ItemClicked
        {
            get
            {
                _clicked = _clicked ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        SummaryItemClickedEvent?.Invoke(_teacherID, SummaryItemColor);
                        ChangeSelectState(true);
                    }));
                return _clicked;
            }
            set
            {
                _clicked = value;
                RaisePropertyChanged("ItemClicked");
            }
        }

        public string TeacherID
        {
            get
            {
                return _teacherID;
            }
        }

        public delegate void SummaryItemClicked(string teacherID, Color itemColor);
        public event SummaryItemClicked SummaryItemClickedEvent;

        private string _teacherID;

        public TeachingClassCountItemViewModel(string itemName, int classCount, string teacherID, Color color)
            : base(itemName, color)
        {
            ItemAmount = classCount.ToString();
            _teacherID = teacherID;
        }
    }
}
