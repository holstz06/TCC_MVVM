using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged;

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
        public decimal Price { get; set; } = 0M;

        public ObservableCollection<string> Accessories { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ColorValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> WidthValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> HeightValues { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> DepthValues { get; set; } = new ObservableCollection<string>();

        public bool HasLength { get; set; } = false;

        #region Member Validation

        static readonly string[] PropertyNames =
        {
            "Name",
            "Width",
            "Height",
            "Depth",
            "Length"
        };

        /// <summary>
        /// Validates a field by their property name
        /// </summary>
        /// <param name="PropertyName">
        /// The property name
        /// </param>
        /// <returns>
        /// True, if the property is valid
        /// </returns>
        public bool IsValid(string PropertyName)
        {
            switch(PropertyName)
            {
                case "Name": if (string.IsNullOrEmpty(Name)) return false; break;
                case "Width": if (Width == "N/A") return false; break;
                case "Height": if (Height == "N/A") return false; break;
                case "Depth": if (Depth == "N/A") return false; break;
                case "Length": if (Length == 0) return false; break;
            }
            return true;
        }

        /// <summary>
        /// Validates all the fields in this accessory
        /// </summary>
        /// <returns>
        /// True, if this accessory is valid
        /// </returns>
        public bool IsValid()
        {
            foreach(string property in PropertyNames)
            {
                if(!IsValid(property))
                    return false;
            }
            return true;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
