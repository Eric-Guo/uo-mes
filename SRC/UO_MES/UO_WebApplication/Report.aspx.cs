using System;
using Aspose.Cells;
using Oracle.DataAccess.Client;

namespace UO_WebApplication
{
    public partial class Report : MES_PageBase
    {
        protected Styles InitWorkbookStyles(ref Workbook wb)
        {
            Styles styles = wb.Styles;

            Style style;

            style = styles[styles.Add()];
            style.Font.Size = 16;
            style.Name = "Title";

            style = styles[styles.Add()];
            style.Number = 22;  // format m/d/yy h:mm
            style.Name = "DateTime";

            style = styles[styles.Add()];
            style.Borders[BorderType.TopBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.LeftBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.RightBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Number = 10;  // Percent 0.00%
            style.Name = "Percent";

            style = styles[styles.Add()];
            style.Number = 16; // Date d-mmm
            style.Name = "Date";

            style = styles[styles.Add()];
            style.Borders[BorderType.TopBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.LeftBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.RightBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; 
            style.Name = "Data";

            style = styles[styles.Add()];
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.VerticalAlignment = TextAlignmentType.Center;
            style.Pattern = BackgroundType.Solid;
            style.ForegroundColor = System.Drawing.Color.Yellow;
            style.Borders[BorderType.TopBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.BottomBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.LeftBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.RightBorder].Color = System.Drawing.Color.Black;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Name = "ColumnHeader";

            return styles;
        }

        protected Worksheet NewReportSheet(ref Workbook wb, string reportTitle, string sheetName, DateTime reportDate)
        {
            Worksheet ws = wb.Worksheets[0];
            if (ws.Name.ToString() == "Sheet1")
                ws.Name = sheetName;
            else
                ws = wb.Worksheets.Add(sheetName);

            Cells cls = ws.Cells;

            cls[0, 1].PutValue(reportTitle);
            cls[0, 1].Style = wb.Styles["Title"];
            cls.Merge(0, 1, 1, 9);
            cls.SetRowHeight(0, 22);
            cls[1, 0].PutValue("Print Date:");
            cls[1, 1].PutValue(DateTime.Now);
            cls[1, 1].Style = wb.Styles["DateTime"];
            cls[2, 0].PutValue("Report Date:");
            cls[2, 1].PutValue(reportDate);
            cls[2, 1].Style = wb.Styles["DateTime"];
            return ws;
        }

        protected static void CellValueStyle(Cell cl, object value, Style style)
        {
            cl.PutValue(value);
            cl.Style = style;
        }

        protected static void SetMergeAndStyle(Aspose.Cells.Style style, Cells cls, int startRow, int startCol, int rowNumber, int colNumber)
        {
            cls.Merge(startRow, startCol, rowNumber, colNumber);
            for (int i = 0; i < rowNumber; i++)
                for (int j = 0; j < colNumber; j++)
                    cls[startRow + i, startCol + j].Style = style;
        }
    }
}
