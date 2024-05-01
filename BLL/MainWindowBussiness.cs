using BLL.ClassManagement;
using BLL.NameCallingManagement;
using BLL.OverdueManagement;
using BLL.PaymentManagement;
using BLL.StatisticManagement;
using BLL.TeachingManagement;
using BLL.TraineeManagement;
using BLL.UserSettingManagement;
using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class MainWindowBussiness
    {
        public TeachingManagementBussiness Teaching { get; private set; }
        public NameCallingMangementBussiness Calling { get; private set; }
        public PaymentManagementBussiness Payment { get; private set; }
        public OverdueManagementBussiness Overdue { get; private set; }
        public StatisticManagementBussiness Statistic { get; private set; }
        public UserSettingManagementBussiness UserSetting { get; private set; }

        public MainWindowBussiness(string userID)
        {
            RegularClassManagement regular = new RegularClassManagement();
            BlockClassManagement block = new BlockClassManagement();
            TraineeManagementBussiness trainee = new TraineeManagementBussiness();
            Teaching = new TeachingManagementBussiness(regular, trainee, block);
            Calling = new NameCallingMangementBussiness(block, regular, trainee.Dal, trainee);
            Payment = new PaymentManagementBussiness(block, regular, trainee.Dal, Calling.NameCalling);
            Overdue = new OverdueManagementBussiness(block, regular);
            Statistic = new StatisticManagementBussiness(Payment.PaymentDAL, trainee, block, regular, Calling.NameCalling, trainee.Dal, Overdue);
            UserSetting = new UserSettingManagementBussiness(userID);

            //新增常规班缴费时，如果剩余课时回正，删除催费信息
            //新增独舞课缴费时，删除对应的催费信息
            //添加点名信息时，出勤/旷课会扣减剩余课时，如果剩余课时<=0，增加催费信息
            //删除点名信息，原来记录为出勤/旷课的学员会剩余课时+1，如果剩余课时>0，删除催费信息
            //在独舞班添加学员时，增加催费记录
            //在学员常规班发生变化时，如果存在常规班变化，更新催费记录中的金额和班级
            //在常规班级信息发生改变时，如果学费发生了变化，更新催费记录中所有该班级的未交费信息的金额
            //如果学员删除，会同时删除该学员当前的所有催费提醒
            //如果学员恢复，会根据常规剩余课数和独舞课的缴费情况，重新激活催费提醒
            Payment.ClassPayment.ClassFee.OverdueChangedEvent += Overdue.Refresh;
            Calling.NameCalling.OverdueChangedEvent += Overdue.Refresh;
            Teaching.Block.BlockTrainee.OverdueChangedEvent += Overdue.Refresh;
            Teaching.Regular.RegularTrainee.OverdueChangedEvent += Overdue.Refresh;
            Teaching.Regular.RegularClasses.OverdueChangedEvent += Overdue.Refresh;
            Teaching.Regular.OverdueChangedEvent += Overdue.Refresh;

            Teaching.TraineeCountChanged += (sender, args) => Statistic.General.RefreshTraineeCount();

            Payment.ClassPayment.CapitalChanged += (sender, args) => Statistic.General.RefreshCurrentCapital();
            Payment.NormalPayment.CapitalChanged += (sender, args) => Statistic.General.RefreshCurrentCapital();
            Statistic.Finance.GeneralCapitalChanged += (sender, args) => Statistic.General.RefreshCurrentCapital();
        }
    }
}
