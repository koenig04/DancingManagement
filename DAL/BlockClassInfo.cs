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
    public class BlockClassInfo
    {
        public bool Add(BlockClassModel model, out string classID, out int classSerial)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@CostPerTerm", SqlDbType.Decimal,9),
                    new SqlParameter("@ClassYear", SqlDbType.Int),
                    new SqlParameter("@ClassSeason",SqlDbType.Int),
                    new SqlParameter("@ClassType", SqlDbType.Int),
                    new SqlParameter("@ClassSerial", SqlDbType.Int),
                    new SqlParameter("@ID",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = model.TeacherID;
            parameters[1].Value = model.CostPerTerm;
            parameters[2].Value = model.ClassYear;
            parameters[3].Value = model.ClassSeason;
            parameters[4].Value = model.ClassType;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[6].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("BlockClassInfo_ADD_LK", parameters, out rowsAffected);
            classSerial = int.Parse(parameters[5].Value.ToString());
            classID = parameters[6].Value.ToString();
            //return rowsAffected > 0 ? true : false;
            return true;
        }

        public bool Update(BlockClassModel model, out int classSerial)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@CostPerTerm", SqlDbType.Decimal,9),
                    new SqlParameter("@ClassYear", SqlDbType.Int),
                    new SqlParameter("@ClassSeason",SqlDbType.Int),
                    new SqlParameter("@ClassType", SqlDbType.Int),
                    new SqlParameter("@ClassSerial", SqlDbType.Int)
                                        };
            parameters[0].Value = model.ClassID;
            parameters[1].Value = model.TeacherID;
            parameters[2].Value = model.CostPerTerm;
            parameters[3].Value = model.ClassYear;
            parameters[4].Value = model.ClassSeason;
            parameters[5].Value = model.ClassType;
            parameters[6].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("BlockCLassInfo_UPDATE_LK", parameters, out rowsAffected);
            classSerial = int.Parse(parameters[6].Value.ToString());
            return true;
        }

        public List<BlockClassModel> GetList()
        {
            DataSet ds = DbHelperSQL.RunProcedure("BlockClassInfo_GetList_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new BlockClassModel()
                        {
                            ClassID = d.Field<string>("ClassID"),
                            TeacherID = d.Field<string>("TeacherID"),
                            TeacherName = d.Field<string>("TeacherName"),
                            CostPerTerm = d.Field<decimal>("CostPerTerm"),
                            ClassType = d.Field<int>("ClassType"),
                            ClassSerial = d.Field<int>("ClassSerial"),
                            ClassYear = d.Field<int>("ClassYear"),
                            ClassSeason=d.Field<int>("ClassSeason")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<BlockClassModel> GetListByTrainee(string traineeID)
        {            
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = traineeID;
            DataSet ds = DbHelperSQL.RunProcedure("BlockClassInfo_GetClass_ByTrainee_LK", parameters, "ds");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new BlockClassModel()
                        {
                            ClassID = d.Field<string>("ClassID"),
                            ClassType = d.Field<int>("ClassType"),
                            ClassSerial = d.Field<int>("ClassSerial"),
                            ClassYear = d.Field<int>("ClassYear"),
                            ClassSeason = d.Field<int>("ClassSeason")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
