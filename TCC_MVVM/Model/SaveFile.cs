using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TCC_MVVM.ViewModel.Job;
using TCC_MVVM.ViewModel.Room;
using TCC_MVVM;
using System.Collections.ObjectModel;
using System.Reflection;

namespace TCC_MVVM.Model
{
    public class SaveFile
    {
        public DataSet DataSet { get; set; }
        private DataTable JobData { get; set; }
        private DataTable RoomData { get; set; }
        private DataTable StripData { get; set; }
        private DataTable PanelData { get; set; }
        private DataTable ShelfData { get; set; }
        private DataTable AccessoryData { get; set; }

        private readonly string DataSetName = "Proposal";
        private readonly string JobDataName = "JobData";
        private readonly string RoomDataName = "RoomData";
        private readonly string StripDataName = "StripData";
        private readonly string PanelDataName = "PanelData";
        private readonly string ShelfDataName = "ShelfData";
        private readonly string AccessoryDataName = "AccessoryData";

        public void Save(JobVM viewmodel)
        {
            DataSet = new DataSet(DataSetName);

            //===================================
            // Extract Job Information
            //===================================
            JobData = new DataTable(JobDataName);

            // Create Columns
            PropertyInfo[] properties = viewmodel.Job.GetType().GetProperties();
            foreach(PropertyInfo property in properties)
            {
                JobData.Columns.Add(property.Name, typeof(string));
            }

            // Populate rows
            JobData.Rows.Add();
            DataRow datarow = JobData.Rows[0];
            foreach(PropertyInfo property in properties)
            {
                if(property.CanRead && property.GetIndexParameters().Length == 0)
                    datarow[property.Name] = property.GetValue(viewmodel.Job);
            }

            //===================================
            // Extract Room Information
            //===================================

            RoomData = new DataTable(RoomDataName);

            // Create columns
            Room room = new Room();
            properties = room.GetType().GetProperties();
            foreach(PropertyInfo property in properties)
            {
                RoomData.Columns.Add(property.Name, typeof(string));
            }

            //RoomData.Columns.Add("RoomNumber", typeof(int));
            //RoomData.Columns.Add("RoomName", typeof(string));
            //RoomData.Columns.Add("RoomColor", typeof(string));
            //RoomData.Columns.Add("StripColor", typeof(string));
            //RoomData.Columns.Add("ShelvingDepth", typeof(string));

            // Populate rows
            int currentRow = 0;
            foreach (RoomVM roomvm in viewmodel.Rooms)
            {
                RoomData.Rows.Add();
                datarow = RoomData.Rows[currentRow++];
                foreach(PropertyInfo property in properties)
                {
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                        datarow[property.Name] = property.GetValue(room);

                }
            }

            //===================================
            // Extract Strip Information
            //===================================

            StripData = new DataTable(StripDataName);

            // Create columns
            StripData.Columns.Add("RoomNumber", typeof(int));
            StripData.Columns.Add("Color", typeof(string));
            StripData.Columns.Add("Length", typeof(double));
            StripData.Columns.Add("Price", typeof(decimal));

            // Populate rows
            foreach (RoomVM roomvm in viewmodel.Rooms)
            {
                foreach(Strip strip in roomvm.StripViewModel.Strips)
                {
                    StripData.Rows.Add(strip);
                    //StripData.Rows.Add(strip.RoomNumber, strip.Color, strip.Length, strip.Price);
                }
            }

            //===================================
            // Extract Panel Information
            //===================================

            PanelData = new DataTable(PanelDataName);

            // Create columns
            PanelData.Columns.Add("RoomNumber", typeof(int));
            PanelData.Columns.Add("SizeHeight", typeof(string));
            PanelData.Columns.Add("SizeDepth", typeof(string));
            PanelData.Columns.Add("Color", typeof(string));
            PanelData.Columns.Add("IsHutch", typeof(bool));
            PanelData.Columns.Add("Quantity", typeof(int));
            PanelData.Columns.Add("Price", typeof(decimal));

            // Populate rows
            foreach(RoomVM roomvm in viewmodel.Rooms)
            {
                foreach(Panel panel in roomvm.PanelViewModel.Panels)
                {
                    PanelData.Rows.Add(panel.RoomNumber, panel.SizeHeight, panel.SizeDepth, panel.Color, panel.IsHutch, panel.Quantity, panel.Price);
                }
            }

            //===================================
            // Extract Shelf Information
            //===================================
            ShelfData = new DataTable(ShelfDataName);

            // Create Columns
            ShelfData.Columns.Add("RoomNumber", typeof(int));
            ShelfData.Columns.Add("SizeWidth", typeof(string));
            ShelfData.Columns.Add("SizeDepth", typeof(string));
            ShelfData.Columns.Add("Color", typeof(string));
            ShelfData.Columns.Add("ShelfType", typeof(ShelfType));
            ShelfData.Columns.Add("Quantity", typeof(int));
            ShelfData.Columns.Add("Price", typeof(decimal));

            ShelfData.Columns.Add("CamPostColor", typeof(string));
            ShelfData.Columns.Add("CamPostQuantity", typeof(int));
            ShelfData.Columns.Add("CamPostPrice", typeof(decimal));

            ShelfData.Columns.Add("HasFence", typeof(bool));
            ShelfData.Columns.Add("FenceColor", typeof(string));
            ShelfData.Columns.Add("FencePrice", typeof(decimal));

            foreach(RoomVM roomvm in viewmodel.Rooms)
            {
                foreach(Shelf shelf in roomvm.ShelfViewModel.Shelves)
                {
                    ShelfData.Rows.Add(shelf.RoomNumber, shelf.SizeWidth, shelf.SizeDepth, shelf.Color, shelf.ShelfType,
                        shelf.Quantity, shelf.Price, shelf.CamPostColor, shelf.CamPostQuantity, shelf.CamPostPrice, shelf.HasFence,
                        shelf.FenceColor, shelf.FencePrice);
                }
            }


            //===================================
            // Extract Accessory Information
            //===================================
            AccessoryData = new DataTable(AccessoryDataName);

            // Create Columns

            //===================================
            // Write the XML information
            //===================================

            DataSet.Tables.Add(JobData);
            DataSet.Tables.Add(RoomData);
            DataSet.Tables.Add(StripData);
            DataSet.Tables.Add(PanelData);
            DataSet.Tables.Add(ShelfData);
            DataSet.Tables.Add(AccessoryData);

            try
            {
                DataSet.WriteXml(viewmodel.Job.FullName + ".xml");
                MessageBox.Show("Proposal Created Successfully");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }

        public JobVM Load()
        {
            // Read in the xml
            DataSet = new DataSet(DataSetName);
            DataSet.ReadXml("Tyler Holstead.xml");
            JobData = DataSet.Tables[JobDataName];
            RoomData = DataSet.Tables[RoomDataName];
            StripData = DataSet.Tables[StripDataName];
            PanelData = DataSet.Tables[PanelDataName];
            ShelfData = DataSet.Tables[ShelfDataName];

            JobVM newJob = new JobVM();

            //===================================
            // Import Job Information
            //===================================

            // Read the job information from
            Job job = new Job();
            foreach(DataColumn propertyName in JobData.Columns)
            {
                string propertyValue = JobData.Rows[0][propertyName].ToString();
                job.SetProperty(propertyName.ColumnName, propertyValue);
            }
            newJob.Job = job;

            //===================================
            // Import Room Information
            //===================================

            ObservableCollection<RoomVM> rooms = new ObservableCollection<RoomVM>();
            foreach(DataRow row in RoomData.Rows)
            {
                RoomVM roomvm = new RoomVM(
                    row["RoomName"].ToString(),
                    int.Parse(row["RoomNumber"].ToString())
                );
                roomvm.Room.RoomColor = row["RoomColor"].ToString();
                roomvm.Room.StripColor = row["StripColor"].ToString();
                roomvm.Room.ShelvingDepth = row["ShelvingDepth"].ToString();

                rooms.Add(roomvm);
            }
            newJob.Rooms = rooms;

            //===================================
            // Import Strip Information
            //===================================
            if(StripData != null)
            {
                foreach (DataRow row in StripData.Rows)
                {
                    Strip strip = new Strip()
                    {
                        RoomNumber = int.Parse(row["RoomNumber"].ToString()),
                        Color = row["Color"].ToString(),
                        Length = double.Parse(row["Length"].ToString())
                    };

                    // Place the strip in the correct room
                    foreach (RoomVM room in newJob.Rooms)
                    {
                        if (room.Room.RoomNumber == strip.RoomNumber)
                            room.StripViewModel.Add(strip);
                    }
                }
            }

            //===================================
            // Import Panel Information
            //===================================
            if(PanelData != null)
            {
                foreach (DataRow row in PanelData.Rows)
                {
                    Panel panel = new Panel()
                    {
                        RoomNumber = int.Parse(row["RoomNumber"].ToString()),
                        SizeDepth = row["SizeDepth"].ToString(),
                        SizeHeight = row["SizeHeight"].ToString(),
                        Color = row["Color"].ToString(),
                        IsHutch = bool.Parse(row["IsHutch"].ToString()),
                        Quantity = int.Parse(row["Quantity"].ToString())
                    };

                    // Place the panel in the correct room
                    foreach (RoomVM room in newJob.Rooms)
                    {
                        if (room.Room.RoomNumber == panel.RoomNumber)
                            room.PanelViewModel.Add(panel);
                    }
                }
            }

            //===================================
            // Import Shelf Information
            //===================================
            if(ShelfData != null)
            {
                //foreach(DataRow row in ShelfData.Rows)
                //{
                //    Shelf shelf = new Shelf()
                //    {
                //        RoomNumber = int.Parse(row["RoomNumber"].ToString()),
                //        SizeDepth = row["SizeDepth"].ToString(),
                //        SizeWidth = row["SizeWidth"].ToString(),
                //        Color = row["Color"].ToString(),
                //        ShelfType = (ShelfType)row["ShelfType"],
                //        Quantity = int.Parse(row["Quantity"].ToString())
                //    };

                //    // Place the shelf in the correct room
                //    foreach (RoomVM room in newJob.Rooms)
                //    {
                //        if (room.Room.RoomNumber == shelf.RoomNumber)
                //            room.ShelfViewModel.Add(shelf);
                //    }
                //}
            }

            return newJob;
        }
    }
}
