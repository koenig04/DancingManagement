using BLL.ClassManagement;
using BLL.CommonBussiness;
using BLL.TeachingManagement.BlockTeaching;
using BLL.TeachingManagement.RegularTeaching;
using BLL.TraineeManagement;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.NameCallingManagement
{
    public class NameCallingMangementBussiness
    {
        public NameCallingBussiness NameCalling { get; private set; }
        public EditNameCallingBussiness EditCalling { get; private set; }
        public ClassSelecterBussiness ClassSelecter { get; private set; }
        public RegularTraineeBussiness RegularTrainee { get; private set; }
        public BlockTraineeBussiness BlockTrainee { get; private set; }

        public NameCallingMangementBussiness(BlockClassManagement block, RegularClassManagement regular, TraineeInfo trainee, TraineeManagementBussiness trainees)
        {
            RegularTrainee = new RegularTraineeBussiness(trainee);
            BlockTrainee = new BlockTraineeBussiness(trainee);
            NameCalling = new NameCallingBussiness();
            ClassSelecter = new ClassSelecterBussiness(regular, block, RegularTrainee, BlockTrainee);
            EditCalling = new EditNameCallingBussiness(NameCalling, block, regular, 
                new ClassSelecterBussiness(regular, block, new RegularTraineeBussiness(trainee), new BlockTraineeBussiness(trainee)),
                trainees);
        }
    }
}
