using System.Linq;
using Core.Domain;
using Xunit;

namespace Tests
{
    public class CustomerDataTests
    {
        private static IRepository _repository;
        private static ObjectContainer _container;
        static CustomerDataTests()
        {
            _container = new ObjectContainer();
            //_container.Boot();
            _repository = _container.GetRepository();
            var commands = new GetCommands().Get();
            _container.HandleAll(commands);
        }
        [Fact]
        public void CanGetCustomerById()
        {
            Assert.NotNull(_repository.GetCustomer(1));
        }

        [Fact]
        public void CanGetProductById()
        {
            Assert.NotNull(_repository.GetProduct(1));
        }
        [Fact]
        public void OrderContainsProduct()
        {
            Assert.True(_repository.GetOrder(1).Products.Contains(1));
        }
        /*[Fact]
        public void OrderHasACustomer()
        {
            Assert.True(!string.IsNullOrEmpty( _repository.GetTheCustomerOrder(1).Firstname));
        }*/
       
    }
}
