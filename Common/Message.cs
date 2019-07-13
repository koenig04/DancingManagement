using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Message
    {
        public static Message Instance = new Message();
        public Dictionary<MessageType,string> MessageDic { get; private set; }

        public Message()
        {
            MessageDic = new Dictionary<MessageType, string>();
            MessageDic.Add(MessageType.ClassTypeErr, "请选择课程类型");
            MessageDic.Add(MessageType.TeacherErr, "请选择任课老师");
            MessageDic.Add(MessageType.ClassDateErr, "请选择上课日期");
            MessageDic.Add(MessageType.ClassNameErr, "请选择课程");
            MessageDic.Add(MessageType.ClassCountErr, "请选择课程次数");
            MessageDic.Add(MessageType.NameCallingSussses, "点名信息添加成功");
            MessageDic.Add(MessageType.TimePaymentNotSupport, "目前不支持按时间缴纳学费");
            MessageDic.Add(MessageType.TermCountErr, "请选择交费的期数");
            MessageDic.Add(MessageType.TraineeErr, "请选择交费的学员");
            MessageDic.Add(MessageType.TraineePaymentSuccess, "学费录入成功");
            MessageDic.Add(MessageType.TeacherFeeDupliPaidErr, "任课老师的该月课时费已结算过");
            MessageDic.Add(MessageType.TeacherFeePaidSuccess, "课时费结算成功");
            MessageDic.Add(MessageType.MonthErr, "请选择月份");
            MessageDic.Add(MessageType.YearErr, "请选择年份");
            MessageDic.Add(MessageType.NormalPaymentSuccess, "一般收支录入成功");
            MessageDic.Add(MessageType.NormalPaymentAmountErr, "请输入正确的金额");
            MessageDic.Add(MessageType.NormalPaymentNoticeErr, "选择其他类别时必须输入备注");
            MessageDic.Add(MessageType.NormalPaymentItemErr, "请选择收支类别");
            MessageDic.Add(MessageType.StartDateErr, "请选择开始日期");
            MessageDic.Add(MessageType.EndDateErr, "请选择结束日期");
            MessageDic.Add(MessageType.SearchingTypeErr, "请选择筛选类别");
            MessageDic.Add(MessageType.ExportSuccess, "导出成功");
            MessageDic.Add(MessageType.ExportFailed, "导出失败");
            MessageDic.Add(MessageType.TraineeStatisticClassErr, "请选择班级");
            MessageDic.Add(MessageType.TraineeStatisticTraineeErr, "请选择学员");
            MessageDic.Add(MessageType.RepeateNameErr, "姓名已存在，请更换");
            MessageDic.Add(MessageType.NameCallingNoCallErr, "有学员未选择出勤情况");
            MessageDic.Add(MessageType.OldPWErr, "旧密码不正确");
            MessageDic.Add(MessageType.NewPWNotSameErr, "新密码两次输入不同");
            MessageDic.Add(MessageType.ChangePWSuccess, "密码修改成功");
        }
    }

    public enum MessageType
    {
        ClassTypeErr,
        TeacherErr,
        ClassDateErr,
        ClassNameErr,
        ClassCountErr,
        NameCallingSussses,
        TimePaymentNotSupport,
        TermCountErr,
        TraineeErr,
        TraineePaymentSuccess,
        TeacherFeeDupliPaidErr,
        TeacherFeePaidSuccess,
        MonthErr,
        YearErr,
        NormalPaymentSuccess,
        NormalPaymentAmountErr,
        NormalPaymentNoticeErr,
        NormalPaymentItemErr,
        StartDateErr,
        EndDateErr,
        SearchingTypeErr,
        ExportSuccess,
        ExportFailed,
        TraineeStatisticClassErr,
        TraineeStatisticTraineeErr,
        RepeateNameErr,
        NameCallingNoCallErr,
        OldPWErr,
        NewPWNotSameErr,
        ChangePWSuccess
    }

    public enum MessageLevel
    {
        Info,
        Warning
    }
}
