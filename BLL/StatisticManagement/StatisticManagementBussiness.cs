using BLL.ClassManagement;
using BLL.NameCallingManagement;
using BLL.OverdueManagement;
using BLL.StatisticManagement.ClassStatistic;
using BLL.StatisticManagement.FinanceStatistic;
using BLL.StatisticManagement.GeneralAndExport;
using BLL.StatisticManagement.TeachingStatistic;
using BLL.StatisticManagement.TraineeStatistic;
using BLL.TeachingManagement.RegularTeaching;
using BLL.TraineeManagement;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement
{
    public class StatisticManagementBussiness
    {
        public FinanceStatisticBussiness Finance { get; private set; }
        public TeachingStatisticBussiness Teacher { get; private set; }
        public TraineeStatisticBussiness Trainee { get; private set; }
        public GeneralAndExportBussiness General { get; private set; }
        public ClassStatisticBussiness ClassStatistic { get; private set; }
        public GeneralInfo GeneralDal { get; private set; }

        public StatisticManagementBussiness(PaymentInfo paymentDal, TraineeManagementBussiness trainees, BlockClassManagement blocks, RegularClassManagement regulars,
            NameCallingBussiness calling, TraineeInfo trainee, OverdueManagementBussiness overdue)
        {
            GeneralDal = new GeneralInfo();
            Finance = new FinanceStatisticBussiness(paymentDal, trainees, blocks, regulars, GeneralDal);
            Teacher = new TeachingStatisticBussiness(calling, regulars, blocks);
            Trainee = new TraineeStatisticBussiness(trainees, regulars, new RegularTraineeBussiness(trainee), overdue, calling, paymentDal);
            General = new GeneralAndExportBussiness(trainees.Dal, GeneralDal, blocks, regulars, trainees);
            ClassStatistic = new ClassStatisticBussiness(regulars);
        }
    }
}
