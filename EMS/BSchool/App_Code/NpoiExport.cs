using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using NPOI.POIFS.FileSystem;
using NPOI.HPSF;








public class NpoiExport : IDisposable
{
    const int MaximumNumberOfRowsPerSheet = 65500;
    const int MaximumSheetNameLength = 25;
    protected HSSFWorkbook Workbook { get; set; }

    public NpoiExport(string companyname, string subject, string createdby)
    {
        this.Workbook = new HSSFWorkbook();

        DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
        dsi.Company = companyname;
        this.Workbook.DocumentSummaryInformation = dsi;

        //create a entry of SummaryInformation
        SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
        si.Subject = subject;
        si.Author = createdby;
        si.Comments = "Copy Rights @IES";
        si.CreateDateTime = DateTime.Now;
        Workbook.SummaryInformation = si;
    }

    protected string EscapeSheetName(string sheetName)
    {
        var escapedSheetName = sheetName
                                    .Replace("/", "-")
                                    .Replace("\\", " ")
                                    .Replace("?", string.Empty)
                                    .Replace("*", string.Empty)
                                    .Replace("[", string.Empty)
                                    .Replace("]", string.Empty)
                                    .Replace(":", string.Empty);

        if (escapedSheetName.Length > MaximumSheetNameLength)
            escapedSheetName = escapedSheetName.Substring(0, MaximumSheetNameLength);

        return escapedSheetName;
    }

