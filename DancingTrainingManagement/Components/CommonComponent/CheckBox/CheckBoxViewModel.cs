using Common;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DancingTrainingManagement.Components.CommonComponent.CheckBox
{
    class CheckBoxViewModel : ViewModelBase
    {
        private bool isEnable_;

        public bool Enable
        {
            get { return isEnable_; }
            set { isEnable_ = value; RaisePropertyChanged("Enable"); }
        }

        private Color bkg_;

        public Color Bkg
        {
            get { return bkg_; }
            set { bkg_ = value; RaisePropertyChanged("Bkg"); }
        }

        private string content_;

        public string Content
        {
            get { return content_; }
            set { content_ = value; RaisePropertyChanged("Content"); }
        }

        private DelegateCommand check_;

        public DelegateCommand Check
        {
            get
            {
                check_ = check_ ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        isChecked_ = !isChecked_;
                        Bkg = isChecked_ ? GlobalVariables.MainColor : GlobalVariables.MainBackColor;
                        CheckboxStatusChangedEvent?.Invoke(isChecked_);
                    }));
                return check_;
            }
            set { check_ = value; RaisePropertyChanged("Check"); }
        }


        public delegate void CheckboxStatusChanged(bool isChecked);
        public event CheckboxStatusChanged CheckboxStatusChangedEvent;

        private bool isChecked_;

        public CheckBoxViewModel()
        {
            Bkg = GlobalVariables.MainBackColor;
            isChecked_ = false;

        }

        public void resetStatus()
        {
            if (isChecked_)
                Check.Execute(null);
        }
    }
}
