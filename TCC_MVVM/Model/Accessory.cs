using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// An accessory to a shelving unit
    /// </summary>
    [ImplementPropertyChanged]
    public class Accessory : INotifyPropertyChanged
    {
        /// <summary>
        /// The room number this accessory belongs to
        /// </summary>
        public int RoomNumber { get; set; }
        /// <summary>
        /// The name of this accessory
        /// </summary>
        public string Name { get; set; } = "N/A";
        /// <summary>
        /// The Color of this accessory (if there is one)
        /// </summary>
        public string Color { get; set; } = "N/A";
        /// <summary>
        /// The width of this accessory (if there is one)
        /// </summary>
        public string Width { get; set; } = "N/A";
        /// <summary>
        /// The height of this accessory (if there is one)
        /// </summary>
        public string Height { get; set; } = "N/A";
        /// <summary>
        /// The depth of this accessory (if there is one)
        /// </summary>
        public string Depth { get; set; } = "N/A";
        /// <summary>
        /// The lenght of this accessory (which is different than width, as length can be any size)
        /// </summary>
        public double Length { get; set; } = 0;
        /// <summary>
        /// The price of this accessory
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// A list of all possible accessories to choose from
        /// </summary>
        public ObservableCollection<string> Accessories { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// A collection of color values this accessory can be
        /// </summary>
        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// A collection of width values this accessory can be
        /// </summary>
        public ObservableCollection<string> WidthValues { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// A collection of height values this accessory can be
        /// </summary>
        public ObservableCollection<string> HeightValues { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// A collection of depth values this accessory can be
        /// </summary>
        public ObservableCollection<string> DepthValues { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Does this accessory have a length (opposed to having a width)
        /// </summary>
        public bool HasLength { get; set; } = false;

        #region PropertyChangedEvent Handler
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// A list of property names
        /// </summary>
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

        /// <summary>
        /// Sets the property value based on proeprty name
        /// </summary>
        /// <param name="PropertyName">The name of the property</param>
        /// <param name="PropertyValue">The value of the property</param>
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

        /// <summary>
        /// What this accessory will display on the view
        /// </summary>
        public string DisplayName
        {
            get
            {
                string displayname = Name;
                if (Color != "N/A")
                    displayname += ", " + Color;
                if (Width != "N/A")
                    displayname += ", " + Width + " in.";
                if (Height != "N/A")
                    displayname += ", " + Height + " in.";
                if (Depth != "N/A")
                    displayname += ", " + Depth + " in.";
                if (Length != 0)
                    displayname += ", " + Length + "in. ";
                return displayname;
            }
        }
    }
}
