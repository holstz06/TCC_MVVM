using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace TCC_MVVM.Model
{
    [ImplementPropertyChanged]
    public class Banding : INotifyPropertyChanged
    {
        public string Color { get; set; }
        public decimal Price { get; set; }
        public Dictionary<string, decimal> BandingValues { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
