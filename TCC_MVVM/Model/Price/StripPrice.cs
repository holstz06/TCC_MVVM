using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TCC_MVVM.Price
{
    class StripPrice
    {
        public List<string> Colors { get; set; }
        public List<decimal> Prices { get; set; }
        public decimal MetalPrice { get; set; }

        /// <summary>
        /// Create a new instance of a Strip Price
        /// </summary>
        public StripPrice()
        {
            InitializeStrip();
        }

        /// <summary>
        /// Get the price of a certain color of strip
        /// </summary>
        /// <param name="color">The color of strip</param>
        /// <returns>The price of that strip</returns>
        public decimal GetStripPrice(string color)
        {
            decimal price = 0;
            for (int index = 0; index < Colors.Count; index++)
            {
                if (Colors[index] == color)
                {
                    price = Prices[index];
                    break;
                }
            }
            return price;
        }

        /// <summary>
        /// Initialize a collection of colors and prices
        /// </summary>
        private void InitializeStrip()
        {
            Colors = new List<string>();
            Prices = new List<decimal>();

            // Attempt to read in all values form file
            try
            {
                List<string> lines = File.ReadAllLines(ConfigurationManager.AppSettings["StripPrice"]).ToList();
                foreach (string line in lines)
                {
                    var splitLines = line.Split(',');

                    // Check if the color is metal and set that price
                    if(splitLines[0].Equals("Metal"))
                    {
                        try{ MetalPrice = Convert.ToDecimal(splitLines[1].Trim()); }
                        catch (Exception e) { MessageBox.Show(e.ToString()); }
                    }
                        
                    else
                    {
                        Colors.Add(splitLines[0].Trim());

                        // This will attempt to convert a string to a double
                        try { Prices.Add(Convert.ToDecimal(splitLines[1].Trim())); }
                        catch (Exception e) { MessageBox.Show(e.ToString()); }
                    }
                    
                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }
    }
}
