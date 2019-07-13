using Common;
using iTextSharp.text.pdf;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL.PDFExporter
{
    public class PDFExporterOverdue : PDFExporterBase
    {
        public PDFExporterOverdue() : base()
        {
            TotalCols = 11;
            if(!Directory.Exists(GlobalVariables.OverduePDFExportPath))
            {
                Directory.CreateDirectory(GlobalVariables.OverduePDFExportPath);
            }
        }

        public bool Export(List<OverdueModel> overdues)
        {
            try
            {
                //清空导出文件夹
                DirectoryInfo dir = new DirectoryInfo(GlobalVariables.OverduePDFExportPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }

                string exportFile = GlobalVariables.OverduePDFExportPath + "催费提醒_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";
                PdfWriter.GetInstance(Doc, new FileStream(exportFile, FileMode.Create));
                Doc.Open();

                PdfPTable table = new PdfPTable(TotalCols);
                AddTitle("缴费提醒", table);
                AddSubTitle(string.Format("（{0}）", DateTime.Now.ToString("yyyy.MM.dd")), table);
                AddBlank(Font_Title, table);


                PdfPCell cell_nameHeader = CreateCellWithBottomBorder("姓名", Font_TableHeader, 2);
                PdfPCell cell_classHeader = CreateCellWithBottomBorder("班级", Font_TableHeader, 4);
                PdfPCell cell_dateHeader = CreateCellWithBottomBorder("到期日期", Font_TableHeader, 3);
                PdfPCell cell_moneyHeader = CreateCellWithBottomBorder("续缴金额", Font_TableHeader, 2);

                table.AddCell(cell_nameHeader);
                table.AddCell(cell_classHeader);
                table.AddCell(cell_dateHeader);
                table.AddCell(cell_moneyHeader);

                foreach (OverdueModel itemn in overdues)
                {
                    PdfPCell cell_regularName = CreateCellWithNoBorder(itemn.TraineeName, Font_TableCell, 2);
                    PdfPCell cell_regularClass = CreateCellWithNoBorder(itemn.ClassName, Font_TableCell, 4);
                    PdfPCell cell_regularDate = CreateCellWithNoBorder(itemn.OverdueDate.ToString("yyyy年MM月dd日"), Font_TableCell, 3);
                    PdfPCell cell_regularMoney = CreateCellWithNoBorder(itemn.RenewAmount.ToString(), Font_TableCell, 2);

                    table.AddCell(cell_regularName);
                    table.AddCell(cell_regularClass);
                    table.AddCell(cell_regularDate);
                    table.AddCell(cell_regularMoney);
                }

                Doc.Add(table);
                Doc.Close();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
