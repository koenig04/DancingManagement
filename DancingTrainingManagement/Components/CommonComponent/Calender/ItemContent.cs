using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingTrainingManagement.Components.CommonComponent.Calender
{
    class ItemContent : ViewModelBase
    {
        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged("Content");
            }
        }

        private int _contentHeight;

        public int ContentHeight
        {
            get { return _contentHeight; }
            set
            {
                _contentHeight = value;
                RaisePropertyChanged("ContentHeight");
            }
        }


        public ItemContent(string content)
        {
            Content = content;
            ContentHeight = 20;
        }
    }
}
