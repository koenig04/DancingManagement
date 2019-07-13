using BLL.ClassManagement;
using BLL.PDFExporter;
using BLL.TraineeManagement;
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
        private PDFExporterBill _billExporter;

        public GeneralAndExportBussiness(TraineeInfo traineeDal, GeneralInfo generalDal, BlockClassManagement blocks,
            RegularClassManagement regulars, TraineeManagementBussiness trainees)
        {
            _traineeDal = traineeDal;
            _generalDal = generalDal;
            _billExporter = new PDFExporterBill(blocks, regulars, trainees);
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

        public bool ExportBills(int billYear, int billMonth)
        {
            return _billExporter.Export(billYear, billMonth);
        }
    }
}
