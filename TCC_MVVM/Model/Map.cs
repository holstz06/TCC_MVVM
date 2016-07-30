using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_MVVM.Model
{
    class Map : INotifyPropertyChanged
    {
        public string Address { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if(propertyName == "Address")
            {
                try
                {
                    StringBuilder queryaddress = new StringBuilder();
                    queryaddress.Append("http://maps.google.com/maps?q=");

                    if(!string.IsNullOrEmpty(Address))
                    {
                        queryaddress.Append(Address);
                    }
                }
                catch(Exception e)
                {

                }
            }
        }
    }
}
