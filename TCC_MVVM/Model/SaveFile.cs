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
        DataTable JobData { get; set; }
        DataTable RoomData { get; set; }
        DataTable StripData { get; set; }
        DataTable PanelData { get; set; }
        DataTable ShelfData { get; set; }
        DataTable AccessoryData { get; set; }

        readonly string DataSetName = "Proposal";
        readonly string JobDataName = "JobData";
        readonly string RoomDataName = "RoomData";
        readonly string StripDataName = "StripData";
        readonly string PanelDataName = "PanelData";
        readonly string ShelfDataName = "ShelfData";
        readonly string AccessoryDataName = "AccessoryData";

        public void Save(JobVM viewmodel)
        {
            DataSet = new DataSet(DataSetName);

            //===================================
            // Extract Job Information
            //===================================
            JobData = new DataTable(JobDataName);

            // Create Columns
            var jobProperties = viewmodel.Job.GetType().GetProperties();
            foreach(PropertyInfo property in jobProperties)
            {
                JobData.Columns.Add(property.Name, typeof(string));
            }

            // Populate rows
            JobData.Rows.Add();
            DataRow datarow = JobData.Rows[0];
            foreach(PropertyInfo property in jobProperties)
            {
                if(property.CanRead && property.GetIndexParameters().Length == 0)
                    datarow[property.Name] = property.GetValue(viewmodel.Job);
            }

            //===================================
            // Extract Room Information
            //===================================

            RoomData = new DataTable(RoomDataName);

            // Create columns
            var room = new Room();
            var roomProperties = room.GetType().GetProperties();
            foreach (PropertyInfo property in roomProperties)
            {
                RoomData.Columns.Add(property.Name, typeof(string));
            }

            // Populate rows
            int currentRow = 0;
            foreach (RoomVM roomvm in viewmodel.Rooms)
            {
                RoomData.Rows.Add();
                DataRow roomDatarow = RoomData.Rows[currentRow];
                foreach (PropertyInfo property in roomProperties)
                {
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                        roomDatarow[property.Name] = property.GetValue(roomvm.Room);
                }
                currentRow++;
            }

            //===================================
            // Extract Strip Information
            //===================================
            StripData = new DataTable(StripDataName);
            var stripmodel = new Strip();
            var stripPropertes = stripmodel.GetType().GetProperties();
            foreach(PropertyInfo property in stripPropertes)
            {
                StripData.Columns.Add(property.Name, typeof(string));
            }

            currentRow = 0;
            foreach(RoomVM roomvm in viewmodel.Rooms)
            {
                foreach(Strip strip in roomvm.StripViewModel.Strips)
                {
                    StripData.Rows.Add();
                    DataRow stripDatarow = StripData.Rows[currentRow];
                    foreach(PropertyInfo property in stripPropertes)
                    {
                        if (property.CanRead && property.GetIndexParameters().Length == 0)
                            stripDatarow[property.Name] = property.GetValue(strip);
                    }
                    currentRow++;
                }
            }

            //===================================
            // Extract Panel Information
            //===================================
            PanelData = new DataTable(PanelDataName);
            var panelmodel = new Panel();
            var panelProperties = panelmodel.GetType().GetProperties();
            foreach(PropertyInfo property in panelProperties)
            {
                PanelData.Columns.Add(property.Name, typeof(string));
            }

            currentRow = 0;
            foreach(RoomVM roomvm in viewmodel.Rooms)
            {
                foreach(Panel panel in roomvm.PanelViewModel.Panels)
                {
                    PanelData.Rows.Add();
                    DataRow panelDatarow = PanelData.Rows[currentRow];
                    foreach(PropertyInfo property in panelProperties)
                    {
                        if (property.CanRead && property.GetIndexParameters().Length == 0)
                            panelDatarow[property.Name] = property.GetValue(panel);
                    }
                    currentRow++;
                }
            }

            //===================================
            // Extract Shelf Information
            //===================================
            ShelfData = new DataTable(ShelfDataName);
            var shelfmodel = new Shelf();
            var shelfProperties = shelfmodel.GetType().GetProperties();
            foreach (PropertyInfo property in shelfProperties)
            {
                ShelfData.Columns.Add(property.Name, typeof(string));
            }

            currentRow = 0;
            foreach (RoomVM roomvm in viewmodel.Rooms)
            {
                foreach (Shelf shelf in roomvm.ShelfViewModel.Shelves)
                {
                    ShelfData.Rows.Add();
                    DataRow shelfDatarow = ShelfData.Rows[currentRow];
                    foreach (PropertyInfo property in shelfProperties)
                    {
                        if (property.CanRead && property.GetIndexParameters().Length == 0)
                            shelfDatarow[property.Name] = property.GetValue(shelf);
                    }
                    currentRow++;
                }
            }

            //===================================
            // Extract Accessory Information
            //===================================
            AccessoryData = new DataTable(AccessoryDataName);
            var accessorymodel = new Accessory();
            var accessoryProperties = accessorymodel.GetType().GetProperties();
            foreach (PropertyInfo property in accessoryProperties)
            {
                AccessoryData.Columns.Add(property.Name, typeof(string));
            }

            currentRow = 0;
            foreach (RoomVM roomvm in viewmodel.Rooms)
            {
                foreach (Accessory accessory in roomvm.AccessoryViewModel.Accessories)
                {
                    AccessoryData.Rows.Add();
                    DataRow accessoryDatarow = AccessoryData.Rows[currentRow];
                    foreach (PropertyInfo property in accessoryProperties)
                    {
                        if (property.CanRead && property.GetIndexParameters().Length == 0)
                            accessoryDatarow[property.Name] = property.GetValue(accessory);
                    }
                    currentRow++;
                }
            }

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
            DataSet = new DataSet(DataSetName);
            DataSet.ReadXml("Tyler Holstead.xml");
            JobData = DataSet.Tables[JobDataName];
            RoomData = DataSet.Tables[RoomDataName];
            StripData = DataSet.Tables[StripDataName];
            PanelData = DataSet.Tables[PanelDataName];
            ShelfData = DataSet.Tables[ShelfDataName];
            AccessoryData = DataSet.Tables[AccessoryDataName];

            var newJob = new JobVM();

            //===================================
            // Import Job Information
            //===================================
            var job = new Job();
            foreach (DataColumn propertyName in JobData.Columns)
            {
                var propertyValue = JobData.Rows[0][propertyName].ToString();
                job.SetProperty(propertyName.ColumnName, propertyValue);
            }
            newJob.Job = job;

            //===================================
            // Import Room Information
            //===================================
            if (RoomData != null)
            {
                for (int row = 0; row < RoomData.Rows.Count; row++)
                {
                    newJob.AddRoom();
                    RoomVM roomvm = newJob.Rooms[row];
                    foreach (string propertyName in roomvm.Room.Properties)
                    {
                        if(RoomData.Columns[propertyName] != null)
                            roomvm.Room.SetProperty(propertyName, RoomData.Rows[row][propertyName].ToString());
                    }
                }
            }

            //===================================
            // Import Strip Information
            //===================================
            if(PanelData != null)
            {
                for (int row = 0; row < StripData.Rows.Count; row++)
                {
                    var strip = new Strip();
                    foreach (string propertyName in strip.Properties)
                    {
                        strip.SetProperty(propertyName, StripData.Rows[row][propertyName].ToString());
                    }

                    foreach (RoomVM roomvm in newJob.Rooms)
                    {
                        if (roomvm.Room.RoomNumber == strip.RoomNumber)
                            roomvm.StripViewModel.AddStrip(strip);
                    }
                }
            }
            

            //===================================
            // Import Panel Information
            //===================================
            if(PanelData != null)
            {
                for (int row = 0; row < PanelData.Rows.Count; row++)
                {
                    var panel = new Panel();
                    foreach (string propertyName in panel.Properties)
                    {
                        panel.SetProperty(propertyName, PanelData.Rows[row][propertyName].ToString());
                    }

                    foreach (RoomVM roomvm in newJob.Rooms)
                    {
                        if (roomvm.Room.RoomNumber == panel.RoomNumber)
                            roomvm.PanelViewModel.Add(panel);
                    }
                }
            }

            //===================================
            // Import Shelf Information
            //===================================
            if(ShelfData != null)
            {
                for (int row = 0; row < ShelfData.Rows.Count; row++)
                {
                    var shelf = new Shelf();
                    foreach (string propertyName in shelf.Properties)
                    {
                        if(ShelfData.Columns[propertyName] != null)
                            shelf.SetProperty(propertyName, ShelfData.Rows[row][propertyName].ToString());
                    }

                    foreach (RoomVM roomvm in newJob.Rooms)
                    {
                        if (roomvm.Room.RoomNumber == shelf.RoomNumber)
                            roomvm.ShelfViewModel.Add(shelf);
                    }
                }
            }

            //===================================
            // Import Accessory Information
            //===================================
            if(AccessoryData != null)
            {
                for (int row = 0; row < AccessoryData.Rows.Count; row++)
                {
                    var accessory = new Accessory();
                    foreach (string propertyName in accessory.Properties)
                    {
                        if (AccessoryData.Columns[propertyName] != null)
                            accessory.SetProperty(propertyName, AccessoryData.Rows[row][propertyName].ToString());
                    }

                    foreach (RoomVM roomvm in newJob.Rooms)
                    {
                        if (roomvm.Room.RoomNumber == accessory.RoomNumber)
                            roomvm.AccessoryViewModel.Add(accessory);
                    }
                }
            }

            return newJob;
        }
    }
}
