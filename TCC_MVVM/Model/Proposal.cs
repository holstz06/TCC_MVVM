using System;
using System.Windows;
using TCC_MVVM.ViewModel.Job;
using TCC_MVVM.ViewModel.Room;

namespace TCC_MVVM.Model
{
    class Proposal
    {
        public const int NAME_COL = 1;
        public const int WIDTH_COL = 2;
        public const int HEIGHT_COL = 3;
        public const int DEPTH_COL = 4;
        public const int LENGTH_COL = 5;
        public const int SHELFTYPE_COL = 6;
        public const int COLOR_COL = 7;
        public const int QUANTITY_COL = 8;
        public const int PRICE_COL = 9;

        public string ProposalTemplatePath { get; set; }
        public string ProposalPath { get; set; }

        public string ItemListTemplatePath { get; set; }
        public string ItemListPath { get; set; }

        Microsoft.Office.Interop.Excel.Application ExcelApp;
        Microsoft.Office.Interop.Excel._Workbook WorkBook;
        Microsoft.Office.Interop.Excel._Worksheet WorkSheet;
        object MissingValue = System.Reflection.Missing.Value;

        readonly JobVM ViewModel;

        public Proposal(JobVM ViewModel)
        {
            this.ViewModel = ViewModel;
        }

        /// <summary>
        /// Opens the Excel application
        /// </summary>
        private void OpenExcel()
        {
            try
            {
                // Start Excel Application
                ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                ExcelApp.Visible = true;

                // Get a new workbook
                WorkBook = (Microsoft.Office.Interop.Excel._Workbook)(ExcelApp.Workbooks.Add(""));
                WorkSheet = (Microsoft.Office.Interop.Excel._Worksheet)WorkBook.ActiveSheet;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                CloseExcel();
            }
        }

        /// <summary>
        /// Closes the Excel application
        /// </summary>
        public void CloseExcel()
        {
            WorkBook.SaveAs(ItemListPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            WorkBook.Close();
        }

        /// <summary>
        /// Creates a new proposal
        /// </summary>
        public void CreateProposal()
        {

        }

        /// <summary>
        /// Loads and Excel spreadsheet with the items list
        /// </summary>
        public void LoadItemList()
        {
            OpenExcel();
            int rowCount = 25;
            int colCount = 25;
            int roomnumber = -1;
            var currentRoom = new RoomVM(null, 0);

            for(int row = 0; row < rowCount; row++)
            {
                // Import job information
                if(WorkSheet.Cells[row, 1] == "Job")
                    ViewModel.Job.SetProperty(WorkSheet.Cells[row, 2], WorkSheet.Cells[row, 3]);

                if(WorkSheet.Cells[row, 1] == "Room")
                {
                    roomnumber++;
                    ViewModel.AddRoom(WorkSheet.Cells[row, 3]);
                    currentRoom = ViewModel.Rooms[roomnumber];
                }

                if(WorkSheet.Cells[row, 1] == "Strip")
                {
                    var strip = new Strip
                    {
                        RoomNumber = roomnumber,
                        Color = WorkSheet.Cells[row, COLOR_COL],
                        Length = WorkSheet.Cells[row, LENGTH_COL]
                    };
                    currentRoom.StripViewModel.Add(strip);
                } 
            }

            CloseExcel();
        }

        /// <summary>
        /// Creates an Excel spreadsheet with the items list
        /// </summary>
        public void CreateItemList()
        {
            OpenExcel();

            int curRow = 1;

            // Export the job information
            foreach(string property in ViewModel.Job.Properties)
            {
                WorkSheet.Cells[curRow, 1] = "Job";
                WorkSheet.Cells[curRow, 2] = property;
                //WorkSheet.Cells[curRow, 3] = ViewModel.Job.GetPropertyValue(property);
                curRow++;
            }

            // Create each room
            foreach (RoomVM roomVM in ViewModel.Rooms)
            {
                curRow++;
                WorkSheet.Cells[curRow, 1] = "Room";
                WorkSheet.Cells[curRow, 2] = roomVM.Room.RoomName;
                WorkSheet.Cells[curRow, PRICE_COL] = roomVM.TotalPrice.ToString();
                curRow++;

                // Create each strip
                foreach(Strip strip in roomVM.StripViewModel.Strips)
                {
                    WorkSheet.Cells[curRow, NAME_COL] = "Strip";
                    WorkSheet.Cells[curRow, LENGTH_COL] = strip.Length;
                    WorkSheet.Cells[curRow, COLOR_COL] = strip.Color;
                    WorkSheet.Cells[curRow, PRICE_COL] = strip.Price;
                    curRow++;
                }

                // Create each panel
                foreach(Panel panel in roomVM.PanelViewModel.Panels)
                {
                    WorkSheet.Cells[curRow, NAME_COL] = "Panel";
                    WorkSheet.Cells[curRow, HEIGHT_COL] = panel.SizeHeight;
                    WorkSheet.Cells[curRow, DEPTH_COL] = panel.SizeDepth;
                    WorkSheet.Cells[curRow, COLOR_COL] = panel.Color;
                    WorkSheet.Cells[curRow, QUANTITY_COL] = panel.Quantity;
                    WorkSheet.Cells[curRow, PRICE_COL] = panel.Price;
                    curRow++;
                }

                // Create each shelf
                foreach (Shelf shelf in roomVM.ShelfViewModel.Shelves)
                {
                    WorkSheet.Cells[curRow, NAME_COL] = "Shelf";
                    WorkSheet.Cells[curRow, WIDTH_COL] = shelf.SizeWidth;
                    WorkSheet.Cells[curRow, DEPTH_COL] = shelf.SizeDepth;
                    WorkSheet.Cells[curRow, COLOR_COL] = shelf.Color;
                    WorkSheet.Cells[curRow, SHELFTYPE_COL] = shelf.ShelfType;
                    WorkSheet.Cells[curRow, QUANTITY_COL] = shelf.Quantity;
                    WorkSheet.Cells[curRow, PRICE_COL] = shelf.Price;
                    curRow++;
                }

                // TODO: Create each accessory
                foreach (Accessory accessory in roomVM.AccessoryViewModel.Accessories)
                {
                    WorkSheet.Cells[curRow, NAME_COL] = accessory.Name;
                    WorkSheet.Cells[curRow, WIDTH_COL] = accessory.Width;
                    WorkSheet.Cells[curRow, DEPTH_COL] = accessory.Depth;
                    WorkSheet.Cells[curRow, HEIGHT_COL] = accessory.Height;
                    WorkSheet.Cells[curRow, COLOR_COL] = accessory.Color;
                    WorkSheet.Cells[curRow, LENGTH_COL] = accessory.Length;
                    WorkSheet.Cells[curRow, QUANTITY_COL] = "1";
                    WorkSheet.Cells[curRow, PRICE_COL] = accessory.Price;
                }
            }
            CloseExcel();
        }
    }
}
