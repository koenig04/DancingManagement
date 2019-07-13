using BLL.ClassManagement;
using BLL.TeachingManagement.BlockTeaching;
using BLL.TeachingManagement.RegularTeaching;
using Common;
using DAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL.PDFExporter
{
    public class PDFExporterCallingList : PDFExporterBase
    {
        private RegularClassManagement regular;
        private BlockClassManagement block;
        private RegularTraineeBussiness regularTrainee;
        private BlockTraineeBussiness blockTrainee;
        public PDFExporterCallingList(TraineeInfo traineeDAL) : base()
        {
            TotalCols = 13;
            if (!Directory.Exists(GlobalVariables.CallingListPDFExportPath))
            {
                Directory.CreateDirectory(GlobalVariables.CallingListPDFExportPath);
            }

            regular = new RegularClassManagement();
            block = new BlockClassManagement();
            regularTrainee = new RegularTraineeBussiness(traineeDAL);
            blockTrainee = new BlockTraineeBussiness(traineeDAL);
        }

        public bool Export()
        {
            try
            {
                //清空导出文件夹
                DirectoryInfo dir = new DirectoryInfo(GlobalVariables.CallingListPDFExportPath);
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

                //Export regular calling list
                List<RegularClassModel> regularClasses = regular.GetClassList();
                foreach (RegularClassModel regular in regularClasses)
                {
                    List<TraineeModel> regularTrainees = regularTrainee.GetTraineesSync(regular.ClassID);
                    if (regularTrainees != null && regularTrainees.Count > 0)
                    {
                        string exportFile = GlobalVariables.CallingListPDFExportPath + regular.ClassName + ".pdf";
                        Doc = new Document(PageSize.A4);
                        PdfWriter.GetInstance(Doc, new FileStream(exportFile, FileMode.Create));
                        Doc.Open();

                        PdfPTable table = new PdfPTable(TotalCols);
                        AddTitle(regular.ClassName, table);
                        AddBlank(Font_Title, table);

                        PdfPCell cell_name_blank = CreateCellWithFullBorder(" ", Font_TableHeader, 3);
                        table.AddCell(cell_name_blank);
                        for (int i = 0; i < 10; i++)
                        {
                            PdfPCell cell_calling_blank = CreateCellWithFullBorder(" ", Font_TableHeader, 1);
                            table.AddCell(cell_calling_blank);
                        }


                        regularTrainees.Sort();
                        foreach (TraineeModel item in regularTrainees)
                        {
                            PdfPCell cell_name = CreateCellWithFullBorder(string.Format("{0}({1})", item.TraineeName, item.RemainRegularCount), Font_TableHeader, 3);
                            table.AddCell(cell_name);
                            for (int i = 0; i < 10; i++)
                            {
                                PdfPCell cell_calling = CreateCellWithFullBorder("", Font_TableHeader, 1);
                                table.AddCell(cell_calling);
                            }
                        }
                        Doc.Add(table);
                        Doc.Close();
                    }
                }

                //Export block calling list
                List<BlockClassModel> blockClasses = block.GetClassList();
                foreach (BlockClassModel block in blockClasses)
                {
                    List<TraineeModel> blockTrainees = blockTrainee.GetTraineesSync(block.ClassID);
                    if (blockTrainees != null && blockTrainees.Count > 0)
                    {
                        string exportFile = GlobalVariables.CallingListPDFExportPath + block.ClassName + ".pdf";
                        Doc = new Document(PageSize.A4);
                        PdfWriter.GetInstance(Doc, new FileStream(exportFile, FileMode.Create));
                        Doc.Open();

                        PdfPTable table = new PdfPTable(TotalCols);
                        AddTitle(block.ClassName, table);
                        AddBlank(Font_Title, table);

                        PdfPCell cell_name_blank = CreateCellWithFullBorder(" ", Font_TableHeader, 3);
                        table.AddCell(cell_name_blank);
                        for (int i = 0; i < 10; i++)
                        {
                            PdfPCell cell_calling_blank = CreateCellWithFullBorder(" ", Font_TableHeader, 1);
                            table.AddCell(cell_calling_blank);
                        }


                        blockTrainees.Sort();
                        foreach (TraineeModel item in blockTrainees)
                        {
                            PdfPCell cell_name = CreateCellWithFullBorder(string.Format("{0}", item.TraineeName), Font_TableHeader, 3);
                            table.AddCell(cell_name);
                            for (int i = 0; i < 10; i++)
                            {
                                PdfPCell cell_calling = CreateCellWithFullBorder("", Font_TableHeader, 1);
                                table.AddCell(cell_calling);
                            }
                        }
                        Doc.Add(table);
                        Doc.Close();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
