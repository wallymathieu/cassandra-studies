using System.Collections.Generic;

namespace Core.Domain
{
    public interface IRepository
    {
        Customer GetCustomer(int id);
        Product GetProduct(int id);
        Order GetOrder(int id);
        void Save(Product product);
        void Save(Order order);
        void Save(Customer customer);
    }
}
