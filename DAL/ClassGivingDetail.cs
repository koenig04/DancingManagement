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
    public class ClassGivingDetailDAL
    {
        public bool Add(ClassGivingDetailModel model, out string givingID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassGivingID", SqlDbType.VarChar,50),
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@GivingDate", SqlDbType.Date),
                    new SqlParameter("@ID",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = model.ClassGivingID;
            parameters[1].Value = model.TraineeID;
            parameters[2].Value = model.GivingDate;
            parameters[3].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("ClassGivingDetail_ADD_LK", parameters, out rowsAffected);

            givingID = parameters[3].Value.ToString();
            return true;
        }

        public List<ClassGivingDetailModel> GetList(string givingID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@GivingID", SqlDbType.VarChar,50) };
            parameters[0].Value = givingID;

            DataSet ds = DbHelperSQL.RunProcedure("ClassGivingDetail_GetList_LK", parameters, "ds");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new ClassGivingDetailModel()
                        {
                            GivingDetailID = d.Field<string>("GivingDetailID"),
                            ClassGivingID = d.Field<string>("ClassGivingID"),
                            TraineeID = d.Field<string>("TraineeID"),
                            GivingDate = d.Field<DateTime>("GivingDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<DateTime> GetListAsGivingInfo(string traineeID, ClassGivingInfoModel givingInfo)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate", SqlDbType.Date),
                    new SqlParameter("@EndDate", SqlDbType.Date),
                    new SqlParameter("@EndDateEnable",SqlDbType.Bit)};
            parameters[0].Value = traineeID;
            parameters[1].Value = givingInfo.ClassID;
            parameters[2].Value = givingInfo.StartDate;
            parameters[3].Value = givingInfo.EndDate;
            parameters[4].Value = givingInfo.EndDateEnable;

            DataSet ds = DbHelperSQL.RunProcedure("NameCallingInfo_GetList_ByTraineeAndClass_LK", parameters, "ds");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select d.Field<DateTime>("ClassDate")).ToList();
            }
            else
            {
                return null;
            }
        }

        public void Del(string givingID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@GivingID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = givingID;;

            DbHelperSQL.RunProcedure("ClassGivingDetail_Del_LK", parameters, out rowsAffected);
        }
    }
}
