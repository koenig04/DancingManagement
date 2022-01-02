using BLL.ClassManagement;
using BLL.NameCallingManagement;
using BLL.OverdueManagement;
using BLL.TeachingManagement.RegularTeaching;
using BLL.TraineeManagement;
using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.TraineeStatistic
{
    public class TraineeStatisticBussiness
    {
        public PresenceBussiness Presence { get; private set; }

        public TraineeStatisticBussiness(TraineeManagementBussiness trainees, RegularClassManagement regular,
            RegularTraineeBussiness regularTrainees, OverdueManagementBussiness overdue, NameCallingBussiness calling,
            PaymentInfo paymentDal)
        {
            Presence = new PresenceBussiness(trainees, regular, regularTrainees, overdue, calling, paymentDal);
        }
    }
}
