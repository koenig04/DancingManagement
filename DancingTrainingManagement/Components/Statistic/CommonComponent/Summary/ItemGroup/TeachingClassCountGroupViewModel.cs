using Common;
using DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.Item;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.Statistic.CommonComponent.Summary.ItemGroup
{
    class TeachingClassCountGroupViewModel : SummaryItemGroupViewModel
    {
        private ObservableCollection<TeachingClassCountItemViewModel> _items;

        public ObservableCollection<TeachingClassCountItemViewModel> SummaryItems
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged("SummaryItems");
            }
        }

        public delegate void SummaryItemClicked(string teacherID, Color itemColor, DateTime summaryDate, bool isTotal);
        public event SummaryItemClicked SummaryItemClickedEvent;

        public TeachingClassCountGroupViewModel(DateTime summaryDate, bool isSortedByMonth) : base(summaryDate, isSortedByMonth)
        {
            SummaryItems = new ObservableCollection<TeachingClassCountItemViewModel>();
        }

        public TeachingClassCountGroupViewModel() : base()
        {
            SummaryItems = new ObservableCollection<TeachingClassCountItemViewModel>();            
        }

        public void AddSummaryItem(string itemName, int classCount, string teacherID, Color color)
        {
            SummaryItems.Add(new TeachingClassCountItemViewModel(itemName, classCount, teacherID, color));
            SummaryItems.Last().SummaryItemClickedEvent += (id, itemColor) =>
            {
                foreach (TeachingClassCountItemViewModel item in SummaryItems.Where(i => i.TeacherID != id))
                {
                    item.ChangeSelectState(false);
                }
                SummaryItemClickedEvent?.Invoke(id, itemColor, SummaryDate, IsTotal);
            };
            ChangeGroupWidth(SummaryItems.Count);
        }

        public void ChangeGroupState(bool state)
        {
            foreach (TeachingClassCountItemViewModel item in SummaryItems)
            {
                item.ChangeSelectState(state);
            }
        }

        public override void ClearSummary()
        {
            SummaryItems.Clear();
        }
    }
}
