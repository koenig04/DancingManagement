using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ClassManagement
{
    /// <summary>
    /// 常规班管理类
    /// 该类负责对班级列表进行维护
    /// </summary>
    public class RegularClassManagement
    {
        public delegate void RegularClassChanged(OperationType operation, RegularClassModel model, int newIndex);
        /// <summary>
        /// 班级信息已经改变，通知别的页面进行相应修改
        /// </summary>
        public event RegularClassChanged RegularClassChangedEvent;
        public delegate void ChangeRegularClass(OperationType operation, RegularClassModel model);
        /// <summary>
        /// 通知班级修改界面显示，进行后续的班级编辑。
        /// </summary>
        public event ChangeRegularClass ChangeRegularClassEvent;
        public delegate void OverdueChanged();
        public event OverdueChanged OverdueChangedEvent;

        private RegularClassInfo _dal = new RegularClassInfo();
        public List<RegularClassModel> RegularClassCollection { get; private set; }
        public Dictionary<string, List<RegularChangingModel>> RegularClassRoutine { get; private set; }

        public RegularClassManagement()
        {
            RegularClassCollection = _dal.GetList();
            if (RegularClassCollection != null)
                RegularClassCollection.Sort();
            else
                RegularClassCollection = new List<RegularClassModel>();

            RegularClassRoutine = new Dictionary<string, List<RegularChangingModel>>();
            List<RegularChangingModel> changingList = _dal.GetChangingList();
            if (changingList != null)
                foreach (List<RegularChangingModel> lst in changingList.GroupBy(r => r.RegularClassID).Select(g => g.ToList()).ToList())
                {
                    RegularClassRoutine.Add(lst[0].RegularClassID, lst);
                }

        }

        public List<RegularClassModel> GetClassList()
        {
            List<RegularClassModel> regularClassCollection = _dal.GetList();
            if (regularClassCollection != null)
                regularClassCollection.Sort();
            else
                regularClassCollection = new List<RegularClassModel>();
            return regularClassCollection;
        }

        /// <summary>
        /// 增加/修改班级信息
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="model"></param>
        public void OperateClassInfo(OperationType operation, RegularClassModel model)
        {
            string classID;
            int classSerial;
            int newIndex = 0;
            switch (operation)
            {
                case OperationType.Add:
                    _dal.Add(model, out classID, out classSerial);
                    model.ClassID = classID;
                    model.ClassSerial = classSerial;

                    RegularClassCollection.Add(model);

                    break;
                case OperationType.Update:
                    _dal.Update(model, out classSerial);
                    model.ClassSerial = classSerial;

                    RegularClassCollection[RegularClassCollection.IndexOf(RegularClassCollection.Where(c => c.ClassID == model.ClassID).First())] = model;
                    OverdueChangedEvent?.Invoke();
                    break;
            }
            RegularClassCollection.Sort();
            newIndex = RegularClassCollection.IndexOf(model);
            RegularClassChangedEvent?.Invoke(operation, model, newIndex);
        }

        /// <summary>
        /// 界面通过该方法发起班级修改请求
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="model"></param>
        public void RaiseChangeRegularClassEvent(OperationType operation, RegularClassModel model)
        {
            if (operation == OperationType.Update)
            {
                model = RegularClassCollection.Where(c => c.ClassID == model.ClassID).First();
            }
            ChangeRegularClassEvent?.Invoke(operation, model);
        }

        public string GetClassNameInHis(string regularClassID, DateTime hisDate)
        {
            string className;

            if (RegularClassRoutine.ContainsKey(regularClassID))// 有修改记录
            {
                RegularChangingModel thres = RegularClassRoutine[regularClassID].Where(c => c.ChangingDate > hisDate).FirstOrDefault();
                if (thres == null)//时间晚于最后一条修改记录
                {
                    className = RegularClassCollection.Where(r => r.ClassID == regularClassID).FirstOrDefault().ClassName;
                }
                else
                {
                    className = thres.PreviousName;
                }
            }
            else
            {
                className = RegularClassCollection.Where(r => r.ClassID == regularClassID).FirstOrDefault().ClassName;
            }

            return className;
        }
    }


}
