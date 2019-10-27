using BLL.ClassManagement;
using BLL.ItemManagement;
using BLL.TeacherManagement;
using BLL.TraineeManagement;
using Common;
using DAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using Model.Payment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL.PDFExporter
{
    class PDFExporterBill : PDFExporterBase
    {
        private PaymentInfo _paymentDal;
        private GeneralInfo _generalDal;
        private BlockClassManagement _blocks;
        private RegularClassManagement _regulars;
        private TraineeManagementBussiness _trainees;

        public PDFExporterBill(BlockClassManagement blocks, RegularClassManagement regulars, TraineeManagementBussiness trainees) : base()
        {
            TotalCols = 17;
            if (!Directory.Exists(GlobalVariables.BillExportPath))
            {
                Directory.CreateDirectory(GlobalVariables.BillExportPath);
            }
            _paymentDal = new PaymentInfo();
            _generalDal = new GeneralInfo();
            _blocks = blocks;
            _regulars = regulars;
            _trainees = trainees;
        }

        public bool Export(int billYear, int billMonth)
        {
            try
            {
                string fileName = GlobalVariables.BillExportPath + string.Format("清影舞蹈{0}年{1}月账单.pdf", billYear, billMonth);
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                DateTime startDate = DateTime.Parse(string.Format("{0}-{1}-1", billYear, billMonth));
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                List<BillItem> bills = GetBillDetails(startDate, endDate);
                decimal totalIncome = bills.Where(b => b.IsIncome).Sum(b => b.BillAmount);
                decimal totalExpense = bills.Where(b => !b.IsIncome).Sum(b => b.BillAmount);
                decimal diff = totalIncome - totalExpense;
                List<CapitalModel> capitals = _generalDal.GetCapitals(startDate, endDate);
                decimal previousCapital, currentCapital;
                if (capitals.Exists(c => c.GeneralYear < startDate.AddDays(-1).Year ||
                    (c.GeneralYear == startDate.AddDays(-1).Year && c.GeneralMonth <= startDate.AddDays(-1).Month)))
                {
                    previousCapital = capitals
                        .Where(c => c.GeneralYear < startDate.AddDays(-1).Year || (c.GeneralYear == startDate.AddDays(-1).Year && c.GeneralMonth <= startDate.AddDays(-1).Month))
                        .Sum(c => c.Capital);
                }
                else
                {
                    previousCapital = 0;
                }
                currentCapital = capitals
                    .Where(c => c.GeneralYear < startDate.Year || (c.GeneralYear == startDate.Year && c.GeneralMonth <= startDate.Month))
                    .Sum(c => c.Capital);


                Doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(Doc, new FileStream(fileName, FileMode.Create));
                Doc.Open();

                PdfPTable table = new PdfPTable(TotalCols);
                AddTitle("账单", table);
                AddSubTitle(string.Format("{0} - {1}", startDate.ToString("yyyy.MM.dd"), endDate.ToString("yyyy.MM.dd")), table);
                AddBlank(Font_Title, table);

                PdfPCell cell_no = CreateCellWithTopBorder("序号", Font_TableHeader, 2);
                PdfPCell cell_date = CreateCellWithTopBorder("日期", Font_TableHeader, 3);
                PdfPCell cell_category = CreateCellWithTopBorder("类别", Font_TableHeader, 2);
                PdfPCell cell_notice = CreateCellWithTopBorder("备注", Font_TableHeader, 4);
                PdfPCell cell_income = CreateCellWithTopBorder("收入", Font_TableHeader, 3);
                PdfPCell cell_expense = CreateCellWithTopBorder("支出", Font_TableHeader, 3);
                table.AddCell(cell_no);
                table.AddCell(cell_date);
                table.AddCell(cell_category);
                table.AddCell(cell_notice);
                table.AddCell(cell_income);
                table.AddCell(cell_expense);

                int serial = 1;
                foreach (BillItem item in bills)
                {
                    PdfPCell cell_serial = CreateCellWithNoBorder(serial.ToString(), Font_TableCellSmall, 2);
                    PdfPCell cell_billdate = CreateCellWithNoBorder(item.BillDate.ToString("yyyy.MM.dd"), Font_TableCellSmall, 3);
                    PdfPCell cell_billcategory = CreateCellWithNoBorder(item.BillCategory, Font_TableCellSmall, 2);
                    PdfPCell cell_billnotice = CreateCellWithNoBorder(item.Notice, Font_TableCellSmall, 4);
                    PdfPCell cell_billincome = CreateCellWithNoBorder(item.IsIncome ? ("+" + item.BillAmount.ToString()) : "", Font_TableCellSmall, 3);
                    PdfPCell cell_billexpense = CreateCellWithNoBorder(item.IsIncome ? "" : ("-" + item.BillAmount.ToString()), Font_TableCellSmall, 3);
                    table.AddCell(cell_serial);
                    table.AddCell(cell_billdate);
                    table.AddCell(cell_billcategory);
                    table.AddCell(cell_billnotice);
                    table.AddCell(cell_billincome);
                    table.AddCell(cell_billexpense);
                    serial++;
                }

                PdfPCell cell_incomeHead = CreateCellWithTopBorder("月收入", Font_TableCellSmallBold, 2);
                PdfPCell cell_incomeblank = CreateCellWithTopBorder("", Font_TableCellSmallBold, 9);
                PdfPCell cell_totalincome = CreateCellWithTopBorder("+" + totalIncome.ToString(), Font_TableCellSmallBold, 3);
                PdfPCell cell_incometailblank = CreateCellWithTopBorder("", Font_TableCellSmallBold, 3);
                table.AddCell(cell_incomeHead);
                table.AddCell(cell_incomeblank);
                table.AddCell(cell_totalincome);
                table.AddCell(cell_incometailblank);

                PdfPCell cell_expenseHead = CreateCellWithNoBorder("月支出", Font_TableCellSmallBold, 2);
                PdfPCell cell_expenseblank = CreateCellWithNoBorder("", Font_TableCellSmallBold, 12);
                PdfPCell cell_totalexpense = CreateCellWithNoBorder("-" + totalExpense.ToString(), Font_TableCellSmallBold, 3);
                table.AddCell(cell_expenseHead);
                table.AddCell(cell_expenseblank);
                table.AddCell(cell_totalexpense);

                PdfPCell cell_diffHead = CreateCellWithTopBorder("月结余", Font_TableCellSmallBold, 2);
                PdfPCell cell_diffblank = CreateCellWithTopBorder("", Font_TableCellSmallBold, diff > 0 ? 9 : 12);
                PdfPCell cell_diff = CreateCellWithTopBorder(diff > 0 ? ("+" + diff.ToString()) : diff.ToString(), Font_TableCellSmallBold, 3);
                table.AddCell(cell_diffHead);
                table.AddCell(cell_diffblank);
                table.AddCell(cell_diff);
                if (diff > 0)
                {
                    PdfPCell cell_difftailblank = CreateCellWithTopBorder("", Font_TableCellSmallBold, 3);
                    table.AddCell(cell_difftailblank);
                }

                PdfPCell cell_previousCapitalHead = CreateCellWithTopBorder("上月资产", Font_TableCellSmallBold, 3);
                cell_previousCapitalHead.HorizontalAlignment = Element.ALIGN_LEFT;
                PdfPCell cell_previoudCapitalBlank = CreateCellWithTopBorder("", Font_TableCellSmallBold, 8);
                PdfPCell cell_previousCapital = CreateCellWithTopBorder(previousCapital.ToString(), Font_TableCellSmallBold, 6);
                cell_previousCapital.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell_previousCapitalHead);
                table.AddCell(cell_previoudCapitalBlank);
                table.AddCell(cell_previousCapital);

                PdfPCell cell_currentCapitalHead = CreateCellWithNoBorder("本月资产", Font_TableCellSmallBold, 3);
                cell_currentCapitalHead.HorizontalAlignment = Element.ALIGN_LEFT;
                PdfPCell cell_currentCapitalBlank = CreateCellWithNoBorder("", Font_TableCellSmallBold, 8);
                PdfPCell cell_currentCapital = CreateCellWithNoBorder(currentCapital.ToString(), Font_TableCellSmallBold, 6);
                cell_currentCapital.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell_currentCapitalHead);
                table.AddCell(cell_currentCapitalBlank);
                table.AddCell(cell_currentCapital);

                PdfPCell cell_bottomline = CreateCellWithTopBorder("", Font_TableCellSmallBold, TotalCols);
                table.AddCell(cell_bottomline);

                Doc.Add(table);
                Doc.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private List<BillItem> GetBillDetails(DateTime startDate, DateTime endDate)
        {
            List<ClassPaymentModel> classPayment = _paymentDal.GetClassPaymentList(startDate, endDate);
            List<TeacherFeeModel> teacherFee = _paymentDal.GetTeacherFeeList(startDate, endDate);
            List<AccountInfoModel> accounts = new List<AccountInfoModel>();
            accounts.AddRange(_paymentDal.GetAccountListByAll(true, startDate, endDate));
            accounts.AddRange(_paymentDal.GetAccountListByAll(false, startDate, endDate));

            List<BillItem> bills = new List<BillItem>();
            if (classPayment != null)
            {
                foreach (ClassPaymentModel item in classPayment)
                {
                    bills.Add(new BillItem()
                    {
                        BillDate = item.PaymentDate,
                        BillCategory = "学费",
                        Notice = (item.ClassType == 0 ?
                            _regulars.RegularClassCollection.Where(r => r.ClassID == item.ClassID).First().ClassName :
                            _blocks.BlockClassCollection.Where(b => b.ClassID == item.ClassID).First().ClassName) +
                            "-" +
                            _trainees.SearchTraineeName(item.TraineeID),
                        IsIncome = true,
                        BillAmount = item.TotalCost
                    });
                }
            }
            if (teacherFee != null)
            {
                foreach (TeacherFeeModel item in teacherFee)
                {
                    bills.Add(new BillItem()
                    {
                        BillDate = item.PaymentDate,
                        BillCategory = "课时费",
                        Notice = string.Format("{0}年{1}月", item.FeeYear, item.FeeMonth) + "-" +
                            TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherID == item.TeacherID).First().TeacherName,
                        IsIncome = false,
                        BillAmount = item.Amount
                    });
                }
            }
            foreach (AccountInfoModel item in accounts)
            {
                bills.Add(new BillItem()
                {
                    BillDate = item.AccountDate,
                    BillCategory = ItemManagementBussiness.Instance.Items.Where(i => i.ItemID == item.ItemID).First().ItemName,
                    Notice = item.Notice,
                    IsIncome = item.IsIncome,
                    BillAmount = item.AccountAmount
                });
            }
            bills.Sort();
            return bills;
        }

        private class BillItem : IComparable<BillItem>
        {
            public DateTime BillDate { get; set; }
            public string BillCategory { get; set; }
            public string Notice { get; set; }
            public bool IsIncome { get; set; }
            public decimal BillAmount { get; set; }

            public int CompareTo(BillItem other)
            {
                if (BillDate > other.BillDate)
                {
                    return 1;
                }
                else if (BillDate < other.BillDate)
                {
                    return -1;
                }
                else
                {
                    if (BillCategory != other.BillCategory)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
