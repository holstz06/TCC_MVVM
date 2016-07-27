﻿using TCC_MVVM.Model;
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
    public partial class ShelfVM : INotifyPropertyChanged
    {
        /// <summary>
        /// The collection of shelves
        /// </summary>
        public ObservableCollection<Shelf> Shelves { get; set; } = new ObservableCollection<Shelf>();

        /// <summary>
        /// The total price of all the shelves
        /// </summary>
        decimal _TotalPrice;
        public decimal TotalPrice
        {
            get { return Math.Round(_TotalPrice, 2, MidpointRounding.AwayFromZero); }
            set { _TotalPrice = value; OnPropertyChanged("TotalPrice"); }
        }

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

            AddAdjustableShelvingCommand = new AddASCommand(this);
        }

        /// <summary>
        /// Removes a shelf from the collection
        /// </summary>
        public void Remove(Shelf Shelf)
        {
            // Remove the price before you remove the shelf, otherwise the price will stay the same
            TotalPrice -= Shelf.Price;
            if (Shelves.Contains(Shelf))
                Shelves.Remove(Shelf);
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
            shelf.Wood.WoodValues = GetWoodValues();
            shelf.Banding.BandingValues = GetBandingValues();
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

    public class AddASCommand : ICommand
    {
        ShelfVM viewmodel;
        public AddASCommand(ShelfVM viewmodel)
        {
            this.viewmodel = viewmodel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Shelf shelf = (parameter as Shelf);
            Shelf newShelf = (shelf.Clone() as Shelf);

            if(shelf.ShelfType.Name == "Fixed")
            {
                foreach (var shelfType in viewmodel.ShelfTypes)
                {
                    if(shelfType.Name == "Adjustable")
                    {
                        newShelf.ShelfTypeName = shelfType.Name;
                        viewmodel.Add(newShelf);
                    }
                }
            }

        }
    }
}
