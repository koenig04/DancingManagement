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
    public class ClassGivingInfoDAL
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
            parameters[5].Value = model.EndDateEnable;
            parameters[6].Direction = ParameterDirection.Output;


            DbHelperSQL.RunProcedure("ClassGivingInfo_ADD_LK", parameters, out rowsAffected);

            givingID = parameters[6].Value.ToString();
            return true;
        }

        public List<ClassGivingInfoModel> GetList(string classID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50)};
            parameters[0].Value = classID;

            DataSet ds = DbHelperSQL.RunProcedure("ClassGivingInfo_GetList_LK", parameters, "ds");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new ClassGivingInfoModel()
                        {
                            ClassGivingID = d.Field<string>("ClassGivingID"),
                            ClassID = d.Field<string>("ClassID"),
                            StartDate = d.Field<DateTime>("StartDate"),
                            EndDate = d.Field<DateTime>("EndDate"),
                            GivingInterval = d.Field<int>("GivingInterval"),
                            GivingCount = d.Field<int>("GivingCount"),
                            EndDateEnable = d.Field<bool>("EndDateEnable")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public void Update(ClassGivingInfoModel model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassGivingID", SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate", SqlDbType.Date),
                    new SqlParameter("@EndDate", SqlDbType.Date),
                    new SqlParameter("@GivingInterval",SqlDbType.Int),
                    new SqlParameter("@GivingCount",SqlDbType.Int),
                    new SqlParameter("@EndDateEnable",SqlDbType.Bit)
                                        };
            parameters[0].Value = model.ClassGivingID;
            parameters[1].Value = model.StartDate;
            parameters[2].Value = model.EndDate;
            parameters[3].Value = model.GivingInterval;
            parameters[4].Value = model.GivingCount;
            parameters[5].Value = model.EndDateEnable;

            DbHelperSQL.RunProcedure("ClassGivingInfo_UpDate_LK", parameters, out rowsAffected);
        }

        public void Del(string givingID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassGivingID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = givingID;

            DbHelperSQL.RunProcedure("ClassGivingInfo_Del_LK", parameters, out rowsAffected);

        }
    }
}
