﻿using DBUtility;
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
            try { 
            DataSet ds= DbHelperSQL.RunProcedure("GeneralInfo_GetCurrentCapital", null, "ds");
                return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
