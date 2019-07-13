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
    /// 独舞班管理类
    /// 该类负责对班级列表进行维护
    /// </summary>
    public class BlockClassManagement
    {
        public delegate void BlockClassChanged(OperationType operation, BlockClassModel model, int newIndex);
        /// <summary>
        /// 班级信息已经改变，通知别的页面进行相应修改
        /// </summary>
        public event BlockClassChanged BlockClassChangedEvent;
        public delegate void ChangeBlockClass(OperationType operation, BlockClassModel model);
        /// <summary>
        /// 通知班级修改界面显示，进行后续的班级编辑。
        /// </summary>
        public event ChangeBlockClass ChangeBlockClassEvent;

        private BlockClassInfo _dal = new BlockClassInfo();
        public List<BlockClassModel> BlockClassCollection { get; private set; }

        public BlockClassManagement()
        {
            BlockClassCollection = _dal.GetList();
            if (BlockClassCollection != null)
                BlockClassCollection.Sort();
            else
                BlockClassCollection = new List<BlockClassModel>();
        }

        public List<BlockClassModel> GetClassList()
        {
            List<BlockClassModel> blockClassCollection = _dal.GetList();
            if (blockClassCollection != null)
                blockClassCollection.Sort();
            else
                blockClassCollection = new List<BlockClassModel>();
            return blockClassCollection;
        }

        /// <summary>
        /// 增加/修改班级信息
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="model"></param>
        public void OperateClassInfo(OperationType operation, BlockClassModel model)
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

                    BlockClassCollection.Add(model);

                    break;
                case OperationType.Update:
                    _dal.Update(model, out classSerial);
                    model.ClassSerial = classSerial;

                    BlockClassCollection[BlockClassCollection.IndexOf(BlockClassCollection.Where(c => c.ClassID == model.ClassID).First())] = model;

                    break;
            }
            BlockClassCollection.Sort();
            newIndex = BlockClassCollection.IndexOf(model);
            BlockClassChangedEvent?.Invoke(operation, model, newIndex);
        }

        /// <summary>
        /// 界面通过该方法发起班级修改请求
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="model"></param>
        public void RaiseChangeBlockClassEvent(OperationType operation, BlockClassModel model)
        {
            if (operation == OperationType.Update)
            {
                model = BlockClassCollection.Where(c => c.ClassID == model.ClassID).First();
            }
            ChangeBlockClassEvent?.Invoke(operation, model);
        }

        public List<BlockClassModel> GetClassByTrainee(string traineeID)
        {
            return _dal.GetListByTrainee(traineeID);
        }
    }
}
