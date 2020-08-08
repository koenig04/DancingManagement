using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility
{
    internal class PubConstant
    {
        public static string ConnectionString()
        {
            string connectionString = CommonConfigurations.Instance.Items.ConnectionString;
            return connectionString;
        }
    }
}
