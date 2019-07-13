using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.UserSettingManagement
{
    public class UserSettingManagementBussiness
    {
        public ChangPasswordBussiness ChangePassword { get; private set; }

        public UserSettingManagementBussiness(string userID)
        {
            UserInfo dal = new UserInfo();
            ChangePassword = new ChangPasswordBussiness(userID, dal);
        }
    }
}
