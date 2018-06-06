using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace BUD_Storage.ExcelTables
{
    class Excel
    {
        private string path = "";
        private _Application excel = new _Excel.Application();
        private Workbook wb;
        private Worksheet ws;

        public Excel(string path, int sheet)
        {
            this.path = path;
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[sheet];
        }

        public string ReadCell(int row, int column)
        {
            if (ws.Cells[row, column].Value2 != null)
            {
                return (ws.Cells[row, column].Value2).ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        public void CloseFile()
        {
            wb.Close();
        }
    }
}
