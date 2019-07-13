using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.UserSettingManagement
{
    public class ChangPasswordBussiness
    {
        private string _userID;
        private UserInfo _dal;

        public ChangPasswordBussiness(string userID, UserInfo dal)
        {
            _userID = userID;
            _dal = dal;
        }

        public bool ChangePassword(string oldpassword, string newpassword, string newpasswordagain, out MessageType msg)
        {
            if (!newpassword.Equals(newpasswordagain))
            {
                msg = MessageType.NewPWNotSameErr;
                return false;
            }
            if (_dal.ChangePassword(_userID, oldpassword, newpassword))
            {
                msg = MessageType.ChangePWSuccess;
                return true;
            }
            else
            {
                msg = MessageType.OldPWErr;
                return false;
            }
        }
    }
}
