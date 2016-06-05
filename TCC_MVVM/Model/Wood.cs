using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// Represents the wood used to make shelving
    /// </summary>
    [ImplementPropertyChanged]
    public class Wood : INotifyPropertyChanged
    {
        public const double MARKUP = 1.40;
        public const double PANEL_INSTALL = 10;
        public const double SHELF_INSTALL = 4;
        public const double DRILLBIT_FEE = 2;
        public const double ROUTER_FEE = 0.5;

        public string Color { get; set; }
        public decimal Price { get; set; }

        public Dictionary<string, decimal> WoodValues { get; set; } 
            = new Dictionary<string, decimal>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
