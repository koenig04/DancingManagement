using DBUtility;
using Model;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class PaymentInfo
    {
        public bool AddCountPayment(ClassPaymentModel model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@PaymentDate", SqlDbType.Date),
                    new SqlParameter("@TraineeID", SqlDbType.VarChar,50),
                    new SqlParameter("@TotalCost", SqlDbType.Decimal,9),
                    new SqlParameter("@ClassType",SqlDbType.Int),
                    new SqlParameter("@ClassID", SqlDbType.VarChar,50),
                    new SqlParameter("@TotalCount", SqlDbType.Int),
                    new SqlParameter("@PaymentTerms",SqlDbType.Int)
                                        };
            parameters[0].Value = model.PaymentDate;
            parameters[1].Value = model.TraineeID;
            parameters[2].Value = model.TotalCost;
            parameters[3].Value = model.ClassType;
            parameters[4].Value = model.ClassID;
            parameters[5].Value = model.TotalCount;
            parameters[6].Value = model.PaymentTerms;

            DbHelperSQL.RunProcedure("ClassPaymentInfo_ADD_CountPayment_LK", parameters, out rowsAffected);
            return true;
        }

        public bool AddNormalPayment(AccountInfoModel model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@AccountDate", SqlDbType.Date),
                    new SqlParameter("@ItemID", SqlDbType.VarChar,50),
                    new SqlParameter("@AccountAmount", SqlDbType.Decimal,9),
                    new SqlParameter("@Notice",SqlDbType.VarChar,200)
                                        };
            parameters[0].Value = model.AccountDate;
            parameters[1].Value = model.ItemID;
            parameters[2].Value = model.AccountAmount;
            parameters[3].Value = model.Notice;

            DbHelperSQL.RunProcedure("AccountInfo_ADD_LK", parameters, out rowsAffected);
            return true;
        }

        public List<ClassPaymentModel> GetClassPaymentList(DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@StartDate", SqlDbType.Date),
                    new SqlParameter("@EndDate", SqlDbType.Date)
                                        };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            DataSet ds = DbHelperSQL.RunProcedure("ClassPaymentInfo_GetList_LK", parameters, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new ClassPaymentModel()
                        {
                            PaymentID = d.Field<string>("PaymentID"),
                            PaymentType = d.Field<int>("PaymentType"),
                            PaymentDate = d.Field<DateTime>("PaymentDate"),
                            TraineeID = d.Field<string>("TraineeID"),
                            ClassType = d.Field<int>("ClassType"),
                            TotalCost = d.Field<decimal>("TotalCost"),
                            ClassID = d.Field<string>("ClassID")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<TeacherFeeModel> GetTeacherFeeList(DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@StartDate", SqlDbType.Date),
                    new SqlParameter("@EndDate", SqlDbType.Date)
                                        };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            DataSet ds = DbHelperSQL.RunProcedure("TeacherFee_GetList_LK", parameters, "ds");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new TeacherFeeModel()
                        {
                            TeacherFeeID = d.Field<string>("TeacherFeeID"),
                            FeeYear = d.Field<int>("GeneralYear"),
                            FeeMonth = d.Field<int>("GeneralMonth"),
                            TeacherID = d.Field<string>("TeacherID"),
                            Amount = d.Field<decimal>("Amount"),
                            PaymentDate = d.Field<DateTime>("PaymentDate")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<AccountInfoModel> GetAccountListByItem(string itemID, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ItemID",SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate", SqlDbType.Date),
                    new SqlParameter("@EndDate", SqlDbType.Date)
                                        };
            parameters[0].Value = itemID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            DataSet ds = DbHelperSQL.RunProcedure("AccountInfo_GetList_ByItem_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new AccountInfoModel()
                        {
                            AccountID = d.Field<string>("AccountID"),
                            IsIncome = d.Field<bool>("IsIncome"),
                            AccountDate = d.Field<DateTime>("AccountDate"),
                            ItemID = d.Field<string>("ItemID"),
                            AccountAmount = d.Field<decimal>("AccountAmount"),
                            Notice = d.Field<string>("Notice")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<AccountInfoModel> GetAccountListByAll(bool isIncome, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@IsIncome",SqlDbType.VarChar,50),
                    new SqlParameter("@StartDate", SqlDbType.Date),
                    new SqlParameter("@EndDate", SqlDbType.Date)
                                        };
            parameters[0].Value = isIncome;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            DataSet ds = DbHelperSQL.RunProcedure("AccountInfo_GetList_LK", parameters, "ds");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new AccountInfoModel()
                        {
                            AccountID = d.Field<string>("AccountID"),
                            IsIncome = d.Field<bool>("IsIncome"),
                            AccountDate = d.Field<DateTime>("AccountDate"),
                            ItemID = d.Field<string>("ItemID"),
                            AccountAmount = d.Field<decimal>("AccountAmount"),
                            Notice = d.Field<string>("Notice")
                        }).ToList();
            }
            else
            {
                return new List<AccountInfoModel>();
            }
        }

        public void DelAccountInfo(string accountID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@AccountID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = accountID;

            DbHelperSQL.RunProcedure("AccountInfo_DEL_LK", parameters, out rowsAffected);
        }

        public void DelTeacherFee(string feeID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@FeeID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = feeID;

            DbHelperSQL.RunProcedure("TeacherFee_DEL_LK", parameters, out rowsAffected);
        }

        public void DelClassPayment(string paymentID)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
                    new SqlParameter("@PaymentID", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = paymentID;

            DbHelperSQL.RunProcedure("ClassPaymentInfo_DEL_LK", parameters, out rowsAffected);
        }
    }
}
