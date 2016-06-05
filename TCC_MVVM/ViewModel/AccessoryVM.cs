using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using TCC_MVVM.Model;

namespace TCC_MVVM.ViewModel
{
    public class AccessoryVM : INotifyPropertyChanged
    {
        private DataTable AccessoryData;
        public int TotalPrice { get; set; }

        /// <summary>
        /// Collection of accessories
        /// </summary>
        public ObservableCollection<Accessory> Accessories { get; set; } 
            = new ObservableCollection<Accessory>();

        
        private ICommand _RemoveCommand;

        /// <summary>
        /// Removes an accessory from the collection
        /// </summary>
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Accessory)param));
                return _RemoveCommand;
            }
        }

        /// <summary>
        /// Creates new instance of an accessory view model
        /// </summary>
        /// <param name="ExcelFilePath">
        /// The path to the Excel file
        /// </param>
        public AccessoryVM(string ExcelFilePath)
        {
            ExcelDataTable Data = new ExcelDataTable();
            AccessoryData = Data.GetData(ExcelFilePath, "Master");
        }
        
        /// <summary>
        /// Adds and accessory to the collection
        /// </summary>
        /// <param name="RoomNumber">
        /// The room number this accessory belongs to
        /// </param>
        public void Add(int RoomNumber)
        {
            Accessory accessory = new Accessory()
            {
                Accessories = new ObservableCollection<string>(GetAccessories()),
                RoomNumber = RoomNumber
            };

            accessory.PropertyChanged += Accessory_PropertyChanged;
            Accessories.Add(accessory);
        }

        /// <summary>
        /// Remove accessory from the collection
        /// </summary>
        /// <param name="AccessoryModel"></param>
        public void Remove(Accessory AccessoryModel)
        {
            if (Accessories.Contains(AccessoryModel))
                Accessories.Remove(AccessoryModel);
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Accessory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Accessory accessory = (Accessory)sender;
            switch(e.PropertyName)
            {
                case "Name":
                    // Set the values back to default when the accessory changes
                    accessory.ColorValues = accessory.DepthValues = accessory.WidthValues = accessory.HeightValues = null;
                    accessory.Length = 0;
                    accessory.HasLength = false;
                    accessory.ColorValues = new ObservableCollection<string>(GetColorValues(accessory.Name));
                    accessory.HasLength = HasLength(accessory.Name);
                    break;
                case "Color":
                    accessory.DepthValues = new ObservableCollection<string>(GetDepthValues(accessory.Name, accessory.Color));
                    accessory.WidthValues = new ObservableCollection<string>(GetWidthValues(accessory.Name, accessory.Color));
                    accessory.HeightValues = new ObservableCollection<string>(GetHeightValues(accessory.Name, accessory.Color));
                    break;
            }
            
            accessory.Price = SetPrice(accessory);
        }

        private decimal SetPrice(Accessory accessory)
        {
            decimal price = (
                from row in AccessoryData.AsEnumerable()
                where row.Field<string>("ItemName") == accessory.Name
                    && row.Field<string>("Color") == accessory.Color
                    && row.Field<string>("Width") == accessory.Width
                    && row.Field<string>("Height") == accessory.Height
                    && row.Field<string>("Depth") == accessory.Depth
                select row.Field<decimal>("Price")).First();

            bool haslength = (
                from row in AccessoryData.AsEnumerable()
                where row.Field<string>("ItemName") == accessory.Name
                    && row.Field<string>("Color") == accessory.Color
                    && row.Field<string>("Width") == accessory.Width
                    && row.Field<string>("Height") == accessory.Height
                    && row.Field<string>("Depth") == accessory.Depth
                select row.Field<bool>("Length")).First();

            if (haslength)
                price = price * (decimal)accessory.Length;
            
            return price;
        }

        /// <summary>
        /// Gets a list of accessories
        /// </summary>
        private List<string> GetAccessories() =>
            (from row in AccessoryData.AsEnumerable()
             select row.Field<string>("ItemName")).Distinct().ToList();

        /// <summary>
        /// Gets a list of color values
        /// </summary>
        /// <param name="AccessoryName">
        /// The name of the accessory to get the colors from
        /// </param>
        private List<string> GetColorValues(string AccessoryName) => 
            (from row in AccessoryData.AsEnumerable()
            where row.Field<string>("ItemName") == AccessoryName
            select row.Field<string>("Color")).Distinct().ToList();

        /// <summary>
        /// Gets the boolean value if the length should be set
        /// </summary>
        /// <param name="AccessoryName">
        /// The name of the accessory
        /// </param>
        private bool HasLength(string AccessoryName) =>
            bool.Parse((from row in AccessoryData.AsEnumerable()
                        where row.Field<string>("ItemName") == AccessoryName
                        select row.Field<string>("Length")).Distinct().First());

        /// <summary>
        /// Gets the height values for the accessory
        /// </summary>
        /// <param name="AccessoryName">
        /// The name of the accessory
        /// </param>
        /// <param name="Color">
        /// The color of the accessory
        /// </param>
        private List<string> GetHeightValues(string AccessoryName, string Color) =>
            (from row in AccessoryData.AsEnumerable()
             where row.Field<string>("ItemName") == AccessoryName
                && row.Field<string>("Color") == Color
             select row.Field<string>("Height")).Distinct().ToList();

        /// <summary>
        /// Gets the width values for the accessory
        /// </summary>
        /// <param name="AccessoryName">
        /// The name of the accessory
        /// </param>
        /// <param name="Color">
        /// The color of the accessory
        /// </param>
        private List<string> GetWidthValues(string AccessoryName, string Color) =>
            (from row in AccessoryData.AsEnumerable()
             where row.Field<string>("ItemName") == AccessoryName
                && row.Field<string>("Color") == Color
             select row.Field<string>("Width")).Distinct().ToList();

        /// <summary>
        /// Gets the depth values for the accessory
        /// </summary>
        /// <param name="AccessoryName">
        /// The name of the accessory
        /// </param>
        /// <param name="Color">
        /// The color of the accessory
        /// </param>
        private List<string> GetDepthValues(string AccessoryName, string Color) =>
            (from row in AccessoryData.AsEnumerable()
             where row.Field<string>("ItemName") == AccessoryName
                && row.Field<string>("Color") == Color
             select row.Field<string>("Depth")).Distinct().ToList();

        #endregion
    }
}
