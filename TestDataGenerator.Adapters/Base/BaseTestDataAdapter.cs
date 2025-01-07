using TestDataGenerator.Core.Interfaces;

namespace TestDataGenerator.Adapters.Base
{
    public abstract class BaseTestDataAdapter<T> : ITestDataAdapter<T> where T : class
    {
        public abstract T Generate();
        public abstract IEnumerable<T> GenerateMany(int count);
    }
}
