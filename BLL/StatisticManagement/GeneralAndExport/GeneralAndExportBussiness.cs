using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.StatisticManagement.GeneralAndExport
{
    public class GeneralAndExportBussiness
    {
        public delegate void TraineeCountChanged(int traineeCount);
        public event TraineeCountChanged TraineeCountChangedEvent;
        public delegate void CurrentCapitalChanged(decimal capital);
        public event CurrentCapitalChanged CurrentCapitalChangedEvent;

        private TraineeInfo _traineeDal;
        private GeneralInfo _generalDal;

        public GeneralAndExportBussiness(TraineeInfo traineeDal, GeneralInfo generalDal)
        {
            _traineeDal = traineeDal;
            _generalDal = generalDal;
        }

        public void Init()
        {
            RefreshCurrentCapital();
            RefreshTraineeCount();
        }

        public void RefreshTraineeCount()
        {
            TraineeCountChangedEvent?.Invoke(_traineeDal.GetCurrentTraineeCount());
        }

        public void RefreshCurrentCapital()
        {
            CurrentCapitalChangedEvent?.Invoke(_generalDal.GetCurrentCapital());
        }
    }
}
