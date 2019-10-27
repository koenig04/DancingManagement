using Common;
using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DBRelated
    {
        public DBRelated()
        {
            
        }

        public bool ExportDB(string dbName)
        {
            try
            {
                string sqlStr = "BACKUP DATABASE " + "DancingTraining" + " TO DISK = '" + dbName + "' ";

                DbHelperSQL.ExecuteSql(sqlStr);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
