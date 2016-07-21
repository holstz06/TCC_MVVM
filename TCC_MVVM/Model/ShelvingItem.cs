using System.Collections.Generic;

namespace TCC_MVVM.Model
{
    public class ShelvingItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public bool HasItem { get; set; }

        public Dictionary<string, object> Properties { get; set; } 
            = new Dictionary<string, object>();

        /// <summary>
        /// Adds a user created property to the list of properties
        /// </summary>
        public void AddCustomProperty(string PropertyName, object PropertyValue)
        {
            Properties.Add(PropertyName, PropertyValue);
        }

        /// <summary>
        /// Gets the value of a property based on the property name
        /// </summary>
        public object GetValue(string PropertyName)
        {
            foreach(var item in Properties)
            {
                if (item.Key == PropertyName)
                    return item.Value;
            }
            return null;
        }
    }
}
