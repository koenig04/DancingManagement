using BLL.ClassManagement;
using BLL.PDFExporter;
using BLL.TraineeManagement;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.OverdueManagement
{
    public class OverdueManagementBussiness
    {
        public delegate void OverdueChanged();
        public event OverdueChanged OverdueChangedEvent;

        public List<OverdueModel> Overdues { get; private set; }
        private BlockClassManagement _block;
        private RegularClassManagement _regular;
        private OverdueInfo _dal;
        private PDFExporterOverdue _exporter;
        public OverdueManagementBussiness(BlockClassManagement block, RegularClassManagement regular)
        {
            _block = block;
            _regular = regular;
            _dal = new OverdueInfo();

            Overdues = _dal.GetList() ?? new List<OverdueModel>();
            Overdues.ForEach(o =>
            {
                o.ClassName = o.ClassType == 0 ?
                _regular.RegularClassCollection.Where(c => c.ClassID == o.ClassID).First().ClassName :
                _block.BlockClassCollection.Where(c => c.ClassID == o.ClassID).First().ClassName;
            });

            _exporter = new PDFExporterOverdue();
        }

        public List<OverdueModel> GetListbyTrainee(string traineeID)
        {
            return _dal.GetListbyTrainee(traineeID);
        }

        public void Refresh()
        {
            Overdues.Clear();
            Overdues = _dal.GetList() ?? new List<OverdueModel>();
            Overdues.ForEach(o =>
            {
                o.ClassName = o.ClassType == 0 ?
                _regular.RegularClassCollection.Where(c => c.ClassID == o.ClassID).First().ClassName :
                _block.BlockClassCollection.Where(c => c.ClassID == o.ClassID).First().ClassName;
            });

            OverdueChangedEvent?.Invoke();
        }

        public bool Export()
        {
            return _exporter.Export(Overdues);
        }
    }
}
