using Common;
using DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
   public class ItemInfo
    {
        public List<ItemInfoModel> GetItems()
        {
            DataSet ds = DbHelperSQL.RunProcedure("ItemInfo_GetList_LK", null, "ds");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from d in ds.Tables[0].AsEnumerable()
                        select new ItemInfoModel()
                        {
                            ItemID = d.Field<string>("ItemID"),
                            ItemName = d.Field<string>("ItemName"),
                            IsIncome = d.Field<bool>("IsIncome"),
                            IconName = GlobalVariables.IconPath+ d.Field<string>("IconName"),
                            IconNamePressed = GlobalVariables.IconPath + d.Field<string>("IconNamePressed")
                        }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
