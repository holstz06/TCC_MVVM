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
    class WoodPrice
    {
        private List<string> Colors { get; set; }
        private List<decimal> Prices { get; set; }

        /// <summary>
        /// Create a new instance of a wood price
        /// </summary>
        public WoodPrice()
        {
            InitializeWood();
        }

        /// <summary>
        /// Get the price for a certain color of wood
        /// </summary>
        /// <param name="color">The color of wood</param>
        /// <returns>The price of that color</returns>
        public decimal GetWoodPrice(string color)
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
        private void InitializeWood()
        {
            Colors = new List<string>();
            Prices = new List<decimal>();

            // Attempt to read in all values form file
            try
            {
                List<string> lines = File.ReadAllLines(ConfigurationManager.AppSettings["WoodPrice"]).ToList();
                foreach (string line in lines)
                {
                    var splitLines = line.Split(',');
                    Colors.Add(splitLines[0].Trim());

                    // This will attempt to convert a string to a double
                    try { Prices.Add(Convert.ToDecimal(splitLines[1].Trim())); }
                    catch (Exception e) { MessageBox.Show(e.ToString()); }
                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }

        }
    }
}
