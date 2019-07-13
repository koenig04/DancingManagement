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
    public class GeneralInfo
    {
        public List<CapitalModel> GetCapitals(DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@StartDate", SqlDbType.Date),
                    new SqlParameter("@EndDate", SqlDbType.Date)
                                        };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            DataSet ds = DbHelperSQL.RunProcedure("GeneralInfo_GetCapitals_LK", parameters, "ds");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new CapitalModel()
                        {
                            Capital = d.Field<decimal>("Capital"),
                            GeneralYear = d.Field<int>("GeneralYear"),
                            GeneralMonth = d.Field<int>("GeneralMonth")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public decimal GetCurrentCapital()
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@CurrentCapital", SqlDbType.Decimal)
                                        };
            parameters[0].Direction = ParameterDirection.Output;

            int rowsAffected;
            DbHelperSQL.RunProcedure("GeneralInfo_GetCurrentCapital", parameters, out rowsAffected);
            return decimal.Parse(parameters[0].Value.ToString());
        }
    }
}
