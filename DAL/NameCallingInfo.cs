using Common;
using DBUtility;
using Model;
using Model.DancingClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class NameCallingInfo
    {
        public bool Add(NameCallingModel model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@ClassType", SqlDbType.Int),
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@ClassDate",SqlDbType.Date),
                    new SqlParameter("@Presence", SqlDbType.VarChar,250),
                    new SqlParameter("@Leave", SqlDbType.VarChar,250),
                    new SqlParameter("@Absence",SqlDbType.VarChar,250),
                    new SqlParameter("@Giving",SqlDbType.VarChar,250)
                                        };
            parameters[0].Value = model.ClassID;
            parameters[1].Value = model.ClassType;
            parameters[2].Value = model.TeacherID;
            parameters[3].Value = model.ClassDate;
            parameters[4].Value = model.Presence;
            parameters[5].Value = model.Leave;
            parameters[6].Value = model.Absence;
            parameters[7].Value = model.Giving;

            DbHelperSQL.RunProcedure("NameCallingInfo_ADD_LK", parameters, out rowsAffected);
            return true;
        }

        public int GetClassCountForTeacher(string teacherID, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate",SqlDbType.Date),
                    new SqlParameter("@EndDate",SqlDbType.Date)};
            parameters[0].Value = teacherID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            DataSet ds = DbHelperSQL.RunProcedure("NameCallingInfo_GetClassCountOfTeacher_LK", parameters, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0]["ClassCount"].ToString());
            }
            else
            {
                return 0;
            }
        }

        public List<ClassInfoForTeacherModel> GetClassInfoForTeacher(string teacherID, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate",SqlDbType.Date),
                    new SqlParameter("@EndDate",SqlDbType.Date)};
            parameters[0].Value = teacherID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            DataSet ds = DbHelperSQL.RunProcedure("NameCalling_GetListByTeacher_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new ClassInfoForTeacherModel()
                        {
                            NameCallingID = d.Field<string>("NameCallingID"),
                            ClassID = d.Field<string>("ClassID"),
                            ClassType = d.Field<int>("ClassType"),
                            TeacherID = d.Field<string>("TeacherID"),
                            ClassDate = d.Field<DateTime>("ClassDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<NameCallingModel> GetListByTrainee(string traineeID, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate",SqlDbType.Date),
                    new SqlParameter("@EndDate",SqlDbType.Date)};
            parameters[0].Value = traineeID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            DataSet ds = DbHelperSQL.RunProcedure("NameCallingInfo_GetList_ByTrainee_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new NameCallingModel()
                        {
                            ClassID = d.Field<string>("ClassID"),
                            Presence = d.Field<string>("Presence"),
                            Leave = d.Field<string>("Leave"),
                            Absence = d.Field<string>("Absence"),
                            Giving = d.Field<string>("Giving"),
                            ClassDate = d.Field<DateTime>("ClassDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<NameCallingModel> GetListForCurrentTerm(string traineeID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@TermDuration",SqlDbType.Int)};
            parameters[0].Value = traineeID;
            parameters[1].Value = 20;

            DataSet ds = DbHelperSQL.RunProcedure("NameCallingInfo_GetCurrentTermPresence_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new NameCallingModel()
                        {
                            ClassID = d.Field<string>("ClassID"),
                            Presence = d.Field<string>("Presence"),
                            Leave = d.Field<string>("Leave"),
                            Absence = d.Field<string>("Absence"),
                            Giving = d.Field<string>("Giving"),
                            ClassDate = d.Field<DateTime>("ClassDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<NameCallingModel> GetListForPreviousTerm(string traineeID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@TermDuration",SqlDbType.Int)};
            parameters[0].Value = traineeID;
            parameters[1].Value = Configuration.Instance.Configurations.CountPerTerm;

            DataSet ds = DbHelperSQL.RunProcedure("NameCallingInfo_GetPreviousTermPresence_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new NameCallingModel()
                        {
                            ClassID = d.Field<string>("ClassID"),
                            Presence = d.Field<string>("Presence"),
                            Leave = d.Field<string>("Leave"),
                            Absence = d.Field<string>("Absence"),
                            Giving = d.Field<string>("Giving"),
                            ClassDate = d.Field<DateTime>("ClassDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<NameCallingModel> GetListByDate(DateTime callingDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@CallingDate", SqlDbType.Date)};
            parameters[0].Value = callingDate;

            DataSet ds = DbHelperSQL.RunProcedure("NameCallingInfo_GetList_ByDate_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new NameCallingModel()
                        {
                            NameCallingID = d.Field<string>("NameCallingID"),
                            ClassID = d.Field<string>("ClassID"),
                            ClassType = d.Field<int>("ClassType"),
                            TeacherID = d.Field<string>("TeacherID"),
                            Presence = d.Field<string>("Presence"),
                            Leave = d.Field<string>("Leave"),
                            Absence = d.Field<string>("Absence"),
                            Giving = d.Field<string>("Giving"),
                            ClassDate = d.Field<DateTime>("ClassDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public void Del(string callingID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@CallingID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = callingID;

            DbHelperSQL.RunProcedure("NameCallingInfo_DEL_LK", parameters, out rowsAffected);
        }

        public bool Update(NameCallingModel model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@ClassType", SqlDbType.Int),
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@ClassDate",SqlDbType.Date),
                    new SqlParameter("@Presence", SqlDbType.VarChar,250),
                    new SqlParameter("@Leave", SqlDbType.VarChar,250),
                    new SqlParameter("@Absence",SqlDbType.VarChar,250),
                    new SqlParameter("@Giving",SqlDbType.VarChar,250),
                    new SqlParameter("@CallingID",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = model.ClassID;
            parameters[1].Value = model.ClassType;
            parameters[2].Value = model.TeacherID;
            parameters[3].Value = model.ClassDate;
            parameters[4].Value = model.Presence;
            parameters[5].Value = model.Leave;
            parameters[6].Value = model.Absence;
            parameters[7].Value = model.Giving;
            parameters[8].Value = model.NameCallingID;

            DbHelperSQL.RunProcedure("NameCallingInfo_Update_LK", parameters, out rowsAffected);
            return true;
        }

        public List<NameCallingModel> GetListByClass(string classID, DateTime startDate, DateTime endDate, bool isGeneral)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate",SqlDbType.Date),
                    new SqlParameter("@EndDate",SqlDbType.Date),
                    new SqlParameter("@IsGeneral",SqlDbType.VarChar,50)};
            parameters[0].Value = classID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;
            parameters[3].Value = isGeneral;

            DataSet ds = DbHelperSQL.RunProcedure("NameCalling_GetListByClass_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new NameCallingModel()
                        {
                            NameCallingID = d.Field<string>("NameCallingID"),
                            ClassID = d.Field<string>("ClassID"),
                            ClassType = d.Field<int>("ClassType"),
                            TeacherID = d.Field<string>("TeacherID"),
                            Presence = d.Field<string>("Presence"),
                            Leave = d.Field<string>("Leave"),
                            Absence = d.Field<string>("Absence"),
                            Giving = d.Field<string>("Giving"),
                            ClassDate = d.Field<DateTime>("ClassDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<ClassGivingInfoModel> GetGivingList(string classID)
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

        public TraineeModel GetTraineeModel(string traineeID)
        {
            List<string> res = new List<string>();
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = traineeID;

            DataSet ds = DbHelperSQL.RunProcedure("TraineeInfo_GetModel_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return new TraineeModel()
                {
                    TraineeID = ds.Tables[0].Rows[0]["TraineeID"].ToString(),
                    TraineeName = ds.Tables[0].Rows[0]["TraineeName"].ToString(),
                    RegularClassID = ds.Tables[0].Rows[0]["RegularClassID"].ToString(),
                    RemainRegularCount = int.Parse(ds.Tables[0].Rows[0]["RemainRegularCount"].ToString())
                };
            }
            else
            {
                return null;
            }
        }

        public void UpdateRemainCount(string traineeID, int remainCount)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@RemainCount", SqlDbType.Int)
                                        };
            parameters[0].Value = traineeID;
            parameters[1].Value = remainCount;
            DbHelperSQL.RunProcedure("TraineeInfo_Update_RemainRegular_LK", parameters, out rowsAffected);
        }

        public void ReduceRemaiCountAndOverdue(string traineeID, int remainCount, string classID, DateTime overdueDate)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@RemainCount", SqlDbType.Int),
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@OverdueDate", SqlDbType.Date)
                                        };
            parameters[0].Value = traineeID;
            parameters[1].Value = remainCount;
            parameters[2].Value = classID;
            parameters[3].Value = overdueDate;
            DbHelperSQL.RunProcedure("TraineeInfo_Reduce_RemainRegular_ChangeOverdue_LK", parameters, out rowsAffected);
        }

        public void IncreaseRemainCountAndOverdue(string traineeID, int remainCount)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@RemainCount", SqlDbType.Int)
                                        };
            parameters[0].Value = traineeID;
            parameters[1].Value = remainCount;
            DbHelperSQL.RunProcedure("TraineeInfo_Increase_RemainRegular_ChangeOverdue_LK", parameters, out rowsAffected);
        }

        public bool AddGivingDetail(ClassGivingDetailModel model, out string givingID)
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

        public List<ClassGivingDetailModel> GetGivingDetail(string traineeID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50)};
            parameters[0].Value = traineeID;

            DataSet ds = DbHelperSQL.RunProcedure("ClassGivingDetail_GetListByTrainee_LK", parameters, "ds");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new ClassGivingDetailModel()
                        {
                            ClassGivingID = d.Field<string>("ClassGivingID"),
                            GivingDetailID = d.Field<string>("GivingDetailID"),
                            TraineeID = d.Field<string>("TraineeID"),
                            GivingDate = d.Field<DateTime>("GivingDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<NameCallingModel> GetListByPresentCount(string traineeID, int presentCount)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@PresentCount", SqlDbType.Int)};
            parameters[0].Value = traineeID;
            parameters[1].Value = presentCount;

            DataSet ds = DbHelperSQL.RunProcedure("NameCallingInfo_GetListByPresentCount_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new NameCallingModel()
                        {
                            ClassID = d.Field<string>("ClassID"),
                            Presence = d.Field<string>("Presence"),
                            Leave = d.Field<string>("Leave"),
                            Absence = d.Field<string>("Absence"),
                            Giving = d.Field<string>("Giving"),
                            ClassDate = d.Field<DateTime>("ClassDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<ClassGivingDetailModel> GetGivingDetailByTraineeAndCount(string traineeID, int givingCount)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@GivingCount", SqlDbType.Int)};
            parameters[0].Value = traineeID;
            parameters[1].Value = givingCount;

            DataSet ds = DbHelperSQL.RunProcedure("ClassGivingDetail_GetListByTraineeAndCount_LK", parameters, "ds");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new ClassGivingDetailModel()
                        {
                            ClassGivingID = d.Field<string>("ClassGivingID"),
                            GivingDetailID = d.Field<string>("GivingDetailID"),
                            TraineeID = d.Field<string>("TraineeID"),
                            GivingDate = d.Field<DateTime>("GivingDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
