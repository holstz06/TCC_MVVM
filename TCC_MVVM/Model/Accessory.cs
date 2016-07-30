﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged;
using System;

namespace TCC_MVVM.Model
{
    [ImplementPropertyChanged]
    public class Accessory : INotifyPropertyChanged
    {
        public int RoomNumber { get; set; }
        public string Name { get; set; } = "N/A";
        public string Color { get; set; } = "N/A";
        public string Width { get; set; } = "N/A";
        public string Height { get; set; } = "N/A";
        public string Depth { get; set; } = "N/A";
        public double Length { get; set; } = 0;
        public decimal Price { get; set; }

        public ObservableCollection<string> Accessories { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> WidthValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> HeightValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> DepthValues { get; set; } = new ObservableCollection<string>();

        public bool HasLength { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public readonly string[] Properties =
        {
            "RoomNumber",
            "Name",
            "Color",
            "Width",
            "Height",
            "Depth",
            "Length",
            "Price"
        };

        public void SetProperty(string PropertyName, string PropertyValue)
        {
            switch(PropertyName)
            {
                case "RoomNumber": RoomNumber = int.Parse(PropertyValue); break;
                case "Name": Name = PropertyValue; break;
                case "Color": Color = PropertyValue; break;
                case "Width": Width = PropertyValue; break;
                case "Height": Height = PropertyValue; break;
                case "Depth": Depth = PropertyValue; break;
                case "Length": Length = double.Parse(PropertyValue); break;
                case "Price": Price = decimal.Parse(PropertyValue); break;
            }
        }

        string _DisplayName;
        public string DisplayName
        {
            get
            {
                _DisplayName = Name;
                if (Color != "N/A")
                    _DisplayName += ", " + Color;
                if (Width != "N/A")
                    _DisplayName += ", " + Width + " in.";
                if (Height != "N/A")
                    _DisplayName += ", " + Height + " in.";
                if (Depth != "N/A")
                    _DisplayName += ", " + Depth + " in.";
                if (Length != 0)
                    _DisplayName += ", " + Length + "in. ";
                return _DisplayName;
            }
        }
    }
}
