using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL.DBExporter
{
    public class DBExporterGeneral
    {
        private DBRelated _dbRelated;
        public DBExporterGeneral()
        {
            if (!Directory.Exists(GlobalVariables.DBExportPath))
            {
                Directory.CreateDirectory(GlobalVariables.DBExportPath);
            }
            _dbRelated = new DBRelated();
        }

        public bool ExportDB()
        {
            //清空DB导出文件夹
            DirectoryInfo dir = new DirectoryInfo(GlobalVariables.DBExportPath);
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

            string dbName = GlobalVariables.DBExportPath + "qingying" + DateTime.Now.ToString("yyyyMMdd") + ".bak";
            return _dbRelated.ExportDB(dbName);
        }
    }
}
