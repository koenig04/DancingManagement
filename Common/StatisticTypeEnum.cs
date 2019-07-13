using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public enum StatisticTypeEnum
    {
        AllIncome,
        AllOutcome,
        Diff,
        ClassFee,
        TeacherFee,
        Bonus,
        Salary,
        OtherIncome,
        OtherOutcome,
        Water,
        Elec,
        Exam,
        ThingIncome,
        ThingCost,
        Rent,
        Investment,
        Internet,
        Capital,
        NormalAccount
    }

    public enum TeachingStatisticTypeEnum
    {
        ClassCount
    }

    public enum StatisticCategoryEnum
    {
        Income,
        Outcome,
        Teaching
    }

    public enum StatisticFuncEnum
    {
        Finance,
        Teacher
    }

    public enum FinanceStatisticCategoryEnum
    {
        None = -1,
        All,
        Detail
    }

    public class StatisticTypeModel
    {
        public StatisticTypeEnum StatisticType { get; set; }
        public StatisticCategoryEnum Catogery { get; set; }
        public string ShownText { get; set; }
        public string ID { get; set; }
    }

    public class TeachingStatisticTypeModel
    {
        public TeachingStatisticTypeEnum StatisticType { get; set; }
        public string ShownText { get; set; }
    }

    public class StatisticTypeManagement
    {
        public static StatisticTypeManagement Instance = new StatisticTypeManagement();

        public List<StatisticTypeModel> StatisticTypeCollection { get; private set; }
        public List<TeachingStatisticTypeModel> TeachingStatisticTypeCollection { get; private set; }

        public StatisticTypeManagement()
        {
            if (File.Exists(GlobalVariables.StatisticPath))
            {
                using (StreamReader sr = new StreamReader(GlobalVariables.StatisticPath))
                {
                    string statisticTypeStr = sr.ReadToEnd();
                    StatisticTypeCollection = JsonConvert.DeserializeObject<List<StatisticTypeModel>>(statisticTypeStr);
                }
            }

            if (File.Exists(GlobalVariables.TeachingStatisticPath))
            {
                using (StreamReader sr = new StreamReader(GlobalVariables.TeachingStatisticPath))
                {
                    string teachingStatisticTypeStr = sr.ReadToEnd();
                    TeachingStatisticTypeCollection = JsonConvert.DeserializeObject<List<TeachingStatisticTypeModel>>(teachingStatisticTypeStr);
                }
            }
        }
    }
}
