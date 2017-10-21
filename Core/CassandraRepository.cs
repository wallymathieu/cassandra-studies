using System;
using System.Collections.Generic;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Core.Domain;

namespace Core
{
    public class CassandraMappings : Mappings
    {
        public CassandraMappings()
        {

            // Define mappings in the constructor of your class
            // that inherits from Mappings
            For<Customer>()
               .TableName("customers")
               ;
            For<Order>()
               .TableName("orders")
               ;
            For<Product>()
               .TableName("products")
               ;
        }
    }

    public class CassandraRepository : IRepository
    {
        private readonly ISession session;
        private readonly Mapper mapper;
        //private readonly MappingConfiguration configuration = new MappingConfiguration();

        public CassandraRepository(ISession session)
        {
            var config = new MappingConfiguration().Define<CassandraMappings>();
            new Table<Customer>(session, config).CreateIfNotExists();
            new Table<Order>(session, config).CreateIfNotExists();
            new Table<Product>(session, config).CreateIfNotExists();

            //configuration.Define<CassandraMappings>();
            mapper = new Mapper(session);//, configuration);
        }

        public Customer GetCustomer(int id) =>
            mapper.Single<Customer>("SELECT * FROM customers WHERE id = ?", id);

        public Order GetOrder(int id) =>
             mapper.Single<Order>("SELECT * FROM orders WHERE id = ?", id);

        public Product GetProduct(int id) =>
             mapper.Single<Product>("SELECT * FROM products WHERE id = ?", id);


        public void Save(Product obj) => mapper.Insert(obj);

        public void Save(Order obj) => mapper.Insert(obj);

        public void Save(Customer obj) => mapper.Insert(obj);
    }
}
