using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Pfm.Core.Entities;
using Pfm.Api.ViewModels;

namespace Pfm.Api.Helpers
{
    public class ExcelHelper
    {
        public ExcelHelper()
        {
        }


        public static IFont SetFontStyle(XSSFWorkbook xssfwb)
        {
            XSSFFont font = (XSSFFont)xssfwb.CreateFont();
            font.FontHeightInPoints = 12;
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.IsBold = true;

            IFont ifont = xssfwb.CreateFont();
            ifont.FontHeightInPoints = 12;
            ifont.IsItalic = false;
            return ifont;

        }


        public static XSSFCellStyle SetTextStye(XSSFWorkbook xssfwb)
        {
            XSSFCellStyle TextStyle = (XSSFCellStyle)xssfwb.CreateCellStyle();
            TextStyle.BorderBottom = BorderStyle.Thin;
            TextStyle.BorderLeft = BorderStyle.Thin;
            TextStyle.BorderRight = BorderStyle.Thin;
            TextStyle.BorderTop = BorderStyle.Thin;
            TextStyle.DataFormat = (short)CellType.String;
            TextStyle.SetFont(SetFontStyle(xssfwb));
            return TextStyle;
        }


        public static XSSFCellStyle SetTextStyleWithoutBorder(XSSFWorkbook xssfwb)
        {
            XSSFCellStyle TextStyle = (XSSFCellStyle)xssfwb.CreateCellStyle();
            TextStyle.BorderBottom = BorderStyle.None;
            TextStyle.BorderLeft = BorderStyle.None;
            TextStyle.BorderRight = BorderStyle.None;
            TextStyle.BorderTop = BorderStyle.None;
            TextStyle.DataFormat = (short)CellType.String;
            TextStyle.SetFont(SetFontStyle(xssfwb));
            return TextStyle;
        }

        public static XSSFCellStyle SetRpStye(XSSFWorkbook xssfwb)
        {
            XSSFCellStyle RpStyle = (XSSFCellStyle)xssfwb.CreateCellStyle();
            RpStyle.DataFormat = xssfwb.CreateDataFormat().GetFormat("#,##0.00");
            RpStyle.BorderBottom = BorderStyle.Thin;
            RpStyle.BorderLeft = BorderStyle.Thin;
            RpStyle.BorderRight = BorderStyle.Thin;
            RpStyle.BorderTop = BorderStyle.Thin;
            RpStyle.Alignment = HorizontalAlignment.Right;
            RpStyle.SetFont(SetFontStyle(xssfwb));
            return RpStyle;
        }
        public static XSSFCellStyle SetNumberStyeWithDecimal(XSSFWorkbook xssfwb, int numDec)
        {
            XSSFCellStyle NumberStyle = (XSSFCellStyle)xssfwb.CreateCellStyle();
            string format = "#,##0";
            for (int i = 0; i < numDec; i++)
            {
                if (i == 0)
                    format += ".";
                format += "0";
            }
            NumberStyle.DataFormat = xssfwb.CreateDataFormat().GetFormat(format);
            NumberStyle.BorderBottom = BorderStyle.Thin;
            NumberStyle.BorderLeft = BorderStyle.Thin;
            NumberStyle.BorderRight = BorderStyle.Thin;
            NumberStyle.BorderTop = BorderStyle.Thin;
            NumberStyle.Alignment = HorizontalAlignment.Right;
            NumberStyle.SetFont(SetFontStyle(xssfwb));
            return NumberStyle;
        }
        public static XSSFCellStyle SetNumberStye(XSSFWorkbook xssfwb)
        {
            XSSFCellStyle NumberStyle = (XSSFCellStyle)xssfwb.CreateCellStyle();

            NumberStyle.DataFormat = xssfwb.CreateDataFormat().GetFormat("#,##0");
            NumberStyle.BorderBottom = BorderStyle.Thin;
            NumberStyle.BorderLeft = BorderStyle.Thin;
            NumberStyle.BorderRight = BorderStyle.Thin;
            NumberStyle.BorderTop = BorderStyle.Thin;
            NumberStyle.Alignment = HorizontalAlignment.Right;
            NumberStyle.SetFont(SetFontStyle(xssfwb));
            return NumberStyle;
        }
        public static XSSFCellStyle SetDoubleStyle(XSSFWorkbook xssfwb)
        {
            XSSFCellStyle RpStyle = (XSSFCellStyle)xssfwb.CreateCellStyle();
            RpStyle.DataFormat = xssfwb.CreateDataFormat().GetFormat("#,##0.00000000000000");
            RpStyle.BorderBottom = BorderStyle.Thin;
            RpStyle.BorderLeft = BorderStyle.Thin;
            RpStyle.BorderRight = BorderStyle.Thin;
            RpStyle.BorderTop = BorderStyle.Thin;
            RpStyle.Alignment = HorizontalAlignment.Right;
            RpStyle.SetFont(SetFontStyle(xssfwb));
            return RpStyle;
        }
        private static IRow GenerateRow(ISheet sheet, int numRow, List<XSSFCellStyle> cellStyles)
        {
            IRow row = sheet.CreateRow(numRow);
            for (int i = 0; i < cellStyles.Count; i++)
            {
                row.CreateCell(i);
                row.GetCell(i).CellStyle = cellStyles[i];
            }
            return row;
        }
        public static string GenerateReportPresenceDaily(TbUser user, ReportPresenceExcelDailyViewModel kehadiran)
        {
            string TemplateFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("Resources/template_laporan_absensi_harian.xlsx"));
            string OutFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, user.IdUser.ToString() + ".xlsx");
            if (File.Exists(OutFileName))
                File.Delete(OutFileName);
            _ = new
            XSSFWorkbook();
            Console.WriteLine(OutFileName);
            FileStream filex = new(TemplateFileName, FileMode.Open, FileAccess.Read);
            XSSFWorkbook xssfwb = new(filex);
            XSSFCellStyle TextStyle = SetTextStye(xssfwb);
            XSSFCellStyle NumberStyle = SetNumberStye(xssfwb);
            ISheet sheet = xssfwb.GetSheet("Sheet1");
            sheet.CopySheet("laporan", true);
            ISheet sheet1 = xssfwb.GetSheet("laporan");
            sheet1.GetRow(3).GetCell(1).SetCellValue(kehadiran.Unit);
            sheet1.GetRow(4).GetCell(1).SetCellValue(kehadiran.Periode);
            int _row = 6;
            List<XSSFCellStyle> cellStyles = new();
            for (int i = 0; i <= 4; i++)
            {
                switch (i)
                {
                    case 0:
                    case 9:
                        cellStyles.Add(NumberStyle);
                        break;
                    default:
                        cellStyles.Add(TextStyle);
                        break;
                }
            }
            foreach (var absen in kehadiran.Lists)
            {
                IRow row = GenerateRow(sheet1, ++_row, cellStyles);
                int i = 0;
                row.GetCell(i).SetCellValue($"{absen.No}");
                row.GetCell(++i).SetCellValue(absen.Nama.ToUpper());
                row.GetCell(++i).SetCellValue(absen.Tanggal);
                row.GetCell(++i).SetCellValue(absen.JamMasuk);
                row.GetCell(++i).SetCellValue(absen.JamPulang);
            }
            xssfwb.RemoveSheetAt(0);
            FileStream file = new(OutFileName, FileMode.Create);
            xssfwb.Write(file);
            file.Close();
            filex.Close();
            return OutFileName;
        }
    }
}


