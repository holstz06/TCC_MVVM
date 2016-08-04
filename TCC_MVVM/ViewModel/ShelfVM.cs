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
using TCC_MVVM.ViewModel.ShelfCommands;

namespace TCC_MVVM.ViewModel
{
    [ImplementPropertyChanged]
    public partial class ShelfVM : INotifyPropertyChanged
    {
        /// <summary>
        /// The collection of shelves
        /// </summary>
        public ObservableCollection<Shelf> Shelves { get; set; } = new ObservableCollection<Shelf>();

        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Command to remove shelf from the room
        /// </summary>
        ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new CollectionChangeCommand(param => Remove((Shelf)param));
                return _RemoveCommand;
            }
        }

        public ICommand AddAdjustableShelvingCommand { get; private set; }

        /// <summary>
        /// Creates a new instance of the shelf view model
        /// </summary>
        public ShelfVM()
        {
            InitializeData();

            AddAdjustableShelvingCommand = new AddAdjustableShelfCommand(this);
        }

        /// <summary>
        ///     Removes a shelf from the collection
        /// </summary>
        public void Remove(Shelf shelf)
        {
            TotalPrice -= shelf.Price;
            TotalQuantity -= shelf.Quantity;
            if (Shelves.Contains(shelf))
                Shelves.Remove(shelf);
        }

        /// <summary>
        /// Adds a shelf to the collection
        /// </summary>
        /// <param name="RoomNumber"></param>
        /// <param name="Color"></param>
        /// <param name="SizeDepth"></param>
        public void Add(int RoomNumber, string Color = null, string SizeDepth = null)
        {
            bool HasColor = false;
            bool HasDepth = false;

            if (Color != null) HasColor = true;
            HasDepth |= SizeDepth != null;

            Shelf shelf = new Shelf(
                RoomNumber,
                HasColor ? Color : null,
                HasDepth ? SizeDepth : null)
            {
                ColorValues = new ObservableCollection<string>(GetColorValues()),
                WidthValues = new ObservableCollection<string>(GetWidthValues()),
                DepthValues = new ObservableCollection<string>(GetDepthValues()),
                ShelfTypeValues = new ObservableCollection<string>(GetShelfTypeNames()),
                viewmodel = this
            };

            shelf.Wood.WoodValues = GetWoodValues();
            shelf.Banding.BandingValues = GetBandingValues();

            TotalQuantity += 1;

            shelf.PropertyChanged += Shelf_PropertyChanged;

            if (Color != null)
                Color_PropertyChanged(shelf);
            Shelves.Add(shelf);
        }

        /// <summary>
        /// Adds a shelf, that has already been instantiated, to the collection
        /// </summary>
        /// <param name="shelf"></param>
        public void Add(Shelf shelf)
        {
            shelf.ColorValues = new ObservableCollection<string>(GetColorValues());
            shelf.WidthValues = new ObservableCollection<string>(GetWidthValues());
            shelf.DepthValues = new ObservableCollection<string>(GetDepthValues());
            shelf.ShelfTypeValues = new ObservableCollection<string>(GetShelfTypeNames());
            shelf.viewmodel = this;
            shelf.Wood.WoodValues = GetWoodValues();
            shelf.Banding.BandingValues = GetBandingValues();
            TotalQuantity += shelf.Quantity;
            shelf.PropertyChanged += Shelf_PropertyChanged;
            Shelves.Add(shelf);
        }

        /// <summary>
        /// Sets all the shelves to the same color
        /// </summary>
        public void SetAllShelfColor(string Color)
        {
            foreach (Shelf shelf in Shelves)
                shelf.Color = Color;
        }

        /// <summary>
        /// Sets all the shelves to the same depth
        /// </summary>
        public void SetAllShelfDepth(string SizeDepth)
        {
            foreach (Shelf shelf in Shelves)
                shelf.SizeDepth = SizeDepth;
        }
    }
}
