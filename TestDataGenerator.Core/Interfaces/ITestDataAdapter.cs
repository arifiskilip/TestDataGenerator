namespace TestDataGenerator.Core.Interfaces
{
    public interface ITestDataAdapter<T> where T : class
    {
        T Generate();
        IEnumerable<T> GenerateMany(int count);
    }
}
