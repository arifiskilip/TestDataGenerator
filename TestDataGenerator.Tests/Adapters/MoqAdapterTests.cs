using TestDataGenerator.Core.Extensions;
using Moq;
using TestDataGenerator.Adapters.Moq;
using TestDataGenerator.Core.Interfaces;
using TestDataGenerator.Core.Models;

namespace TestDataGenerator.Tests.Adapters
{
    public class MoqAdapterTests
    {
        private readonly ITestDataAdapter<Product> _adapter;

        public MoqAdapterTests()
        {
            _adapter = new MoqTestDataAdapter<Product>();
        }

        [Fact]
        public void Generate_Should_Create_Single_Product()
        {
            // Act
            var product = _adapter.Generate();

            // Assert
            Assert.NotNull(product);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void GenerateMany_Should_Create_Specified_Number_Of_Products(int count)
        {
            // Act
            var products = _adapter.GenerateMany(count).ToList();

            // Assert
            Assert.Equal(count, products.Count);
            Assert.All(products, p => Assert.NotNull(p));
        }

        [Fact]
        public void GetMockRepository_Should_Return_Repository_Mock()
        {
            // Arrange
            var moqAdapter = (MoqTestDataAdapter<Product>)_adapter;

            // Act
            var repository = moqAdapter.GetMockRepository();

            // Assert
            Assert.NotNull(repository);
            Assert.IsType<Mock<IRepository<Product>>>(repository);
        }

        [Fact]
        public void MockRepository_Should_Track_Method_Calls()
        {
            // Arrange
            var moqAdapter = (MoqTestDataAdapter<Product>)_adapter;
            var repository = moqAdapter.GetMockRepository();

            // Act
            var product = _adapter.Generate();
            _ = repository.Object.Get(1);

            // Assert
            repository.Verify(r => r.Get(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GenerateAsync_Extension_Should_Create_Product()
        {
            // Act
            var product = await _adapter.GenerateAsync();

            // Assert
            Assert.NotNull(product);
        }
    }
}
