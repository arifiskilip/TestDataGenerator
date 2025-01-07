using Bogus;
using TestDataGenerator.Adapters.Base;
using TestDataGenerator.Core.Models;

namespace TestDataGenerator.Adapters.Faker
{
    public class FakerTestDataAdapter<T> : BaseTestDataAdapter<T> where T : class
    {
        private readonly Faker<T> _faker;

        public FakerTestDataAdapter() 
        {
            _faker = new Faker<T>();
            ConfigureFaker();
        }

        private void ConfigureFaker()
        {
            if (typeof(T) == typeof(Product))
            {
                var productFaker = _faker as Faker<Product>;
                productFaker
                    .RuleFor(p => p.Id, f => f.Random.Number(1, 100))
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
                    .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(p => p.CreatedDate, f => f.Date.Past())
                    .RuleFor(p => p.IsActive, f => f.Random.Bool());
            }
        }

        public override T Generate()
        {
            return _faker.Generate();
        }

        public override IEnumerable<T> GenerateMany(int count)
        {
            return _faker.Generate(count);
        }

        public Faker<T> GetFaker() => _faker;
    }
}
