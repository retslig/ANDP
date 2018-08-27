using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;

namespace Common.Lib.Utility
{
    public static class ExcelHelper
    {
        public static Sheets RetrieveAllExcelSheets(Stream stream)
        {
            Sheets theSheets = null;

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                theSheets = wbPart.Workbook.Sheets;
            }
            return theSheets;
        }

        public static WorksheetPart RetrieveExcelWorksheetPartByName(Stream stream, string sheetName)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                // Find the sheet with the supplied name, and then use that Sheet
                // object to retrieve a reference to the appropriate worksheet.
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();

                //Reference to Excel worksheet with order data.
                return (WorksheetPart)document.WorkbookPart.GetPartById(theSheet.Id);
            }
        }

        //public static WorksheetPart RetrieveExcelWorkSheetPart(Sheet sheet, string sheetName)
        //{
        //    //get worksheet based on name
        //    var firstOrDefault = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => sheetName.Equals(s.Name));
        //    if (firstOrDefault != null)
        //        return (WorksheetPart)workbookPart.GetPartById(firstOrDefault.Id);

        //    return null;
        //}

        //public static Row RetrieveExcelRow(WorkbookPart workbookPart, string sheetName)
        //{
        //    //get worksheet based on name
        //    var firstOrDefault = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => sheetName.Equals(s.Name));
        //    if (firstOrDefault != null)
        //        return (WorksheetPart)workbookPart.GetPartById(firstOrDefault.Id);

        //    return null;
        //}

        //public static Cell RetrieveExcelCell(WorkbookPart workbookPart, string sheetName)
        //{
        //    //get worksheet based on name
        //    var firstOrDefault = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => sheetName.Equals(s.Name));
        //    if (firstOrDefault != null)
        //        return (WorksheetPart)workbookPart.GetPartById(firstOrDefault.Id);

        //    return null;
        //}

        ///// <summary>
        ///// Helper method for creating a list of customers 
        ///// from an Excel worksheet.
        ///// </summary>
        //public static List<T> FillObject<T>(Stream stream)
        //{
        //    using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
        //    {
        //        WorkbookPart wbPart = document.WorkbookPart;
        //        theSheets = wbPart.Workbook.Sheets;
        //    }

        //    //Initialize the customer list.
        //    List<T> result = new List<T>();

        //    //LINQ query to skip first row with column names.
        //    IEnumerable<Row> dataRows =
        //      from row in worksheet.Descendants<Row>()
        //      where row.RowIndex > 1
        //      select row;

        //    foreach (Row row in dataRows)
        //    {
        //        //LINQ query to return the row's cell values.
        //        //Where clause filters out any cells that do not contain a value.
        //        //Select returns the value of a cell unless the cell contains
        //        //  a Shared String.
        //        //If the cell contains a Shared String, its value will be a 
        //        //  reference id which will be used to look up the value in the 
        //        //  Shared String table.
        //        IEnumerable<String> textValues =
        //          from cell in row.Descendants<Cell>()
        //          where cell.CellValue != null
        //          select
        //            (cell.DataType != null
        //              && cell.DataType.HasValue
        //              && cell.DataType == CellValues.SharedString
        //            ? sharedString.ChildElements[
        //              int.Parse(cell.CellValue.InnerText)].InnerText
        //            : cell.CellValue.InnerText)
        //          ;

        //        //Check to verify the row contained data.
        //        if (textValues.Count() > 0)
        //        {
        //            //Create a customer and add it to the list.
        //            var textArray = textValues.ToArray();
        //            Customer customer = new Customer();
        //            customer.Name = textArray[0];
        //            customer.City = textArray[1];
        //            customer.State = textArray[2];
        //            result.Add(customer);
        //        }
        //        else
        //        {
        //            //If no cells, then you have reached the end of the table.
        //            break;
        //        }
        //    }

        //    //Return populated list of customers.
        //    return result;
        //}


        public static string RetrieveExcelCellValue(Stream stream, string sheetName, string addressName)
        {
            string value = null;

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;

                // Find the sheet with the supplied name, and then use that Sheet
                // object to retrieve a reference to the appropriate worksheet.
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                  Where(s => s.Name == sheetName).FirstOrDefault();

                if (theSheet == null)
                {
                    throw new ArgumentException("sheetName");
                }

                // Retrieve a reference to the worksheet part, and then use its 
                // Worksheet property to get a reference to the cell whose 
                // address matches the address you supplied:
                WorksheetPart wsPart =
                  (WorksheetPart)(wbPart.GetPartById(theSheet.Id));
                Cell theCell = wsPart.Worksheet.Descendants<Cell>().
                  Where(c => c.CellReference == addressName).FirstOrDefault();

                // If the cell does not exist, return an empty string:
                if (theCell != null)
                {
                    value = theCell.InnerText;

                    // If the cell represents a numeric value, you are done. 
                    // For dates, this code returns the serialized value that 
                    // represents the date. The code handles strings and Booleans
                    // individually. For shared strings, the code looks up the 
                    // corresponding value in the shared string table. For Booleans, 
                    // the code converts the value into the words TRUE or FALSE.
                    if (theCell.DataType != null)
                    {
                        switch (theCell.DataType.Value)
                        {
                            case CellValues.SharedString:
                                // For shared strings, look up the value in the shared 
                                // strings table.
                                var stringTable = wbPart.
                                  GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                                // If the shared string table is missing, something is 
                                // wrong. Return the index that you found in the cell.
                                // Otherwise, look up the correct text in the table.
                                if (stringTable != null)
                                {
                                    value = stringTable.SharedStringTable.
                                      ElementAt(int.Parse(value)).InnerText;
                                }
                                break;

                            case CellValues.Boolean:
                                switch (value)
                                {
                                    case "0":
                                        value = "FALSE";
                                        break;
                                    default:
                                        value = "TRUE";
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            return value;
        }

        public static List<Row> RetrieveExcelRows(Stream stream)
        {
            IEnumerable<Row> dataRows = new List<Row>();
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                foreach (var part in wbPart.WorksheetParts)
                {
                    dataRows =
                      from row in part.Worksheet.Descendants<Row>()
                      where row.RowIndex > 1
                      select row;
                }

            }
            return dataRows.ToList();
        }

        public static List<object> RetrieveExcelCellsFromRow(Row row, SharedStringTable sharedStrings, List<CellFormats> cellFormats)
        {
            object obj = null;
            List<object> list = new List<object>();
            foreach (var theCell in row.Descendants<Cell>())
            {
                // If the cell represents a numeric value, you are done. 
                // For dates, this code returns the serialized value that 
                // represents the date. The code handles strings and Booleans
                // individually. For shared strings, the code looks up the 
                // corresponding value in the shared string table. For Booleans, 
                // the code converts the value into the words TRUE or FALSE.
                string value = theCell.InnerText;
                if (theCell.DataType != null && theCell.DataType.HasValue)
                {
                    switch (theCell.DataType.Value)
                    {
                        case CellValues.Number:
                            int intTemp = int.TryParse(value, out intTemp) ? intTemp : 0;
                            obj = intTemp;
                            break;
                        case CellValues.Date:
                            DateTime dateTimeTemp = DateTime.TryParse(value, out dateTimeTemp) ? dateTimeTemp : new DateTime();
                            obj = dateTimeTemp;
                            break;
                        case CellValues.SharedString:
                            // If the shared string table is missing, something is 
                            // wrong. Return the index that you found in the cell.
                            // Otherwise, look up the correct text in the table.
                            if (sharedStrings != null)
                            {
                                obj = sharedStrings.ElementAt(int.Parse(value)).InnerText;
                            }
                            break;

                        case CellValues.Boolean:
                            switch (value)
                            {
                                case "0":
                                    obj = false;
                                    break;
                                default:
                                    obj = true;
                                    break;
                            }
                            break;
                        default:
                            obj = value;
                            break;
                    }
                    list.Add(obj);
                }
                else if (theCell.StyleIndex != null && theCell.StyleIndex.HasValue)
                {
                    // look up the style used for the cell
                    int formatStyleIndex = Convert.ToInt32(theCell.StyleIndex.Value);
                    CellFormat cf = cellFormats[0].Descendants<CellFormat>().ToList()[formatStyleIndex];

                    switch (cf.NumberFormatId.Value)
                    {
                        case 14:
                            double cellValue = double.TryParse(value, out cellValue) ? cellValue : 0;
                            if (cellValue > 59)
                                cellValue -= 1; //Excel/Lotus 29/2/1900 excel thinks there was a leap year in 1900.

                            obj = new DateTime(1900, 12, 31).AddDays(cellValue);
                            break;
                        default:
                            obj = value;
                            break;
                    }

                    list.Add(obj);
                }
                else
                    list.Add("");
            }
            return list;
        }

        //NumberFormatId Table
        //0	General
        //1	0
        //2	0.00
        //3	#,##0
        //4	#,##0.00
        //9	0%
        //10	0.00%
        //11	0.00E+00
        //12	# ?/?
        //13	# ??/??
        //14	d/m/yyyy
        //15	d-mmm-yy
        //16	d-mmm
        //17	mmm-yy
        //18	h:mm tt
        //19	h:mm:ss tt
        //20	H:mm
        //21	H:mm:ss
        //22	m/d/yyyy H:mm
        //37	#,##0 ;(#,##0)
        //38	#,##0 ;[Red](#,##0)
        //39	#,##0.00;(#,##0.00)
        //40	#,##0.00;[Red](#,##0.00)
        //45	mm:ss
        //46	[h]:mm:ss
        //47	mmss.0
        //48	##0.0E+0
        //49	@

        public static List<Row> RetrieveExcelRowsFromWorkSheetName(Stream stream, string workSheetName, out SharedStringTable sharedStrings, out List<CellFormats> cellFormats)
        {
            IEnumerable<Row> dataRows = new List<Row>();
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
            {
                Workbook workBook = document.WorkbookPart.Workbook;
                IEnumerable<Sheet> workSheets = workBook.Descendants<Sheet>();
                sharedStrings = document.WorkbookPart.SharedStringTablePart.SharedStringTable;
                cellFormats = new List<CellFormats>(document.WorkbookPart.WorkbookStylesPart.Stylesheet.Descendants<CellFormats>());

                //Reference to Excel worksheet with order data.
                string id = workSheets.First(s => s.Name == workSheetName).Id;
                WorksheetPart sheet = (WorksheetPart)document.WorkbookPart.GetPartById(id);

                dataRows = from row in sheet.Worksheet.Descendants<Row>()
                           where row.RowIndex > 0
                           select row;

            }
            return dataRows.ToList();
        }


        public static void WriteCsv<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(",", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(",", props.Select(p => p.GetValue(item, null))));
                }
            }
        }
    
    }
}
