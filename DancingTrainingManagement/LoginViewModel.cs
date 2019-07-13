using BLL.Login;
using DancingTrainingManagement.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DancingTrainingManagement
{
    class LoginViewModel : ViewModelBase
    {
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        private Visibility _errVis = Visibility.Hidden;

        public Visibility ErrVis
        {
            get { return _errVis; }
            set
            {
                _errVis = value;
                RaisePropertyChanged("ErrVis");
            }
        }

        private Visibility _loginVis = Visibility.Visible;

        public Visibility LoginVis
        {
            get { return _loginVis; }
            set
            {
                _loginVis = value;
                RaisePropertyChanged("LoginVis");
            }
        }


        private DelegateCommand _login;

        public DelegateCommand Login
        {
            get
            {
                _login = _login ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        _bussiness.Login(UserName, Password);
                    }));
                return _login;
            }
            set
            {
                _login = value;
                RaisePropertyChanged("Login");
            }
        }

        private DelegateCommand _quit;

        public DelegateCommand Quit
        {
            get
            {
                _quit = _quit ?? new DelegateCommand(new Action<object>(
                    o =>
                    {
                        System.Windows.Application.Current.Shutdown();
                    }));
                return _quit;
            }
            set
            {
                _quit = value;
                RaisePropertyChanged("Quit");
            }
        }

        private bool _loginEnable = true;

        public bool LoginEnable
        {
            get { return _loginEnable; }
            set
            {
                _loginEnable = value;
                RaisePropertyChanged("LoginEnable");
            }
        }


        private LoginBussiness _bussiness;

        public LoginViewModel()
        {
            _bussiness = new LoginBussiness();
            _bussiness.LoginInfoChangedEvent += OnLoginInfoChanged;
        }

        private void OnLoginInfoChanged(bool isSuccess, string userID)
        {
            if (isSuccess)
            {
                var mainWindow = new MainWindow(userID);
                mainWindow.Show();
                LoginEnable = false;
            }
            else
            {
                ErrVis = Visibility.Visible;
            }
        }
    }
}
