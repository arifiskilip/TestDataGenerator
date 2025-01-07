﻿using Bogus;
using TestDataGenerator.Adapters.Faker;
using TestDataGenerator.Core.Interfaces;
using TestDataGenerator.Core.Models;
using TestDataGenerator.Core.Extensions;

namespace TestDataGenerator.Tests.Adapters
{
    public class FakerAdapterTests
    {
        private readonly ITestDataAdapter<Product> _adapter;

        public FakerAdapterTests()
        {
            _adapter = new FakerTestDataAdapter<Product>();
        }

        [Fact]
        public void Generate_Should_Create_Product_With_Valid_Data()
        {
            // Act
            var product = _adapter.Generate();

            // Assert
            Assert.NotNull(product);
            Assert.NotEqual(0, product.Id);
            Assert.NotEmpty(product.Name);
            Assert.NotEmpty(product.Description);
            Assert.True(product.Price > 0);
            Assert.NotEqual(default, product.CreatedDate);
            
        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        [InlineData(15)]
        public void GenerateMany_Should_Create_Unique_Products(int count)
        {
            // Act
            var products = _adapter.GenerateMany(count).ToList();

            // Assert
            Assert.Equal(count, products.Count);
            Assert.Equal(count, products.Select(p => p.Name).Distinct().Count());
            
        }

        [Fact]
        public void GetFaker_Should_Return_Configured_Faker()
        {
            // Arrange
            var fakerAdapter = (FakerTestDataAdapter<Product>)_adapter;

            // Act
            var faker = fakerAdapter.GetFaker();

            // Assert
            Assert.NotNull(faker);
            Assert.IsType<Faker<Product>>(faker);
        }

        [Fact]
        public void Generate_Should_Respect_Custom_Rules()
        {
            // Arrange
            var fakerAdapter = (FakerTestDataAdapter<Product>)_adapter;
            var faker = fakerAdapter.GetFaker();
            faker.RuleFor(p => p.Price, f => 999.99m);

            // Act
            var product = _adapter.Generate();

            // Assert
            Assert.Equal(999.99m, product.Price);
        }

        [Fact]
        public async Task GenerateManyAsync_Extension_Should_Create_Products()
        {
            // Act
            var products = await _adapter.GenerateManyAsync(5);

            // Assert
            Assert.Equal(5, products.Count());
        }
    }
}
