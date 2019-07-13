using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.PDFExporter
{
    public class PDFExporterBase
    {
        protected BaseFont BF_Light;
        protected BaseFont BF_Hei;
        protected Font Font_Title;
        protected Font Font_SubTitle;
        protected Font Font_TableHeader;
        protected Font Font_TableCell;
        protected Font Font_TableCellSmall;
        protected Font Font_TableCellSmallBold;
        protected Document Doc;
        protected int TotalCols;

        public PDFExporterBase()
        {
            BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            BF_Hei = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font_Title = new Font(BF_Light, 25, Font.BOLD);
            Font_SubTitle = new Font(BF_Light, 20, Font.BOLD);
            Font_TableHeader = new Font(BF_Light, 18, Font.BOLD);
            Font_TableCell = new Font(BF_Light, 16);
            Font_TableCellSmall = new Font(BF_Light, 14);
            Font_TableCellSmallBold = new Font(BF_Light, 14, Font.BOLD);

            Doc = new Document(PageSize.A4);
        }

        protected void AddTitle(string title, PdfPTable table)
        {
            table.WidthPercentage = 100;
            PdfPCell cell_titleLeft = CreateCellWithNoBorder(string.Empty, Font_Title, 1);
            table.AddCell(cell_titleLeft);
            PdfPCell cell_title = CreateCellWithNoBorder(title, Font_Title, TotalCols - 2);
            table.AddCell(cell_title);
            PdfPCell cell_titleRight = CreateCellWithNoBorder(string.Empty, Font_Title, 1);
            table.AddCell(cell_titleRight);
        }

        protected void AddSubTitle(string subtitle, PdfPTable table)
        {
            table.WidthPercentage = 100;
            PdfPCell cell_subtitleLeft = CreateCellWithNoBorder(string.Empty, Font_SubTitle, 1);
            table.AddCell(cell_subtitleLeft);
            PdfPCell cell_subtitle = CreateCellWithNoBorder(subtitle, Font_SubTitle, TotalCols - 2);
            table.AddCell(cell_subtitle);
            PdfPCell cell_subtitleRight = CreateCellWithNoBorder(string.Empty, Font_SubTitle, 1);
            table.AddCell(cell_subtitleRight);
        }

        protected void AddBlank(Font font, PdfPTable table)
        {
            PdfPCell blank = CreateCellWithNoBorder(" ", font, TotalCols);
            table.AddCell(blank);
        }

        protected PdfPCell CreateCellWithBottomBorder(string content, Font font, int colSpan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content, font));
            cell.BorderWidthBottom = 2;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthRight = 0;
            cell.Colspan = colSpan;
            return CreateCenterCell(cell);
        }

        protected PdfPCell CreateCellWithTopBorder(string content, Font font, int colSpan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content, font));
            cell.BorderWidthBottom = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 2;
            cell.BorderWidthRight = 0;
            cell.Colspan = colSpan;
            return CreateCenterCell(cell);
        }

        protected PdfPCell CreateCellWithNoBorder(string content, Font font, int colSpan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content, font));
            cell.BorderWidth = 0;
            cell.Colspan = colSpan;
            return CreateCenterCell(cell);
        }

        protected PdfPCell CreateCellWithFullBorder(string content, Font font, int colSpan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content, font));
            cell.BorderWidth = 1;
            cell.Colspan = colSpan;
            return CreateCenterCell(cell);
        }

        private PdfPCell CreateCenterCell(PdfPCell cell)
        {
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.Padding = 8;
            return cell;
        }
    }
}
