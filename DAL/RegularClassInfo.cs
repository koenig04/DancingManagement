using Common;
using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RegularClassInfo
    {
        public bool Add(RegularClassModel model, out string classID, out int classSerial)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@CostPerTerm", SqlDbType.Decimal,9),
                    new SqlParameter("@ClassType", SqlDbType.Int),
                    new SqlParameter("@ClassSerial", SqlDbType.Int),
                    new SqlParameter("@ID",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = model.TeacherID;
            parameters[1].Value = model.CostPerTerm;
            parameters[2].Value = model.ClassType;
            parameters[3].Direction = ParameterDirection.Output;
            parameters[4].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("RegularClassInfo_ADD_LK", parameters, out rowsAffected);
            classSerial = int.Parse(parameters[3].Value.ToString());
            classID = parameters[4].Value.ToString();
            //return rowsAffected > 0 ? true : false;
            return true;
        }

        public bool Update(RegularClassModel model, out int classSerial)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@CostPerTerm", SqlDbType.Decimal,9),
                    new SqlParameter("@ClassType", SqlDbType.Int),
                    new SqlParameter("@ClassSerial", SqlDbType.Int)
                                        };
            parameters[0].Value = model.ClassID;
            parameters[1].Value = model.TeacherID;
            parameters[2].Value = model.CostPerTerm;
            parameters[3].Value = model.ClassType;
            parameters[4].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("RegularClassInfo_Update_LK", parameters, out rowsAffected);
            classSerial = int.Parse(parameters[4].Value.ToString());
            return true;
        }

        public RegularClassModel GetModel(string classID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50)};
            parameters[0].Value = classID;

            DataSet ds = DbHelperSQL.RunProcedure("RegularClassInfo_GetModel_LK", parameters, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return new Model.RegularClassModel()
                {
                    ClassID = ds.Tables[0].Rows[0]["ClassID"].ToString(),
                    ClassSerial = int.Parse(ds.Tables[0].Rows[0]["int.Parse"].ToString()),
                    TeacherID = ds.Tables[0].Rows[0]["TeacherID"].ToString(),
                    TeacherName = ds.Tables[0].Rows[0]["TeacherName"].ToString(),
                    CostPerTerm = decimal.Parse(ds.Tables[0].Rows[0]["CostPerTerm"].ToString()),
                    ClassType = int.Parse(ds.Tables[0].Rows[0]["ClassType"].ToString())
                };
            }
            else
            {
                return null;
            }
        }

        public List<RegularClassModel> GetList()
        {
            DataSet ds = DbHelperSQL.RunProcedure("RegularClassInfo_GetList_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new RegularClassModel()
                        {
                            ClassID = d.Field<string>("ClassID"),
                            TeacherID = d.Field<string>("TeacherID"),
                            TeacherName = d.Field<string>("TeacherName"),
                            CostPerTerm = d.Field<decimal>("CostPerTerm"),
                            ClassType = d.Field<int>("ClassType"),
                            ClassSerial = d.Field<int>("ClassSerial")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<RegularChangingModel> GetChangingList()
        {
            DataSet ds = DbHelperSQL.RunProcedure("RegularChangingInfo_GetList_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new RegularChangingModel()
                        {
                            RegularChangingID = d.Field<string>("RegularChangingID"),
                            ChangingDate = d.Field<DateTime>("ChangingDate"),
                            RegularClassID = d.Field<string>("RegularClassID"),
                            PreviousClassType = d.Field<int>("PreviousClassType"),
                            PreviousClassSerial = d.Field<int>("PreviousClassSerial"),
                            PostClassType = d.Field<int>("PostClassType"),
                            PostClassSerial = d.Field<int>("PostClassSerial")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
