using TCC_MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;
using System;
using System.Windows;

namespace TCC_MVVM.ViewModel
{
    [ImplementPropertyChanged]
    public class ShelfVM : INotifyPropertyChanged
    {
        // Data Tables Vairables
        //================================
        private DataTable ShelfData;
        private DataTable ShelvingDepthData;
        private DataTable ShelfWidthData;
        private DataTable WoodData;
        private DataTable BandingData;
        private DataTable ShelfTypeData;

        // Collections Variables
        //=================================
        public ObservableCollection<Shelf> Shelves { get; set; } = new ObservableCollection<Shelf>();
        private List<CamPost> CamPosts { get; set; } = new List<CamPost>();
        private List<Fence> Fences { get; set; } = new List<Fence>();
        private List<ShelfType> ShelfTypes { get; set; } = new List<ShelfType>();
        private Dictionary<string, decimal> Woodvalues;
        private Dictionary<string, decimal> BandingValues;

        // Total Price of all shelves
        //=================================
        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get { return Math.Round(_TotalPrice, 2, MidpointRounding.AwayFromZero); }
            set { _TotalPrice = value; OnPropertyChanged("TotalPrice"); }
        }

        // Commands
        //=================================
        private ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Shelf)param));
                return _RemoveCommand;
            }
        }


        /// <summary>
        /// Creates a new instance of the shelf view model
        /// </summary>
        public ShelfVM()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml("ShelfData.xml");
            ShelfData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ShelfWidthData.xml");
            ShelfWidthData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ShelvingDepthData.xml");
            ShelvingDepthData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("WoodData.xml");
            WoodData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("BandingData.xml");
            BandingData = dataset.Tables[0];

            dataset = new DataSet();
            dataset.ReadXml("ShelfTypeData.xml");
            ShelfTypeData = dataset.Tables[0];

            // Get shelf related items from datasets
            CamPosts = GetCamPosts();
            Fences = GetFences();
            ShelfTypes = GetShelfTypes();
            Woodvalues = GetWoodValues();
            BandingValues = GetBandingValues();
        }

        /// <summary>
        /// Removes a shelf from the collection
        /// </summary>
        /// <param name="ShelfModel">
        /// The shelf to remove
        /// </param>
        public void Remove(Shelf Shelf)
        {
            // Remove the price before you remove the shelf, otherwise the price will stay the same
            TotalPrice -= Shelf.Price;
            if (Shelves.Contains(Shelf))
                Shelves.Remove(Shelf);
        }

        /// <summary>
        /// Add shelf to the collection
        /// </summary>
        /// <param name="RoomNumber"></param>
        /// <param name="ShelfNumber"></param>
        /// <param name="Color"></param>
        /// <param name="SizeDepth"></param>
        public void Add(int RoomNumber, int ShelfNumber, string Color = null, string SizeDepth = null)
        {
            bool HasColor = false;
            bool HasDepth = false;

            if (Color != null) HasColor = true;
            if (SizeDepth != null) HasDepth = true;

            Shelf shelf = new Shelf(RoomNumber, HasColor ? Color : null, HasDepth ? SizeDepth : null)
            {
                ColorValues = new ObservableCollection<string>(GetColorValues()),
                WidthValues = new ObservableCollection<string>(GetWidthValues()),
                DepthValues = new ObservableCollection<string>(GetDepthValues()),
                ShelfTypeValues = new ObservableCollection<ShelfType>(GetShelfTypes())
            };

            shelf.Wood.WoodValues = GetWoodValues();
            shelf.Banding.BandingValues = GetBandingValues();

            shelf.PropertyChanged += Shelf_PropertyChanged;

            Shelves.Add(shelf);
        }

        /// <summary>
        /// Retrieves a list of shelf types
        /// </summary>
        /// <returns>
        /// A list of shelf types
        /// </returns>
        private List<ShelfType> GetShelfTypes()
            => ShelfTypeData.AsEnumerable().Select(row => new ShelfType()
            {
                Name = row.Field<string>("ShelfType"),
                CamQuantity = int.Parse(row.Field<string>("CamQuantity")),
                HasFencePost = bool.Parse(row.Field<string>("HasFencePost")),
                FencePostColor = row.Field<string>("FencePostColor")
            }).ToList();

        /// <summary>
        /// Retrieves a list of camposts
        /// </summary>
        /// <returns>
        /// A list of camposts
        /// </returns>
        private List<CamPost> GetCamPosts()
            => (from row in ShelfData.AsEnumerable()
                where row.Field<string>("ItemName") == "Cam Post"
                select new CamPost
                {
                    WoodColor = row.Field<string>("WoodColor"),
                    Color = row.Field<string>("Color"),
                    Price = decimal.Parse(row.Field<string>("Price"))
                }).ToList();

        /// <summary>
        /// Retrieves a list of fences
        /// </summary>
        /// <returns>
        /// A list of fences
        /// </returns>
        private List<Fence> GetFences()
            => (from row in ShelfData.AsEnumerable()
                where row.Field<string>("ItemName") == "Fence"
                select new Fence
                {
                    Color = row.Field<string>("Color"),
                    Price = decimal.Parse(row.Field<string>("Price"))
                }).ToList();

        /// <summary>
        /// Retrieves a list of color values
        /// </summary>
        /// <returns>
        /// A list of color values
        /// </returns>
        private List<string> GetColorValues() => WoodData.AsEnumerable().Select(row => row.Field<string>("WoodColor")).Distinct().ToList();

        /// <summary>
        /// Retrieves a list of width values
        /// </summary>
        /// <returns>
        /// A list of width values
        /// </returns>
        private List<string> GetWidthValues() => ShelfWidthData.AsEnumerable().Select(row => row.Field<string>("ShelfWidth")).Distinct().ToList();

        /// <summary>
        /// Retrieves a list of depth values
        /// </summary>
        /// <returns>
        /// A list of depth values
        /// </returns>
        private List<string> GetDepthValues() => ShelvingDepthData.AsEnumerable().Select(row => row.Field<string>("ShelvingDepth")).Distinct().ToList();

        /// <summary>
        /// Gets a dictionary of wood prices by their color
        /// Key = Wood Color
        /// Value = Wood Price
        /// </summary>
        /// <returns>
        /// A dictionary of wood colors and prices
        /// </returns>
        private Dictionary<string, decimal> GetWoodValues()
            => WoodData.AsEnumerable().ToDictionary(row => row.Field<string>("WoodColor"), row => decimal.Parse(row.Field<string>("WoodPrice")));

        /// <summary>
        /// Gets a dictionary of banding prices by their color
        /// Key = Banding Color
        /// Value = Bandin Price
        /// </summary>
        /// <returns>
        /// A dictionary of banding colors and prices
        /// </returns>
        private Dictionary<string, decimal> GetBandingValues()
            => BandingData.AsEnumerable().ToDictionary(row => row.Field<string>("BandingColor"), row => decimal.Parse(row.Field<string>("BandingPrice")));

        /// <summary>
        /// Sets all the shelves to the same color
        /// </summary>
        /// <param name="Color">
        /// The color of the shelf to set
        /// </param>
        public void SetAllShelfColor(string Color)
        {
            foreach(Shelf shelf in Shelves)
                shelf.Color = Color;
        }

        /// <summary>
        /// Sets all the shelves to the same depth
        /// </summary>
        /// <param name="SizeDepth">
        /// The depth of the shelf to set
        /// </param>
        public void SetAllShelfDepth(string SizeDepth)
        {
            foreach (Shelf shelf in Shelves)
                shelf.SizeDepth = SizeDepth;
        }

        //====================================================================//
        //                      PROPERTY CHANGED HANDELERS                    //
        //====================================================================//

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Event listener for properties of a shelf
        /// </summary>
        void Shelf_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Shelf shelf = (Shelf)sender;
            switch(e.PropertyName)
            {
                case "Price":
                    SetPriceProperty(sender);
                    break;
                case "ShelfType":
                    ShelfType_PropertyChanged(shelf);
                    break;
                case "Color":
                    Color_PropertyChanged(shelf);
                    //shelf.CamPost = GetCamPost(shelf.Color);
                    break;
            }
        }

        /// <summary>
        /// Event that triggers when 'ShelfType' property has been chaged
        /// </summary>
        /// <param name="shelf">
        /// The Shelf model
        /// </param>
        private void ShelfType_PropertyChanged(Shelf shelf)
        {
            foreach(ShelfType shelftype in ShelfTypes)
            {
                if(shelftype.Name == shelf.ShelfType.Name)
                {
                    shelf.CamPostQuantity = shelftype.CamQuantity;
                    shelf.HasFence = shelftype.HasFencePost;
                    shelf.FenceColor = shelftype.FencePostColor;
                    if(shelf.HasFence)
                    {
                        foreach(Fence fence in Fences)
                        {
                            if (fence.Color == shelf.FenceColor)
                                shelf.FencePrice = fence.Price;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Event that triggers when 'Color' property has been changed
        /// </summary>
        /// <param name="shelf">
        /// The shelf model
        /// </param>
        private void Color_PropertyChanged(Shelf shelf)
        {
            foreach(CamPost campost in CamPosts)
            {
                if(campost.WoodColor == shelf.Color)
                {
                    shelf.CamPostColor = campost.Color;
                    shelf.CamPostPrice = campost.Price;
                }
            }

            // Set the value of the wood (color and price)
            shelf.Wood.Color = shelf.Color;
            shelf.Wood.Price = Woodvalues[shelf.Color];

            // Set the value of the banding (color and price)
            shelf.Banding.Color = shelf.Color;
            shelf.Banding.Price = BandingValues[shelf.Color];
        }

        /// <summary>
        /// Selects the correct fence that belongs to the shelf
        /// based on the shelf's type
        /// </summary>
        /// <param name="ShelfType">
        /// The type of shelf
        /// </param>
        /// <returns>
        /// The fence
        /// </returns>
        private Fence GetFence(ShelfType ShelfType)
        {
            if(ShelfType.HasFencePost)
            {
                foreach(Fence fence in Fences)
                {
                    if (ShelfType.FencePostColor == fence.Color)
                        return fence;
                }
            }

            return null;
        }

        /// <summary>
        /// Selects the correct campost that belongs to the shelf
        /// based on the shelf's color
        /// </summary>
        /// <param name="color">
        /// The color of the shelf
        /// </param>
        /// <returns>
        /// The campost
        /// </returns>
        private CamPost GetCamPost(string color)
        {
            foreach(CamPost campost in CamPosts)
            {
                if (campost.WoodColor == color)
                    return campost; 
            }
            return null;
        }

        /// <summary>
        /// Reset the price of the view model when a shelf changes (added, deleted, or updated)
        /// </summary>
        /// <param name="sender">
        /// The shelf that changes
        /// </param>
        void SetPriceProperty(object sender)
        {
            TotalPrice = 0;
            foreach (Shelf shelf in Shelves)
                TotalPrice += shelf.Price;
        }
    }
}
