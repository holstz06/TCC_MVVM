using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TCC_MVVM.Model
{
    public class SaveFile
    {
        public string FilePath { get; set; }
        public List<string> SaveLines { get; set; } = new List<string>();

        /// <summary>
        /// Create a new instance of a save file
        /// </summary>
        /// <param name="SaveLines"></param>
        public SaveFile(List<string> SaveLines = null)
        {
            if (SaveLines != null)
                this.SaveLines = SaveLines;
        }

        /// <summary>
        /// Create new instance of a save file
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="SaveLines"></param>
        public SaveFile(string FilePath, List<string> SaveLines = null)
        {
            this.FilePath = FilePath;
            if (SaveLines != null)
                this.SaveLines = SaveLines;
        }

        /// <summary>
        /// Write save lines to the file
        /// </summary>
        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                foreach (string line in SaveLines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Load the lines from the file
        /// </summary>
        public void Load()
        {
            SaveLines = File.ReadAllLines(FilePath).ToList();
        }
    }
}
