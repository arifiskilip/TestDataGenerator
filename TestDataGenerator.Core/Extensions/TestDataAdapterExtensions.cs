using TestDataGenerator.Core.Interfaces;

namespace TestDataGenerator.Core.Extensions
{
    public static class TestDataAdapterExtensions
    {
        public static T GenerateWithCustomization<T>(
        this ITestDataAdapter<T> adapter,
        Action<T> customization) where T : class
        {
            var entity = adapter.Generate();
            customization(entity);
            return entity;
        }

        public static IEnumerable<T> GenerateManyWithCustomization<T>(
            this ITestDataAdapter<T> adapter,
            int count,
            Action<T> customization) where T : class
        {
            var entities = adapter.GenerateMany(count);
            foreach (var entity in entities)
            {
                customization(entity);
            }
            return entities;
        }

        public static async Task<T> GenerateAsync<T>(
            this ITestDataAdapter<T> adapter) where T : class
        {
            return await Task.FromResult(adapter.Generate());
        }

        public static async Task<IEnumerable<T>> GenerateManyAsync<T>(
            this ITestDataAdapter<T> adapter,
            int count) where T : class
        {
            return await Task.FromResult(adapter.GenerateMany(count));
        }
    }
}
