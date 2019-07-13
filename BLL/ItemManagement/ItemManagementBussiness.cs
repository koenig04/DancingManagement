using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ItemManagement
{
    public class ItemManagementBussiness
    {
        public static ItemManagementBussiness Instance = new ItemManagementBussiness();

        public List<ItemInfoModel> Items { get; private set; }
        private ItemInfo _dal;
        public ItemManagementBussiness()
        {
            _dal = new ItemInfo();
            Items = _dal.GetItems();
        }
    }
}
