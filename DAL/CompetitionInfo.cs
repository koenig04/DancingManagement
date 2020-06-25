using DBUtility;
using Model.Competition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class CompetitionInfo
    {
        public List<CompetitionInfoModel> GetList()
        {
            DataSet ds = DbHelperSQL.RunProcedure("CompetitionInfo_GetList_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new CompetitionInfoModel()
                        {
                            CompetitionID = d.Field<string>("CompetitionID"),
                            CompetitionName = d.Field<string>("CompetitionName"),
                            CompetitionYear = d.Field<int>("CompetitionYear"),
                            CompetitionTraineeCount = d.Field<int>("CompetitionTraineeCount"),
                            CompetitionFee = d.Field<decimal>("CompetitionFee"),
                            CompetitionActualFee = d.Field<decimal>("CompetitionActualFee"),
                            IsRecorded = d.Field<bool>("IsRecorded")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public CompetitionInfoModel GetModel(string competitionID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50)};
            parameters[0].Value = competitionID;

            DataSet ds = DbHelperSQL.RunProcedure("CompetitionInfo_GetModel_LK", parameters, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return new CompetitionInfoModel()
                {
                    CompetitionID = ds.Tables[0].Rows[0]["CompetitionID"].ToString(),
                    CompetitionName = ds.Tables[0].Rows[0]["CompetitionName"].ToString(),
                    CompetitionYear = int.Parse(ds.Tables[0].Rows[0]["CompetitionYear"].ToString()),
                    CompetitionTraineeCount = int.Parse(ds.Tables[0].Rows[0]["CompetitionTraineeCount"].ToString()),
                    CompetitionFee = decimal.Parse(ds.Tables[0].Rows[0]["CompetitionFee"].ToString()),
                    CompetitionActualFee = decimal.Parse(ds.Tables[0].Rows[0]["CompetitionActualFee"].ToString()),
                    IsRecorded = bool.Parse(ds.Tables[0].Rows[0]["IsRecorded"].ToString())
                };
            }
            else
            {
                return null;
            }
        }
    }
}