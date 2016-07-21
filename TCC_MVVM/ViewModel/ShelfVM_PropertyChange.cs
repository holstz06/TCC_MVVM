using TCC_MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;
using System;

namespace TCC_MVVM.ViewModel
{
    public partial class ShelfVM
    {
        /// <summary>
        /// The Shelf view model subscribes to the Property Changed Event Handeler,
        /// that allows all properties of this object to be observered when changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Subscribes the shelves of the shelf view model to the property changed event handeler.
        /// Allows the View Model to view property changes of its Models
        /// </summary>
        void Shelf_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Shelf shelf = (Shelf)sender;
            switch (e.PropertyName)
            {
                case "Price":
                    Price_PropertyChanged();
                    break;
                case "ShelfTypeName":
                    ShelfTypeName_PropertyChanged(shelf);
                    break;
                case "Color":
                    Color_PropertyChanged(shelf);
                    break;
            }
        }

        /// <summary>
        /// Triggers when 'ShelfTypeName' property has been changed
        /// </summary>
        void ShelfTypeName_PropertyChanged(Shelf shelf)
        {
            // Loop through the list of shelf types and select the correct one
            foreach(ShelfType item in ShelfTypes)
            {
                if (shelf.ShelfTypeName == item.Name)
                {
                    shelf.ShelfType = item;
                    Fence fence = GetFence(shelf);
                    CamPost campost = GetCamPost(shelf);
                    TopConnector topconnector = GetTopConnector(shelf);

                    if(fence != null)
                    {
                        shelf.ShelfType.FencePrice = fence.Price;
                        shelf.ShelfType.FenceColor = fence.Color;
                    }
                    if (campost != null)
                    {
                        shelf.ShelfType.CamPostColor = campost.Color;
                        shelf.ShelfType.CamPostPrice = campost.Price;
                    }
                    if(topconnector != null)
                    {
                        shelf.ShelfType.TopConnectorColor = topconnector.Color;
                        shelf.ShelfType.TopConnectorPrice = topconnector.Price;
                    }
                }   
            }
        }

        /// <summary>
        /// Event that triggers when 'Color' property has been changed
        /// </summary>
        void Color_PropertyChanged(Shelf shelf)
        {
            // Select the appropriate cam post
            foreach (CamPost campost in CamPosts)
            {
                if (campost.WoodColor == shelf.Color)
                {
                    shelf.ShelfType.CamPostColor = campost.Color;
                    shelf.ShelfType.CamPostPrice = campost.Price;
                }
            }

            // Select the appropriate top connector
            foreach(TopConnector top in TopConnectors)
            {
                if(top.WoodColor == shelf.Color)
                {
                    shelf.ShelfType.TopConnectorColor = top.Color;
                    shelf.ShelfType.TopConnectorPrice = top.Price;
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
        Fence GetFence(Shelf shelf)
        {
            if (shelf.ShelfType.HasFence)
            {
                foreach (Fence fence in Fences)
                {
                    if (shelf.ShelfType.FenceColor == fence.Color)
                        return fence;
                }
            }

            return null;
        }

        /// <summary>
        /// Selects the correct campost that belongs to the shelf
        /// based on the shelf's color
        /// </summary>
        CamPost GetCamPost(Shelf shelf)
        {
            foreach (CamPost campost in CamPosts)
            {
                if (campost.WoodColor == shelf.Color)
                    return campost;
            }
            return null;
        }

        /// <summary>
        /// Selects the correct top connector that belongs to the shelf
        /// based on the shelf's color
        /// </summary>
        /// <param name="shelf"></param>
        /// <returns></returns>
        TopConnector GetTopConnector(Shelf shelf)
        {
            foreach(TopConnector top in TopConnectors)
            {
                if (shelf.Color == top.WoodColor)
                    return top;
            }
            return null;
        }

        /// <summary>
        /// Reset the price of the view model when a shelf changes (added, deleted, or updated)
        /// </summary>
        void Price_PropertyChanged()
        {
            TotalPrice = 0;
            foreach (Shelf shelf in Shelves)
                TotalPrice += shelf.Price;
        }
    }
}