    protected ISheet CreateExportDataTableSheetAndHeaderRow(DataTable exportData, string sheetName, ICellStyle headerRowStyle)
    {
        var sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));
        // Create the header row
        sheet.CreateRow(0);
        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, exportData.Columns.Count - 1));
        var row = sheet.CreateRow(0);
        var cell = row.CreateCell(0);
        cell.SetCellValue(sheetName);
        cell.CellStyle = headerRowStyle;
        row = sheet.CreateRow(1);

        sheet.CreateFreezePane(0, 2);

        for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
        {
            cell = row.CreateCell(colIndex);
            cell.SetCellValue(exportData.Columns[colIndex].ColumnName);

            if (headerRowStyle != null)
                cell.CellStyle = headerRowStyle;
        }

        return sheet;
    }

    public void ExportDataTableToWorkbook(DataTable exportData, string sheetName)
    {
        // Create the header row cell style
        var headerLabelCellStyle = this.Workbook.CreateCellStyle();
        headerLabelCellStyle.BorderBottom = BorderStyle.Thick;
        headerLabelCellStyle.BorderLeft = BorderStyle.Thick;
        headerLabelCellStyle.BorderRight = BorderStyle.Thick;
        headerLabelCellStyle.BorderTop = BorderStyle.Thick;
        headerLabelCellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
        headerLabelCellStyle.FillPattern = FillPattern.SolidForeground;
        headerLabelCellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;



        var headerLabelFont = this.Workbook.CreateFont();
        headerLabelFont.Boldweight = (short)FontBoldWeight.Bold;
        headerLabelFont.FontHeightInPoints = 12;
        headerLabelFont.FontName = "calibri";


        var rowLabelCellStyle = this.Workbook.CreateCellStyle();
        rowLabelCellStyle.BorderBottom = BorderStyle.Thin;
        rowLabelCellStyle.BorderLeft = BorderStyle.Thin;
        rowLabelCellStyle.BorderRight = BorderStyle.Thin;
        rowLabelCellStyle.BorderTop = BorderStyle.Thin;

        var rowLabelFont = this.Workbook.CreateFont();
        rowLabelFont.Boldweight = (short)FontBoldWeight.Normal;
        rowLabelFont.FontHeightInPoints = 10;
        rowLabelFont.FontName = "calibri";


        headerLabelCellStyle.SetFont(headerLabelFont);

        rowLabelCellStyle.SetFont(rowLabelFont);

        var sheet = CreateExportDataTableSheetAndHeaderRow(exportData, sheetName, headerLabelCellStyle);
        var currentNPOIRowIndex = 2;
        var sheetCount = 1;

        for (var rowIndex = 0; rowIndex < exportData.Rows.Count; rowIndex++)
        {
            if (currentNPOIRowIndex >= MaximumNumberOfRowsPerSheet)
            {
                sheetCount++;
                currentNPOIRowIndex = 2;

                sheet = CreateExportDataTableSheetAndHeaderRow(exportData,
                                                               sheetName + " - " + sheetCount,
                                                               headerLabelCellStyle);
            }

            var row = sheet.CreateRow(currentNPOIRowIndex++);
            IDataFormat format = Workbook.CreateDataFormat();
            for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
            {
                var cell = row.CreateCell(colIndex);
                string formatvalue = GetDefaultDataFormat(exportData.Rows[rowIndex][colIndex].ToString());
                //rowLabelCellStyle.DataFormat = format.GetFormat(formatvalue);

                if (formatvalue == "0")
                {
                    cell.SetCellValue(
                        Convert.ToInt64(exportData.Rows[rowIndex][colIndex])
                        );
                }
                else if (formatvalue == "0.00")
                {
                    cell.SetCellValue(
                        Convert.ToDouble(exportData.Rows[rowIndex][colIndex])
                        );
                }
                else
                {

                    if (Convert.ToString(exportData.Rows[rowIndex][colIndex]).IndexOf("%") != -1)
                    {
                        cell.SetCellType(CellType.Numeric);
                        cell.SetCellValue(
                        Convert.ToString(exportData.Rows[rowIndex][colIndex])
                        );
                    }
                    else
                    {
                        cell.SetCellValue(
                        Convert.ToString(exportData.Rows[rowIndex][colIndex])
                        );
                    }
                }



                cell.CellStyle = rowLabelCellStyle;
                DataColumn column = exportData.Columns[colIndex];
                sheet.AutoSizeColumn(column.Ordinal);


            }
        }


    }

    public void PivotExportDataTableToWorkbook(DataTable exportData, string sheetName)
    {
        // Create the header row cell style
        var headerLabelCellStyle = this.Workbook.CreateCellStyle();
        headerLabelCellStyle.BorderBottom = BorderStyle.Thick;
        headerLabelCellStyle.BorderLeft = BorderStyle.Thick;
        headerLabelCellStyle.BorderRight = BorderStyle.Thick;
        headerLabelCellStyle.BorderTop = BorderStyle.Thick;
        headerLabelCellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkYellow.Index;
        headerLabelCellStyle.FillPattern = FillPattern.SolidForeground;
        headerLabelCellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;



        var headerLabelFont = this.Workbook.CreateFont();
        headerLabelFont.Boldweight = (short)FontBoldWeight.Bold;
        headerLabelFont.FontHeightInPoints = 12;
        headerLabelFont.FontName = "calibri";


        var rowLabelCellStyle = this.Workbook.CreateCellStyle();
        rowLabelCellStyle.BorderBottom = BorderStyle.Thin;
        rowLabelCellStyle.BorderLeft = BorderStyle.Thin;
        rowLabelCellStyle.BorderRight = BorderStyle.Thin;
        rowLabelCellStyle.BorderTop = BorderStyle.Thin;

        var rowLabelFont = this.Workbook.CreateFont();
        rowLabelFont.Boldweight = (short)FontBoldWeight.Normal;
        rowLabelFont.FontHeightInPoints = 10;
        rowLabelFont.FontName = "calibri";


        headerLabelCellStyle.SetFont(headerLabelFont);

        rowLabelCellStyle.SetFont(rowLabelFont);

        var sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));
        sheet.CreateRow(0);
        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 1));
        var row_m = sheet.CreateRow(0);
        var cell_m = row_m.CreateCell(0);
        cell_m.SetCellValue(sheetName);
        cell_m.CellStyle = headerLabelCellStyle;

        var currentNPOIRowIndex = 1;
        sheet.AutoSizeColumn(0);


        for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
        {

            var row = sheet.CreateRow(currentNPOIRowIndex++);
            IDataFormat format = Workbook.CreateDataFormat();

            var cell = row.CreateCell(0);
            cell.CellStyle = headerLabelCellStyle;

            cell.SetCellValue(
                   Convert.ToString(exportData.Columns[colIndex].ColumnName)
                   );

            cell = row.CreateCell(1);
            cell.CellStyle = rowLabelCellStyle;
            string formatvalue = GetDefaultDataFormat(exportData.Rows[0][colIndex].ToString());


            if (formatvalue == "0")
            {
                cell.SetCellValue(
                    Convert.ToInt64(exportData.Rows[0][colIndex])
                    );
            }
            else if (formatvalue == "0.00")
            {
                cell.SetCellValue(
                    Convert.ToDouble(exportData.Rows[0][colIndex])
                    );
            }
            else
            {

                if (Convert.ToString(exportData.Rows[0][colIndex]).IndexOf("%") != -1)
                {
                    cell.SetCellType(CellType.Numeric);
                    cell.SetCellValue(
                    Convert.ToString(exportData.Rows[0][colIndex])
                    );
                }
                else
                {
                    cell.SetCellValue(
                    Convert.ToString(exportData.Rows[0][colIndex])
                    );
                }
            }
            cell.CellStyle = rowLabelCellStyle;
            DataColumn column = exportData.Columns[0];
            sheet.AutoSizeColumn(column.Ordinal + 100);
        }


    }
    private string GetDefaultDataFormat(object value)
    {
        if (value == null)
        {
            return "General";
        }


        bool bool_out;
        byte byte_out;
        ushort ushort_out;
        short short_out;
        uint uint_out;
        int int_out;
        ulong ulong_out;
        long long_out;

        float float_out;
        double double_out;


        if (bool.TryParse(value.ToString(), out bool_out))
        {
            return "[=0]\"Yes\";[=1]\"No\"";
        }

        if (byte.TryParse(value.ToString(), out byte_out) || ushort.TryParse(value.ToString(), out ushort_out) ||
            short.TryParse(value.ToString(), out short_out) || uint.TryParse(value.ToString(), out uint_out) ||
            int.TryParse(value.ToString(), out int_out) || ulong.TryParse(value.ToString(), out ulong_out) ||
            long.TryParse(value.ToString(), out long_out)
            )
        {
            return "0";
        }

        if (float.TryParse(value.ToString(), out float_out) || double.TryParse(value.ToString(), out double_out))
        {
            return "0.00";
        }

        // strings and anything else should be text
        return "text";
    }

    public byte[] GetBytes()
    {
        using (var buffer = new MemoryStream())
        {
            this.Workbook.Write(buffer);
            return buffer.GetBuffer();
        }
    }

    public void Dispose()
    {
        if (this.Workbook != null)
            this.Workbook.Dispose();
    }
}