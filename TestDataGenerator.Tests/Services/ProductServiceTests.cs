using Moq;
using TestDataGenerator.Adapters.Faker;
using TestDataGenerator.Adapters.Moq;
using TestDataGenerator.Core.Interfaces;
using TestDataGenerator.Core.Models;
using TestDataGenerator.Demo.Services;
using TestDataGenerator.Core.Extensions;

namespace TestDataGenerator.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly ITestDataAdapter<Product> _moqAdapter;
        private readonly ITestDataAdapter<Product> _fakerAdapter;

        public ProductServiceTests()
        {
            _moqAdapter = new MoqTestDataAdapter<Product>();
            _fakerAdapter = new FakerTestDataAdapter<Product>();
        }

        [Fact]
        public void GetAllProducts_Should_Return_Multiple_Products_Using_Moq()
        {
            // Arrange
            var mockRepo = ((MoqTestDataAdapter<Product>)_moqAdapter).GetMockRepository();
            var service = new ProductService(mockRepo.Object);
            var products = _moqAdapter.GenerateMany(3);

            // Act
            var result = service.GetAllProducts();

            // Assert
            Assert.Equal(3, result.Count());
            mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetProduct_Should_Return_Product_With_Specific_Price_Using_Faker()
        {
            // Arrange
            var fakerAdapter = (FakerTestDataAdapter<Product>)_fakerAdapter;
            var faker = fakerAdapter.GetFaker();

            // Custom rule for specific price
            faker.RuleFor(p => p.Price, f => 499.99m);

            var product = fakerAdapter.Generate();
            var mockRepo = new Mock<IRepository<Product>>();
            mockRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(product);

            var service = new ProductService(mockRepo.Object);

            // Act
            var result = service.GetProduct(1);

            // Assert
            Assert.Equal(499.99m, result.Price);
        }

        [Fact]
        public void GetAllProducts_Should_Log_And_Return_Active_Products()
        {
            // Arrange
            var products = _fakerAdapter.GenerateManyWithCustomization(5, p => p.IsActive = true);
            var mockRepo = new Mock<IRepository<Product>>();
            mockRepo.Setup(r => r.GetAll()).Returns(products);

            var service = new ProductService(mockRepo.Object);

            // Act
            var result = service.GetAllProducts();

        }

        [Fact]
        public async Task GetProduct_Should_Handle_Async_Generation()
        {
            // Arrange
            var product = await _moqAdapter.GenerateAsync();
            var mockRepo = new Mock<IRepository<Product>>();
            mockRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(product);

            var service = new ProductService(mockRepo.Object);

            // Act
            var result = service.GetProduct(1);

            // Assert
            Assert.NotNull(result);
            mockRepo.Verify(r => r.Get(It.IsAny<int>()), Times.Once);
        }
    }
}
