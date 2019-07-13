using Common;
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
    public class UserInfo
    {
        public List<TeacherModel> GetTeachers()
        {
            DataSet ds = DbHelperSQL.RunProcedure("UserInfo_GetTeachers_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new TeacherModel()
                        {
                            TeacherID = d.Field<string>("UserID"),
                            TeacherName = d.Field<string>("UserName"),
                            TeacherFee = d.Field<decimal>("TeacherFee")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<UserModel> GetUsers()
        {
            DataSet ds = DbHelperSQL.RunProcedure("UserInfo_GetList_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new UserModel()
                        {
                            UserID = d.Field<string>("UserID"),
                            LoginName = d.Field<string>("LoginName"),
                            PassWord = d.Field<string>("PassWord")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public bool AddTeacher(TeacherModel teacher, out string teacherID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TeacherName", SqlDbType.VarChar,20),
                    new SqlParameter("@TeacherFee", SqlDbType.Decimal,9),
                    new SqlParameter("@ID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = teacher.TeacherName;
            parameters[1].Value = teacher.TeacherFee;
            parameters[2].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("UserInfo_AddTeacher_LK", parameters, out rowsAffected);
            teacherID = parameters[2].Value.ToString();
            return true;
        }

        public void UpdateTeacher(TeacherModel teacher)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@TeacherName", SqlDbType.VarChar,20),
                    new SqlParameter("@TeacherFee", SqlDbType.Decimal,9)
                                        };
            parameters[0].Value = teacher.TeacherID;
            parameters[1].Value = teacher.TeacherName;
            parameters[2].Value = teacher.TeacherFee;

            DbHelperSQL.RunProcedure("UserInfo_UpdateTeacher_LK", parameters, out rowsAffected);
        }

        public bool ChangePassword(string userID, string oldpassword, string newpassword)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.VarChar,50),
                    new SqlParameter("@OldPassWord", SqlDbType.VarChar,10),
                    new SqlParameter("@NewPassWord", SqlDbType.VarChar,10),
                    new SqlParameter("@IsSuccess",SqlDbType.Bit)
                                        };
            parameters[0].Value = userID;
            parameters[1].Value = oldpassword;
            parameters[2].Value = newpassword;
            parameters[3].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("UserInfo_ChangePassword_LK", parameters, out rowsAffected);
            if (bool.Parse(parameters[3].Value.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
