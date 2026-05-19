using SampleConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleConsoleApp.Service
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product AddProduct(Product p);
        string UpdateProduct(int id, Product p);
        string DeleteProduct(int id);
    }
}
