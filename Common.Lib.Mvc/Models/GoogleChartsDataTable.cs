using System.Collections.Generic;

namespace Common.Lib.MVC.Models
{
    public class GoogleChartsDataTable
    {
        public List<GoogleChartsDataTableColumn> cols { get; set; }
        public List<GoogleChartsDataTableRow> rows { get; set; }
        //The table-level p property is a map of custom values applied to the whole DataTable. These values can be of any JavaScript type. 
        //If your visualization supports any datatable-level properties, it will describe them; otherwise, this property will be ignored. 
        //Example: p:{className: 'myDataTable'}.
        public object p { get; set; }
    }

    public class GoogleChartsDataTableColumn
    {
        public string id { get; set; }
        public string label { get; set; }
        public string pattern { get; set; }
        public string type { get; set; }
    }

    public class GoogleChartsDataTableRow
    {
        public List<GoogleChartsDataTableRowInfo> c { get; set; }
    }

    public class GoogleChartsDataTableRowInfo
    {
        public object v { get; set; }
        public string f { get; set; }
    }
}
