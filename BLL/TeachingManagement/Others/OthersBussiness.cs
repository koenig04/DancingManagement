using BLL.PDFExporter;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.TeachingManagement.Others
{
    public class OthersBussiness
    {
        private PDFExporterCallingList _exportCallingList;

        public OthersBussiness(TraineeInfo traineeDAL)
        {
            _exportCallingList = new PDFExporterCallingList(traineeDAL);
        }

        public bool ExportCallingList()
        {
            return _exportCallingList.Export();
        }
    }
}
