using System;
using System.Data;
using System.Windows;
using PropertyChanged;
using System.ComponentModel;

namespace TCC_MVVM.Data
{
    [ImplementPropertyChanged]
    public class TCCData : INotifyPropertyChanged
    {
        public DataTable Data { get; set; }

        public TCCData()
        {
            
        }

        /// <summary>
        /// Create a new instance of TCCData
        /// </summary>
        /// <param name="Path">
        /// Load existing data from the path
        /// </param>
        public TCCData(string Path)
        {
            Load(Path);
        }

        /// <summary>
        /// Loads the data from an xml file
        /// </summary>
        /// <param name="Path">
        /// The path to the xml file
        /// </param>
        public void Load(string Path)
        {
            if(!string.IsNullOrEmpty(Path))
            {
                try { Data.ReadXml(Path); }
                catch (Exception e) { MessageBox.Show(e.ToString()); }
            }
        }

        /// <summary>
        /// Exports the xml file
        /// </summary>
        /// <param name="Path">
        /// The path to exort the file to
        /// </param>
        public void Export(string Path)
        {
            if (!string.IsNullOrEmpty(Path))
            {
                try { Data.WriteXml(Path); }
                catch (Exception e) { MessageBox.Show(e.ToString()); }
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Property Changed Event Handeler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Invoke new property change
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property
        /// </param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
