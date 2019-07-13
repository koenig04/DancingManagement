using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Common
{
    public class GlobalVariables
    {
        public static string PDFExportPath = @"D:\导出\";
        public static string OverduePDFExportPath = PDFExportPath + @"催费提醒\";
        public static string CallingListPDFExportPath = PDFExportPath + @"花名册\";
        public static string BillExportPath = PDFExportPath + @"账单\";
        public static string ConfigPath = AppDomain.CurrentDomain.BaseDirectory + @"configs\";
        public static string IconPath = AppDomain.CurrentDomain.BaseDirectory + @"icons\";
        public static string RegularTypePath = ConfigPath + "RegularClassType.json";
        public static string BlockTypePath = ConfigPath + "BlockClassType.json";
        public static string BlockSeasonPath = ConfigPath + "BlockClassSeason.json";
        public static string ConfigurationPath = ConfigPath + "Configuration.json";
        public static string StatisticPath = ConfigPath + "Statistic.json";
        public static string TeachingStatisticPath = ConfigPath + "TeachingStatistic.json";
        public static string TeacherFeeIcon = IconPath + "teacherfee.png";
        public static string TeacherFeeIconPressed = IconPath + "teacherfeeW.png";
        public static string ClassPaymentIcon = IconPath + "dangcing.png";
        public static string ClassPaymentIconPressed = IconPath + "dangcingW.png";

        public static Color IncomeColor = (Color)ColorConverter.ConvertFromString("#4CB944");
        public static Color ExpenseColor = (Color)ColorConverter.ConvertFromString("#EE6C4D");
        public static Color MainBackColor = (Color)ColorConverter.ConvertFromString("#F4F4F4");
        public static Color DarkMainBackColor = (Color)ColorConverter.ConvertFromString("#C4C4C4");
        public static Color MainColor = (Color)ColorConverter.ConvertFromString("#246EB9");
        public static Color LightMainColor = (Color)ColorConverter.ConvertFromString("#6BA7E2");
        public static Color SecondaryColor = (Color)ColorConverter.ConvertFromString("#8165A0");
        public static Color AssistBackColor = (Color)ColorConverter.ConvertFromString("#F5EE9E");

        public static Dictionary<string, ClassType> ClassTypeDic = new Dictionary<string, ClassType>()
        {
            {"常规课",ClassType.Regular },
            {"短期课",ClassType.Block }
        };

        public static int ClassCountPerTerm = 20;
        public static DateTime StartDate = DateTime.Parse("2019-06-30");

        public static List<Color> CustomerColorSet = new List<Color>()
        {
            (Color)ColorConverter.ConvertFromString("#3E679B"),
            (Color)ColorConverter.ConvertFromString("#9F403E"),
            (Color)ColorConverter.ConvertFromString("#7F9B47"),
            (Color)ColorConverter.ConvertFromString("#695087"),
            (Color)ColorConverter.ConvertFromString("#3C8D9E"),
            (Color)ColorConverter.ConvertFromString("#CB7C3A"),
            (Color)ColorConverter.ConvertFromString("#5083BA"),
            (Color)ColorConverter.ConvertFromString("#BD5151"),
            (Color)ColorConverter.ConvertFromString("#99B352"),
            (Color)ColorConverter.ConvertFromString("#8165A0"),
            (Color)ColorConverter.ConvertFromString("#49AECA"),
            (Color)ColorConverter.ConvertFromString("#E29551")
        };
    }
}
