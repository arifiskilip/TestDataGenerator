using TestDataGenerator.Core.Interfaces;
using TestDataGenerator.Core.Models;

namespace TestDataGenerator.Demo.Services
{
    public class ProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public Product GetProduct(int id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _repository.GetAll();
        }
    }
}
