using System;
using System.Collections.Generic;
using System.Text;

namespace SampleConsoleApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"Product id: {Id}  Name: {Name}  Price: {Price}  Quantity:{Quantity}";
        }
    }
}
