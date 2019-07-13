using BLL.ClassManagement;
using BLL.TeachingManagement.BlockTeaching;
using BLL.TeachingManagement.Others;
using BLL.TeachingManagement.RegularTeaching;
using BLL.TraineeManagement;
using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement
{
    public class TeachingManagementBussiness
    {
        public RegularTeachingBussiness Regular { get; private set; }
        public BlockTeachingBussiness Block { get; private set; }
        public OthersBussiness Others { get; private set; }
        public event EventHandler TraineeCountChanged;

        public TeachingManagementBussiness(RegularClassManagement regular, TraineeManagementBussiness traineeManagement, BlockClassManagement block)
        {
            Regular = new RegularTeachingBussiness(regular, traineeManagement);
            Block = new BlockTeachingBussiness(block, traineeManagement);
            Others = new OthersBussiness(traineeManagement.Dal);

            Regular.TraineeCountChanged += (sender, args) => TraineeCountChanged?.Invoke(sender, args);
            Block.TraineeCountChanged += (sender, args) => TraineeCountChanged?.Invoke(sender, args);
        }
    }
}
