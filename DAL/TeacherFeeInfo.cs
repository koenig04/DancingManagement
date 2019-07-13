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
    public class TeacherFeeInfo
    {
        public bool Add(string teacherID, int feeYear, int feeMonth, decimal amount)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@TeacherID", SqlDbType.VarChar,50),
                    new SqlParameter("@FeeYear", SqlDbType.Int),
                    new SqlParameter("@FeeMonth", SqlDbType.Int),
                    new SqlParameter("@Amount",SqlDbType.Decimal,9),
                    new SqlParameter("@HasPaid", SqlDbType.Bit)
                                        };
            parameters[0].Value = teacherID;
            parameters[1].Value = feeYear;
            parameters[2].Value = feeMonth;
            parameters[3].Value = amount;
            parameters[4].Direction = ParameterDirection.Output;

            DbHelperSQL.RunProcedure("TeacherFee_ADD_LK", parameters, out rowsAffected);
            return bool.Parse(parameters[4].Value.ToString());
        }
    }
}
