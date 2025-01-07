using Moq;
using TestDataGenerator.Adapters.Base;
using TestDataGenerator.Core.Interfaces;

namespace TestDataGenerator.Adapters.Moq
{
    public class MoqTestDataAdapter<T> : BaseTestDataAdapter<T> where T : class
    {
        private readonly Mock<IRepository<T>> _repository;

        public MoqTestDataAdapter() 
        {
            _repository = new Mock<IRepository<T>>();
        }

        public override T Generate()
        {

            var entity = Activator.CreateInstance<T>();
            _repository.Setup(r => r.Get(It.IsAny<int>()))
                      .Returns(entity);

            return entity;
        }

        public override IEnumerable<T> GenerateMany(int count)
        {
            var entities = Enumerable.Range(1, count)
                                   .Select(_ => Activator.CreateInstance<T>())
                                   .ToList();

            _repository.Setup(r => r.GetAll())
                      .Returns(entities);

            return entities;
        }

        public Mock<IRepository<T>> GetMockRepository() => _repository;
    }
}
