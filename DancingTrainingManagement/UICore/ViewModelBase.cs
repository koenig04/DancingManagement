using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DancingTrainingManagement.UICore
{
    public class ViewModelBase : NotificationObject
    {
        private Visibility _vis;

        public Visibility Vis
        {
            get { return _vis; }
            set
            {
                _vis = value;
                RaisePropertyChanged("Vis");
            }
        }

        private bool _canEdit=true;

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                RaisePropertyChanged("CanEdit");
            }
        }

        public void OnOperateEnableEvent(bool isEnable, bool isShown)
        {
            CanEdit = isEnable;
            Vis = isShown ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
