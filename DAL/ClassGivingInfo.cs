using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.DancingClass;
using System.Data;
using System.Data.SqlClient;
using DBUtility;

namespace DAL
{
    public class ClassGivingInfo
    {
        public bool Add(ClassGivingInfoModel model, out string givingID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate", SqlDbType.Date),
                    new SqlParameter("@EndDate", SqlDbType.Date),
                    new SqlParameter("@GivingInterval",SqlDbType.Int),
                    new SqlParameter("@GivingCount",SqlDbType.Int),
                    new SqlParameter("@EndDateEnable",SqlDbType.Bit),
                    new SqlParameter("@ID",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = model.ClassID;
            parameters[1].Value = model.StartDate;
            parameters[2].Value = model.EndDate;
            parameters[3].Value = model.GivingInterval;
            parameters[4].Value = model.GivingCount;
            parameters[5].Value = model.EndDataEnable;
            parameters[6].Direction = ParameterDirection.Output;

            
            DbHelperSQL.RunProcedure("ClassGivingInfo_ADD_LK", parameters, out rowsAffected);

            givingID = parameters[5].Value.ToString();
            return true;
        }
    }
}
