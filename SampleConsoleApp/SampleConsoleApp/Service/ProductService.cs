using SampleConsoleApp.Databases;
using SampleConsoleApp.Exceptions;
using SampleConsoleApp.Models;
using SampleConsoleApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleConsoleApp.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo)
        {
            this._repo = repo;
        }

        public List<Product> GetAll()
        {
            return _repo.GetAll();
        }

        public Product GetById(int id)
        {
            return _repo.GetById(id);
        }
        public Product AddProduct(Product p)
        {
            return _repo.AddProduct(p);
        }

        public string UpdateProduct(int id, Product p)
        {
            return _repo.UpdateProduct(id, p);
        }

        public string DeleteProduct(int id)
        {
            return _repo.DeleteProduct(id);
        }
    }
}
