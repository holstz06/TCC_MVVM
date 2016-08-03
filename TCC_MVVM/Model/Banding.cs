using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace TCC_MVVM.Model
{
    /// <summary>
    /// The banding of the wood shelving
    /// </summary>
    [ImplementPropertyChanged]
    public class Banding : INotifyPropertyChanged
    {
        /// <summary>
        /// The color of the banding
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// The price of the banding
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// A list of all possible Banding Values
        /// </summary>
        public Dictionary<string, decimal> BandingValues { get; set; }

        #region PropertyChanged EventHandler
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
