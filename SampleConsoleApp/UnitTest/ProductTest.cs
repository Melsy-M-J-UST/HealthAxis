using SampleConsoleApp.Databases;
using SampleConsoleApp.Exceptions;
using SampleConsoleApp.Models;
using SampleConsoleApp.Repository;

namespace UnitTest
{
    public class ProductTest
    {
        private readonly ProductDb _db;
        private readonly RepositoryProduct _repo;

        public ProductTest()
        {
            _db = new ProductDb();
            _repo = new RepositoryProduct(_db);
        }
        
        [Fact]
        public void GetAll_ShouldReturnAllRecords()
        {
            var result = _repo.GetAll();
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("tv", result[1].Name, ignoreCase:true);

        }

        [Theory]
        [InlineData(1, "Phone")]
        [InlineData(3, "Laptop")]
        public void GetById_GivenId_ShouldReturnProduct(int id, string name)
        {
            var result = _repo.GetById(id);
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
        }

        [Fact]
        public void UpdateProduct_GivenId_ShouldUpdate()
        {
            Product product = new Product() { Id = 2, Name = "Trimmer", Price = 2500, Quantity = 2 };
            var result = _repo.UpdateProduct(2, product);
            Assert.Equal("Updated successfully", result);
            Assert.Equal("Trimmer", _db.Products[1].Name);
            Assert.Throws<ProductConflict>(() => _repo.UpdateProduct(5, product));
        }

        [Theory]
        [InlineData(2, "Deleted successfully")]

        public void DeleteProduct_GivenId_ShouldDelete(int id, string output)
        {
            var result = _repo.DeleteProduct(id);
            Assert.Equal("Deleted successfully", result);
            Assert.Equal(2, _db.Products.Count);
            Assert.Throws<ProductConflict>(() => _repo.DeleteProduct(5));
        }
    }
}
