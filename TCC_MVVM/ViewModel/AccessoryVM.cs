using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using TCC_MVVM.Model;
using System;
using System.Windows;

namespace TCC_MVVM.ViewModel
{
    public class AccessoryVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Data table with the accessories and their attributes
        /// </summary>
        private DataTable AccessoryData;

        /// <summary>
        /// The total price of all the panels
        /// </summary>
        public decimal TotalPrice
        {
            get { return Math.Round(_TotalPrice, 2, MidpointRounding.AwayFromZero); }
            set { _TotalPrice = value; OnPropertyChanged("TotalPrice"); }
        }
        private decimal _TotalPrice;

        /// <summary>
        /// Collection of accessories
        /// </summary>
        public ObservableCollection<Accessory> Accessories { get; set; } = new ObservableCollection<Accessory>();
        private List<Accessory> AllAccessories { get; set; } = new List<Accessory>();

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
        private ICommand _RemoveCommand;

        /// <summary>
        /// Creates new instance of an accessory view model
        /// </summary>
        public AccessoryVM()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml("AccessoryData.xml");
            AccessoryData = dataset.Tables[0];

            // Create dictionary of prices by accessory name
            AllAccessories = AccessoryData.AsEnumerable().Select(row => new Accessory()
            {
                Name  = row.Field<string>("ItemName"),
                Color = row.Field<string>("Color"),
                Width  = row.Field<string>("Width"),
                Height = row.Field<string>("Height"),
                Depth  = row.Field<string>("Depth"),
                Price = decimal.Parse(row.Field<string>("Price"))
            }).ToList();
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

        public void Add(Accessory Accessory)
        {
            Accessory.Accessories = new ObservableCollection<string>(GetAccessories());
            Accessory.PropertyChanged += Accessory_PropertyChanged;
            Accessories.Add(Accessory);
        }

        /// <summary>
        /// Remove accessory from the collection
        /// </summary>
        /// <param name="AccessoryModel"></param>
        public void Remove(Accessory Accessory)
        {
            // Remove the price before you remove the accessory, otherwise the price will stay the same
            TotalPrice -= Accessory.Price;
            if (Accessories.Contains(Accessory))
                Accessories.Remove(Accessory);
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Accessory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                Accessory accessory = (Accessory)sender;
                switch (e.PropertyName)
                {
                    case "Name":
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
                    case "Price":
                        TotalPrice = 0;
                        foreach (Accessory accessoryModel in Accessories)
                            TotalPrice += accessoryModel.Price;
                        break;
                }
                if(!e.PropertyName.Equals("Price")
                    && !string.IsNullOrEmpty(accessory.Color)
                    && !string.IsNullOrEmpty(accessory.Width)
                    && !string.IsNullOrEmpty(accessory.Depth)
                    && !string.IsNullOrEmpty(accessory.Height))
                    accessory.Price = SetPrice(accessory);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private decimal SetPrice(Accessory selectedAccessory)
        {
            foreach(Accessory accessory in AllAccessories)
            {
                if (selectedAccessory.Color == accessory.Color
                    && selectedAccessory.Width == accessory.Width
                    && selectedAccessory.Depth == accessory.Depth
                    && selectedAccessory.Height == accessory.Height
                    && selectedAccessory.Name == accessory.Name)
                {
                    if (selectedAccessory.HasLength)
                        return accessory.Price * (decimal)selectedAccessory.Length;
                    else
                        return accessory.Price;
                }     
            }
            return 0M;
        }

        /// <summary>
        /// Retrieves a list of accessory names
        /// </summary>
        /// <returns>
        /// A list of accessory names
        /// </returns>
        private List<string> GetAccessories() 
            => AccessoryData.AsEnumerable().Select(row => row.Field<string>("ItemName")).Distinct().ToList();


        /// <summary>
        /// Retrieves a list of color based on accessory name
        /// </summary>
        /// <param name="AccessoryName">
        /// The name of the accessory
        /// </param>
        private List<string> GetColorValues(string AccessoryName) 
            => (from row in AccessoryData.AsEnumerable()
                where row.Field<string>("ItemName") == AccessoryName
                select row.Field<string>("Color")).Distinct().ToList();

        /// <summary>
        /// Gets the boolean value if the length should be set
        /// </summary>
        /// <param name="AccessoryName">
        /// The name of the accessory
        /// </param>
        private bool HasLength(string AccessoryName)
        {
            bool haslength = false; 
            var query = (from row in AccessoryData.AsEnumerable()
                        where row.Field<string>("ItemName") == AccessoryName
                        select row.Field<string>("Length")).Distinct().First();
            if (query == "TRUE")
                haslength = true;
            return haslength;
        }
            

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
