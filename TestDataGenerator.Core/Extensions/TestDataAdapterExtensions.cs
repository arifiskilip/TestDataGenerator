using System.Reflection;
using TestDataGenerator.Core.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TestDataGenerator.Core.Extensions
{
    public static class TestDataAdapterExtensions
    {
        //Tek bir test verisi(entity) oluşturur
        //Oluşturulan veriye özel düzenlemeler yapabilmenizi sağlar
        //Örnek kullanım
        //var user = userAdapter.GenerateWithCustomization(u => {
        //    u.Name = "Test";
        //    u.Age = 25;
        //});
        public static T GenerateWithCustomization<T>(
        this ITestDataAdapter<T> adapter,
        Action<T> customization) where T : class
        {
            var entity = adapter.Generate();
            customization(entity);
            return entity;
        }
        //Birden fazla test verisi oluşturur
        //Her bir oluşturulan veriye özel düzenlemeler yapabilmenizi sağlar
        //Örnek kullanım:
        //var users = userAdapter.GenerateManyWithCustomization(3, u => {
        //    u.IsActive = true;
        //    u.CreatedDate = DateTime.Now;
        //});
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

        //Tek bir test verisini asenkron olarak oluşturur
        //Aslında senkron Generate işlemini Task ile sarar
        //Örnek kullanım:
        //var user = await userAdapter.GenerateAsync();
        public static async Task<T> GenerateAsync<T>(
            this ITestDataAdapter<T> adapter) where T : class
        {
            return await Task.FromResult(adapter.Generate());
        }
        //Birden fazla test verisini asenkron olarak oluşturur
        //Yine senkron GenerateMany işlemini Task ile sarar
        //Örnek kullanım:
        //var users = await userAdapter.GenerateManyAsync(5);
        public static async Task<IEnumerable<T>> GenerateManyAsync<T>(
            this ITestDataAdapter<T> adapter,
            int count) where T : class
        {
            return await Task.FromResult(adapter.GenerateMany(count));
        }
    }
}
