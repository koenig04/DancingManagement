using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class OverdueInfo
    {
        public List<OverdueModel> GetList()
        {
            DataSet ds = DbHelperSQL.RunProcedure("OverdueInfo_GetList_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new OverdueModel()
                        {
                            OverdueID = d.Field<string>("OverdueID"),
                            TraineeID = d.Field<string>("TraineeID"),
                            TraineeName = d.Field<string>("TraineeName"),
                            ClassID = d.Field<string>("ClassID"),
                            RenewAmount = d.Field<decimal>("RenewAmount"),
                            ClassType = d.Field<int>("ClassType"),
                            OverdueDate = d.Field<DateTime>("OverdueDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<OverdueModel> GetListbyTrainee(string traineeID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = traineeID;
            DataSet ds = DbHelperSQL.RunProcedure("OverdueInfo_GetList_ByTrainee_LK", parameters, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new OverdueModel()
                        {
                            TraineeID = d.Field<string>("TraineeID"),
                            ClassID = d.Field<string>("ClassID"),
                            OverdueDate = d.Field<DateTime>("OverdueDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
