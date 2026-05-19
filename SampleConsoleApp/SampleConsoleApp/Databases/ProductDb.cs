using SampleConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleConsoleApp.Databases
{
    public class ProductDb
    {
        public List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Phone", Price = 35000, Quantity = 3 },
            new Product { Id = 2, Name = "TV", Price = 45000, Quantity = 2 },
            new Product { Id = 3, Name = "Laptop", Price = 40000, Quantity = 4 }
        };
    }
}
