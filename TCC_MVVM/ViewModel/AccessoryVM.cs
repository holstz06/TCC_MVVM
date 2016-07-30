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
        DataTable AccessoryData;
        DataTable ExtraAccessoryData;

        decimal _TotalPrice;
        public decimal TotalPrice
        {
            get { return Math.Round(_TotalPrice, 2, MidpointRounding.AwayFromZero); }
            set { _TotalPrice = value; OnPropertyChanged("TotalPrice"); }
        }
        public ObservableCollection<Accessory> Accessories { get; set; } = new ObservableCollection<Accessory>();
        List<Accessory> AllAccessories { get; set; } = new List<Accessory>();

        ///====================================================================================
        /// <summary>
        ///     Command to call the remove function (removes accessory from the collection
        /// </summary>
        ///====================================================================================
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Accessory)param));
                return _RemoveCommand;
            }
        }
        ICommand _RemoveCommand;


        ///====================================================================================
        /// <summary>
        ///     Creates a new instance of the Accessory View Model - Default
        /// </summary>
        ///====================================================================================
        public AccessoryVM()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml("AccessoryData.xml");
            AccessoryData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ExtraAccessoryDataSet.xml");
            ExtraAccessoryData = dataset.Tables[0];

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

        ///====================================================================================
        /// <summary>
        ///     Adds an accessory to the collection
        /// </summary>
        /// 
        /// <param name="roomNumber">
        ///     The room number to assign the accessory to
        /// </param>
        ///====================================================================================
        public void Add(int roomNumber)
        {
            Accessory accessory = new Accessory()
            {
                Accessories = new ObservableCollection<string>(GetAccessories()),
                RoomNumber = roomNumber
            };

            accessory.PropertyChanged += Accessory_PropertyChanged;
            Accessories.Add(accessory);
        }


        ///====================================================================================
        /// <summary>
        ///     Adds an accessory to the collection
        /// </summary>
        /// 
        /// <param name="accessory">
        ///     The accessory to add
        /// </param>
        ///====================================================================================
        public void Add(Accessory accessory)
        {
            accessory.Accessories = new ObservableCollection<string>(GetAccessories());
            accessory.ColorValues = new ObservableCollection<string>(GetColorValues(accessory.Name));
            accessory.WidthValues = new ObservableCollection<string>(GetWidthValues(accessory.Name, accessory.Color));
            accessory.HeightValues = new ObservableCollection<string>(GetHeightValues(accessory.Name, accessory.Color));
            accessory.DepthValues = new ObservableCollection<string>(GetDepthValues(accessory.Name, accessory.Color));
            accessory.PropertyChanged += Accessory_PropertyChanged;
            Accessories.Add(accessory);
        }

        ///====================================================================================
        /// <summary> 
        ///     Removes an accessory from the collection
        /// </summary>
        /// 
        /// <param name="Accessory">
        ///     The accessory to be removed
        /// </param>
        ///==================================================================================== 
        public void Remove(Accessory Accessory)
        {
            // Remove the price before you remove the accessory, otherwise the price will stay the same
            TotalPrice -= Accessory.Price;
            if (Accessories.Contains(Accessory))
                Accessories.Remove(Accessory);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
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
                    case "NonRoundedPrice":
                        accessory.Price = accessory.Price;
                        break;
                }
                if(!e.PropertyName.Equals("Price")
                    && !string.IsNullOrEmpty(accessory.Color)
                    && !string.IsNullOrEmpty(accessory.Width)
                    && !string.IsNullOrEmpty(accessory.Depth)
                    && !string.IsNullOrEmpty(accessory.Height))
                {
                    accessory.Price = 0 + SetPrice(accessory);
                    foreach(var price in GetExtraPrice(accessory.Name))
                    {
                        accessory.Price += decimal.Parse(price);
                    }
                }
                    
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        decimal SetPrice(Accessory selectedAccessory)
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
                    return accessory.Price;
                }     
            }
            return 0M;
        }

        ///====================================================================================
        /// <summary>
        ///     Retrieves a list of accessory names
        /// </summary>
        /// 
        /// <returns>
        ///     A list of accessory names
        /// </returns>
        ///==================================================================================== 
        List<string> GetAccessories() 
            => AccessoryData.AsEnumerable().Select(row => row.Field<string>("ItemName")).Distinct().ToList();

        ///====================================================================================
        /// <summary>
        ///     Retrieves a list of color based on the accessory's name
        /// </summary>
        /// 
        /// <param name="AccessoryName">
        ///     The name of the accessory
        /// </param>
        /// 
        /// <returns>
        ///     A list of color values
        /// </returns>
        ///====================================================================================
        List<string> GetColorValues(string AccessoryName) 
            => (from row in AccessoryData.AsEnumerable()
                where row.Field<string>("ItemName") == AccessoryName
                select row.Field<string>("Color")).Distinct().ToList();

        ///====================================================================================
        /// <summary>
        ///     Gets the boolean value if the length should be set
        /// </summary>
        /// 
        /// <param name="AccessoryName">
        ///     The name of the accessory
        /// </param>
        /// 
        /// <returns>
        ///     True, if the accessory has a length field
        ///     False, if the accessory does not have a length field
        /// </returns>
        ///====================================================================================
        bool HasLength(string AccessoryName)
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
        List<string> GetHeightValues(string AccessoryName, string Color) =>
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
        List<string> GetWidthValues(string AccessoryName, string Color) =>
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
        List<string> GetDepthValues(string AccessoryName, string Color) =>
            (from row in AccessoryData.AsEnumerable()
             where row.Field<string>("ItemName") == AccessoryName
                && row.Field<string>("Color") == Color
             select row.Field<string>("Depth")).Distinct().ToList();

        List<string> GetExtraPrice(string AccessoryName) =>
            (from row in ExtraAccessoryData.AsEnumerable()
             where row.Field<string>("ItemName") == AccessoryName
             select row.Field<string>("ExtraPrice")).Distinct().ToList();

    }
}
