using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aspose.Cells;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace UO_WebApplication
{
    public partial class ProductionSummaryByOperation : Report
    {
        static ProductionSummaryByOperation()
        {
            InitCmdIn_Out_Yield_CT();
            InitCmdWIPQty();
            InitCmdOperation();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private Worksheet FillWorksheet(ref Workbook wb)
        {
            Styles styles = InitWorkbookStyles(ref wb);
            DateTime reportDate = DateTime.Parse(dateCutTime.Text);

            using (OracleDataReader rdOperation = cmdOperation.ExecuteReader())
            {
                string objecttype = null;
                Worksheet ws = null;
                Cells cls;
                int startRow = 7, startCol = 2;
                DateTime dtdStart = reportDate.AddDays(-1);
                DateTime wtdStart = reportDate.AddDays(-7);
                DateTime mtdStart = reportDate.AddMonths(-1);
                while (rdOperation.Read())
                {
                    if (objecttype != rdOperation["objecttype"].ToString())
                    {
                        ws = NewReportSheet(ref wb, reportTitle.InnerText, objecttype, reportDate);

                        cls = ws.Cells;
                        cls[4, 0].PutValue("DTD:");
                        CellValueStyle(cls[4, 1], dtdStart, styles["DateTime"]);
                        cls[4, 2].PutValue("To");
                        CellValueStyle(cls[4, 3], reportDate, styles["DateTime"]);
                        
                        cls[5, 0].PutValue("WTD:");
                        CellValueStyle(cls[5, 1], wtdStart, styles["DateTime"]);
                        cls[5, 2].PutValue("To");
                        CellValueStyle(cls[5, 3], reportDate, styles["DateTime"]);
                        
                        cls[6, 0].PutValue("MTD:");
                        CellValueStyle(cls[6, 1], mtdStart, styles["DateTime"]);
                        cls[6, 2].PutValue("To");
                        CellValueStyle(cls[6, 3], reportDate, styles["DateTime"]);

                        startRow = 7; startCol = 2;
                    }
                    objecttype = rdOperation["objecttype"].ToString();
                    string operationname = rdOperation["operationname"].ToString();
                    cls = ws.Cells;
                    cls[startRow, startCol].PutValue(operationname);
                    SetMergeAndStyle(styles["ColumnHeader"], cls, startRow, startCol, 1, 5);

                    startRow++;
                    CellValueStyle(cls[startRow, startCol + 0], "In", styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow, startCol + 1], "Out", styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow, startCol + 2], "WIP", styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow, startCol + 3], "Yield", styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow, startCol + 4], "CT", styles["ColumnHeader"]);


                    Fill_In_Out_Yield_CT(mtdStart, wtdStart, dtdStart, reportDate, operationname, startRow, startCol, cls, styles);
                    Fill_WIPQty(operationname, startRow, startCol, cls, styles);

                    startCol += 5;
                    startRow = 7;
                }
                return ws;
            }
        }

        private static void Fill_In_Out_Yield_CT(DateTime mtdDate, DateTime wtdDate, DateTime dtdDate, DateTime endDate, string operationName, int startRow, int startCol, Cells cls, Styles styles)
        {
            int dtdStartRow = startRow + 1;
            int wtdStartRow = startRow + 2;
            int mtdStartRow = startRow + 3;
            cmdIn_Out_Yield_CT.Parameters["END_DATE"].Value = endDate;
            cmdIn_Out_Yield_CT.Parameters["OPERATION_NAME"].Value = operationName;

            cmdIn_Out_Yield_CT.Parameters["START_DATE"].Value = dtdDate;
            using (OracleDataReader rd = cmdIn_Out_Yield_CT.ExecuteReader())
            {
                while (rd.Read())
                {
                    CellValueStyle(cls[dtdStartRow, 0], rd["productname"], styles["ColumnHeader"]);
                    CellValueStyle(cls[dtdStartRow, 1], "DTD", styles["ColumnHeader"]);
                    CellValueStyle(cls[dtdStartRow, startCol + 0], rd["InQty"], styles["Data"]);
                    CellValueStyle(cls[dtdStartRow, startCol + 1], rd["OutQty"], styles["Data"]);
                    cls[dtdStartRow, startCol + 3].R1C1Formula = "=1-" + rd["RejectQty"] + "/R[0]C[-3]";
                    cls[dtdStartRow, startCol + 3].Style = styles["Percent"];
                    CellValueStyle(cls[dtdStartRow, startCol + 4], rd["CycleTime"], styles["Data"]);
                    dtdStartRow += 3;
                }
            }

            cmdIn_Out_Yield_CT.Parameters["START_DATE"].Value = wtdDate;
            using (OracleDataReader rd = cmdIn_Out_Yield_CT.ExecuteReader())
            {
                while (rd.Read())
                {
                    CellValueStyle(cls[wtdStartRow, 0], rd["productname"], styles["ColumnHeader"]);
                    CellValueStyle(cls[wtdStartRow, 1], "WTD", styles["ColumnHeader"]);
                    CellValueStyle(cls[wtdStartRow, startCol + 0], rd["InQty"], styles["Data"]);
                    CellValueStyle(cls[wtdStartRow, startCol + 1], rd["OutQty"], styles["Data"]);
                    cls[wtdStartRow, startCol + 3].R1C1Formula = "=1-" + rd["RejectQty"] + "/R[0]C[-3]";
                    cls[wtdStartRow, startCol + 3].Style = styles["Percent"];
                    CellValueStyle(cls[wtdStartRow, startCol + 4], rd["CycleTime"], styles["Data"]);
                    wtdStartRow += 3;
                }
            }

            cmdIn_Out_Yield_CT.Parameters["START_DATE"].Value = mtdDate;
            using (OracleDataReader rd = cmdIn_Out_Yield_CT.ExecuteReader())
            {
                while (rd.Read())
                {
                    CellValueStyle(cls[mtdStartRow, 0], rd["productname"], styles["ColumnHeader"]);
                    CellValueStyle(cls[mtdStartRow, 1], "MTD", styles["ColumnHeader"]);
                    CellValueStyle(cls[mtdStartRow, startCol + 0], rd["InQty"], styles["Data"]);
                    CellValueStyle(cls[mtdStartRow, startCol + 1], rd["OutQty"], styles["Data"]);
                    cls[mtdStartRow, startCol + 3].R1C1Formula = "=1-" + rd["RejectQty"] + "/R[0]C[-3]";
                    cls[mtdStartRow, startCol + 3].Style = styles["Percent"];
                    CellValueStyle(cls[mtdStartRow, startCol + 4], rd["CycleTime"], styles["Data"]);
                    mtdStartRow += 3;
                }
            }
        }

        private static void Fill_WIPQty(string operationName, int startRow, int startCol, Cells cls, Styles styles)
        {
            startRow++;
            cmdWIPQty.Parameters["OPERATION_NAME"].Value = operationName;
            using (OracleDataReader rdWIPQty = cmdWIPQty.ExecuteReader())
            {
                while (rdWIPQty.Read())
                {
                    CellValueStyle(cls[startRow + 0, 0], rdWIPQty["productname"], styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow + 0, 1], "DTD", styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow + 0, startCol + 2], rdWIPQty["WipQty"], styles["Data"]);
                    CellValueStyle(cls[startRow + 1, 0], rdWIPQty["productname"], styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow + 1, 1], "WTD", styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow + 1, startCol + 2], rdWIPQty["WipQty"], styles["Data"]);
                    CellValueStyle(cls[startRow + 2, 0], rdWIPQty["productname"], styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow + 2, 1], "MTD", styles["ColumnHeader"]);
                    CellValueStyle(cls[startRow + 2, startCol + 2], rdWIPQty["WipQty"], styles["Data"]);
                    startRow += 3;
                }
            }
        }

        #region OracleCommand used
        private static OracleCommand cmdOperation;
        private static void InitCmdOperation()
        {
            cmdOperation = new OracleCommand(
@"SELECT DISTINCT objecttype, operationname
FROM (SELECT op.objecttype, op.operationname
FROM operation op INNER JOIN spec ON spec.operationid=op.operationid
INNER JOIN specbase sb ON sb.specbaseid = spec.specbaseid
INNER JOIN Workflowstep ws ON ws.specbaseid = sb.specbaseid
WHERE op.objectcategory = 'WIP'
ORDER BY op.objecttype, ws.sequence) seq_op
ORDER BY objecttype", ConnectionMgr.Connection);

            cmdOperation.Prepare();
        }

        private static OracleCommand cmdIn_Out_Yield_CT;
        private static void InitCmdIn_Out_Yield_CT()
        {
            cmdIn_Out_Yield_CT = new OracleCommand(
@"SELECT wlh.productname, sum(wlh.moveinqty) InQty, sum(wlh.moveoutqty) OutQty, 
round(AVG(wlh.moveouttimestamp-wlh.moveintimestamp)*24,2) CycleTime, sum(wlh.totalrejectqty) RejectQty
FROM a_wiplothistory wlh INNER JOIN specbase sb ON wlh.specname = sb.specname
INNER JOIN spec ON sb.specbaseid = spec.specbaseid
INNER JOIN operation op ON op.operationid = spec.operationid
WHERE op.operationname = :OPERATION_NAME
  AND wlh.moveintimestamp > :START_DATE
  AND wlh.moveouttimestamp < :END_DATE
GROUP BY wlh.productname
ORDER BY wlh.productname", ConnectionMgr.Connection);

            OracleParameter OPERATION_NAME_Param = new OracleParameter("OPERATION_NAME", OracleDbType.Varchar2);
            OPERATION_NAME_Param.Direction = ParameterDirection.Input;
            cmdIn_Out_Yield_CT.Parameters.Add(OPERATION_NAME_Param);

            OracleParameter START_DATE_Param = new OracleParameter("START_DATE", OracleDbType.Date);
            START_DATE_Param.Direction = ParameterDirection.Input;
            cmdIn_Out_Yield_CT.Parameters.Add(START_DATE_Param);

            OracleParameter END_DATE_Param = new OracleParameter("END_DATE", OracleDbType.Date);
            END_DATE_Param.Direction = ParameterDirection.Input;
            cmdIn_Out_Yield_CT.Parameters.Add(END_DATE_Param);

            cmdIn_Out_Yield_CT.Prepare();
        }

        private static OracleCommand cmdWIPQty;
        private static void InitCmdWIPQty()
        {
            cmdWIPQty = new OracleCommand(
@"SELECT pb.productname, SUM(co.qty) WipQty " +
@"FROM container co INNER JOIN currentstatus cs ON co.currentstatusid = cs.currentstatusid " +
@"INNER JOIN spec ON spec.specid = cs.specid " +
@"INNER JOIN operation op ON spec.operationid = op.operationid " +
@"INNER JOIN product pt ON co.productid = pt.productid " +
@"INNER JOIN productbase pb ON pt.productbaseid = pb.productbaseid " +
@"WHERE op.operationname = :OPERATION_NAME " +
@"GROUP BY pb.productname " +
@"ORDER BY pb.productname ", ConnectionMgr.Connection);

            OracleParameter OPERATION_NAME_Param = new OracleParameter("OPERATION_NAME", OracleDbType.Varchar2);
            OPERATION_NAME_Param.Direction = ParameterDirection.Input;
            cmdWIPQty.Parameters.Add(OPERATION_NAME_Param);

            cmdWIPQty.Prepare();
        }
        #endregion

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Workbook wb = new Workbook();
            Worksheet ws = FillWorksheet(ref wb);
            DataTable dt = ws.Cells.ExportDataTable(0, 0, 10, 8, true);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Workbook wb = new Workbook();
            Worksheet ws = FillWorksheet(ref wb);
            wb.Save("Report.xls", FileFormatType.Default, SaveType.OpenInExcel, Response);
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            Workbook wb = new Workbook();
            Worksheet ws = FillWorksheet(ref wb);
            wb.Save("Report.pdf", FileFormatType.Pdf, SaveType.OpenInExcel, Response);
        }
    }
}
