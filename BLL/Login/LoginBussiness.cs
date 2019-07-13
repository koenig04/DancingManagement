using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Login
{
    public class LoginBussiness
    {
        public delegate void LoginInfoChanged(bool isSuccess, string userID);
        public event LoginInfoChanged LoginInfoChangedEvent;
        
        private UserInfo _dal;
        private List<UserModel> users;
        public LoginBussiness()
        {
            _dal = new UserInfo();
            users = _dal.GetUsers();
        }

        public void Login(string userName, string password)
        {
            if (CheckLoginInput(userName, password))
                LoginInfoChangedEvent?.Invoke(true, users.Where(u => u.LoginName == userName).First().UserID);
            else
                LoginInfoChangedEvent?.Invoke(false, null);
        }

        private bool CheckLoginInput(string userName, string password)
        {
            if (users.Exists(u => u.LoginName == userName) && users.Where(u => u.LoginName == userName).First().PassWord == password)
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
