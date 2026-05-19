using SampleConsoleApp.Databases;
using SampleConsoleApp.Exceptions;
using SampleConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SampleConsoleApp.Repository
{
    public class RepositoryProduct : IProductRepository
    {
        private readonly ProductDb _db;
        public RepositoryProduct(ProductDb db)
        {
            _db = db;
        }

        public List<Product> GetAll()
        {
            return _db.Products;
        }

        public Product GetById(int id)
        {
            var pdt = _db.Products.FirstOrDefault(p => p.Id == id);
            if (pdt is null)
            {
                throw new ProductConflict($"Product with id: {id} not found");
            }
            return pdt;
        }
        public Product AddProduct(Product p)
        {
            _db.Products.Add(p);
            return p;
        }

        public string UpdateProduct(int id, Product p)
        {
            var product = _db.Products.FirstOrDefault(pdt=> pdt.Id == id);
            if(product is null)
            {
                throw new ProductConflict($"Product {id} not found");
            }
            product.Name = p.Name;
            product.Price = p.Price;
            product.Quantity = p.Quantity;
            return "Updated successfully";
        }

        public string DeleteProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(pdt => pdt.Id == id);
            if (product is null)
            {
                throw new ProductConflict($"Product {id} not found");
            }
            _db.Products.Remove(product);
            return "Deleted successfully";
        }
    }
}
