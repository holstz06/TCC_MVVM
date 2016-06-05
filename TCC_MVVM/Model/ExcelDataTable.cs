using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace TCC_MVVM.Model
{
    class ExcelDataTable
    {
        public DataTable Table { get; private set; } = new DataTable();

        /// <summary>
        /// Creates a data table based on the contents from the Excel file
        /// </summary>
        /// <param name="Path">
        /// The path to the Excel file
        /// </param>
        /// <param name="SheetName">
        /// The sheet name in the excel workbook
        /// </param>
        /// <returns>
        /// The datatable
        /// </returns>
        public DataTable GetData(string Path, string SheetName)
        {
            OleDbConnection connection 
                = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties=\"Excel 12.0;HDR=YES;\"");

            try
            {
                connection.Open();
                try
                {
                    OleDbDataAdapter adapter 
                        = new OleDbDataAdapter("SELECT * FROM [" + SheetName + "$]", connection);
                    adapter.Fill(Table);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            connection.Close();

            return Table;
        }
    }
}
