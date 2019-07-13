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
    public class TraineeInfo
    {
        public bool Add(TraineeModel model, out string traineeID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeName", SqlDbType.VarChar,20),
                    new SqlParameter("@RegularClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@RemainRegularCount", SqlDbType.Int),
                    new SqlParameter("@InitialRemainRegularCount",SqlDbType.Int),
                    new SqlParameter("@ID",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = model.TraineeName;
            parameters[1].Value = model.RegularClassID;
            parameters[2].Value = model.RemainRegularCount;
            parameters[3].Value = model.InitialRemainRegularCount;
            parameters[4].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("TraineeInfo_ADD_LK", parameters, out rowsAffected);

            traineeID = parameters[4].Value.ToString();
            return true;
        }

        public List<TraineeModel> GetList()
        {
            DataSet ds = DbHelperSQL.RunProcedure("Trainee_GetList_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new TraineeModel()
                        {
                            TraineeID = d.Field<string>("TraineeID"),
                            TraineeName = d.Field<string>("TraineeName"),
                            RegularClassID = d.Field<string>("RegularClassID"),
                            RegularClassName = string.IsNullOrEmpty(d.Field<string>("RegularClassID")) ?
                                                "" :
                                                RegularClassType.Instance.RegularClassTypeCollection.Where(r => r.Level == d.Field<int>("ClassType")).First().Name
                                                + d.Field<int>("ClassSerial").ToString()
                                                + "班",
                            RemainRegularCount = d.Field<int>("RemainRegularCount"),
                            InitialRemainRegularCount = d.Field<int>("InitialRemainRegularCount"),
                            State = d.Field<int>("State"),
                            CreateDate = d.Field<DateTime>("CreateDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<TraineeModel> GetTraineesForRegular(string classID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50)};
            parameters[0].Value = classID;

            DataSet ds = DbHelperSQL.RunProcedure("RegularClassInfo_GetTrainee_LK", parameters, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new TraineeModel()
                        {
                            TraineeID = d.Field<string>("TraineeID"),
                            TraineeName = d.Field<string>("TraineeName"),
                            RegularClassID = d.Field<string>("RegularClassID"),
                            RegularClassName = RegularClassType.Instance.RegularClassTypeCollection.Where(r => r.Level == d.Field<int>("ClassType")).First().Name
                                                + d.Field<int>("ClassSerial").ToString()
                                                + "班",
                            RemainRegularCount = d.Field<int>("RemainRegularCount"),
                            InitialRemainRegularCount = d.Field<int>("InitialRemainRegularCount"),
                            State = d.Field<int>("State"),
                            CreateDate = d.Field<DateTime>("CreateDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<TraineeModel> GetTraineesForRegularCallingList(string classID)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50)};
            parameters[0].Value = classID;

            DataSet ds = DbHelperSQL.RunProcedure("RegularClassInfo_GetTrainee_ByCallingList_LK", parameters, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new TraineeModel()
                        {
                            TraineeID = d.Field<string>("TraineeID"),
                            TraineeName = d.Field<string>("TraineeName"),
                            RemainRegularCount = d.Field<int>("RemainRegularCount"),
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<TraineeModel> GetTraineesForBlock(string classID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50)};
            parameters[0].Value = classID;

            DataSet ds = DbHelperSQL.RunProcedure("BlockClassInfo_GetTrainee_LK", parameters, "ds");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new TraineeModel()
                        {
                            TraineeID = d.Field<string>("TraineeID"),
                            TraineeName = d.Field<string>("TraineeName"),
                            State = d.Field<int>("State"),
                            CreateDate = d.Field<DateTime>("CreateDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public bool Update(TraineeModel model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@TraineeName", SqlDbType.VarChar,50),
                    new SqlParameter("@RegularClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@InitialRemainRegularCount",SqlDbType.Int),
                    new SqlParameter("@State",SqlDbType.Int)
                                        };
            parameters[0].Value = model.TraineeID;
            parameters[1].Value = model.TraineeName;
            parameters[2].Value = model.RegularClassID;
            parameters[3].Value = model.InitialRemainRegularCount;
            parameters[4].Value = model.State;

            DbHelperSQL.RunProcedure("TraineeInfo_Update_Regular_LK", parameters, out rowsAffected);
            return true;
        }

        public bool UpdateForBlock(TraineeModel model, string oldBlockID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@TraineeName", SqlDbType.VarChar,50),
                    new SqlParameter("@NewBlockID", SqlDbType.VarChar,50),
                    new SqlParameter("@OldBlockID",SqlDbType.VarChar,50),
                                        };
            parameters[0].Value = model.TraineeID;
            parameters[1].Value = model.TraineeName;
            parameters[2].Value = model.CurrentBlockID;
            parameters[3].Value = oldBlockID;

            DbHelperSQL.RunProcedure("TraineeInfo_Update_Block_LK", parameters, out rowsAffected);
            return true;
        }

        /// <summary>
        /// 将学员从独舞班移除（学员依然存在于系统中）
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="traineeID"></param>
        public void DeleteTraineeForBlock(string classID, string traineeID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = classID;
            parameters[1].Value = traineeID + "|";

            DbHelperSQL.RunProcedure("BlockClassInfo_DELETE_Trainee_LK", parameters, out rowsAffected);
        }

        /// <summary>
        /// 将系统中的学员添加入独舞班（不是新建学员）
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="traineeID"></param>
        public void AddTraineeForBlock(string classID, string traineeID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = classID;
            parameters[1].Value = traineeID + "|";

            DbHelperSQL.RunProcedure("BlockClassInfo_ADD_Trainee_LK", parameters, out rowsAffected);
        }

        /// <summary>
        /// 将系统中的学员添加入常规班（不是新建学员）
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="traineeID"></param>
        public void AddTraineeForRegular(string classID, string traineeID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = classID;
            parameters[1].Value = traineeID;

            DbHelperSQL.RunProcedure("RegularClassInfo_ADD_Trainee_LK", parameters, out rowsAffected);
        }

        public List<string> GetBlockTraineeInSameYearAndSeason(string blockID)
        {
            List<string> res = new List<string>();
            SqlParameter[] parameters = {
                    new SqlParameter("@BlockID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = blockID;

            DataSet ds = DbHelperSQL.RunProcedure("BlockClassInfo_GetExistTrainee_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (string item in (from d in ds.Tables[0].AsEnumerable()
                                         select d.Field<string>("Trainees")).ToList())
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        res.AddRange(item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
                    }
                }
            }

            return res;
        }

        public List<string> GetListInPayment(string classID)
        {
            List<string> res = new List<string>();
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = classID;

            DataSet ds = DbHelperSQL.RunProcedure("ClassPaymentInfo_GetListByClassID_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select d.Field<string>("TraineeID")).ToList();
            }
            else
            {
                return null;
            }
        }

        public int GetCurrentTraineeCount()
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TraineeCount", SqlDbType.Int)
                                        };
            parameters[0].Direction = ParameterDirection.Output;

            int rowsAffected;
            DbHelperSQL.RunProcedure("TraineeInfo_GetTraineeCount_LK", parameters, out rowsAffected);
            return int.Parse(parameters[0].Value.ToString());
        }
    }
}
