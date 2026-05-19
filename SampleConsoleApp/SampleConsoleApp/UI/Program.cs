
using SampleConsoleApp.Models;
using SampleConsoleApp.Exceptions;
using SampleConsoleApp.Service;
using Microsoft.Extensions.DependencyInjection;
using SampleConsoleApp.Databases;
using SampleConsoleApp.Repository;

var services = new ServiceCollection();
services.AddSingleton<ProductDb>();
services.AddScoped<IProductRepository, RepositoryProduct>();
services.AddScoped<IProductService, ProductService>();


var provider = services.BuildServiceProvider();
var db = provider.GetRequiredService<ProductDb>();
IProductService productService = provider.GetRequiredService<IProductService>();

while (true)
{
    Console.WriteLine("1. Display All Products");
    Console.WriteLine("2. Get Product by Id");
    Console.WriteLine("3. Add Product");
    Console.WriteLine("4. Delete Product");
    Console.WriteLine("5. Exit");
    int choice = Convert.ToInt32(Console.ReadLine());
    switch (choice)
    {
        case 1:
            var products = productService.GetAll();
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
            break;
        case 2:
            try
            {
                Console.Write("Enter Product Id: ");
                int id = Convert.ToInt32(Console.ReadLine());
                var product = productService.GetById(id);
                Console.WriteLine(product);
                Console.WriteLine();
            }
            catch (ProductConflict ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
            break;
        case 3:
            Product pro = new Product();
            Console.WriteLine("Enter Product Id: ");
            pro.Id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Product Name: ");
            pro.Name = Console.ReadLine()!;
            Console.WriteLine("Enter Price: ");
            pro.Price = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter Quantity: ");
            pro.Quantity = Convert.ToInt32(Console.ReadLine());
            var result = productService.AddProduct(pro);
            Console.WriteLine(result);
            Console.WriteLine();
            break;

        case 4:
            try
            {
                Console.Write("Enter Product Id: ");
                int id = Convert.ToInt32(Console.ReadLine());
                var res = productService.DeleteProduct(id);
                Console.WriteLine(res);
                Console.WriteLine();
            }
            catch (ProductConflict ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
            break;

        case 5:
            return;
    }


}
